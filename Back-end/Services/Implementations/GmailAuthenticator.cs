using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.Util.Store;


public class GmailAuthenticator
{
    private readonly string _gmailAccount;
    private readonly ClientSecrets _clientSecrets;
    private readonly ILogger<GmailAuthenticator> _logger;

    public GmailAuthenticator(IConfiguration configuration, ILogger<GmailAuthenticator> logger)
    {
        // Lendo os valores do appsettings.json
        _gmailAccount = configuration["GmailSettings:GMailAccount"]
            ?? throw new ArgumentNullException("GmailSettings:GmailAccount");

        _clientSecrets = new ClientSecrets
        {
            ClientId = configuration["GmailSettings:ClientId"]
                    ?? throw new ArgumentNullException("GmailSettings:ClientId"),

            ClientSecret = configuration["GmailSettings:ClientSecret"]
                    ?? throw new ArgumentNullException("GmailSettings:ClientSecret")
        };

        _logger = logger;
    }

    public async Task<GmailService> AuthenticateAsync()
    {
        try
        {
            var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                // Cache tokens in ~/.local/share/google-filedatastore/CredentialCacheFolder on Linux/Mac
                DataStore = new FileDataStore("CredentialCacheFolder", false),
                Scopes = new[] { GmailService.Scope.GmailSend }, 
                ClientSecrets = _clientSecrets,
                LoginHint = _gmailAccount
            });

            // Note: For a web app, you'll want to use AuthorizationCodeWebApp instead.
            var codeReceiver = new LocalServerCodeReceiver();
            var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);
            var credential = await authCode.AuthorizeAsync(_gmailAccount, CancellationToken.None);

            if (credential.Token.IsStale)
                await credential.RefreshTokenAsync(CancellationToken.None);

            return new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "EventosPro"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to authenticate with Gmail");
            throw;
        }
    }

    private async Task<UserCredential> GetUserCredentialAsync(GoogleAuthorizationCodeFlow flow)
    {
        var codeReceiver = new LocalServerCodeReceiver();
        var authCode = new AuthorizationCodeInstalledApp(flow, codeReceiver);

        var credential = await authCode.AuthorizeAsync(_gmailAccount, CancellationToken.None);

        if (credential.Token.IsStale)
        {
            await credential.RefreshTokenAsync(CancellationToken.None);
        }

        return credential;
    }
}
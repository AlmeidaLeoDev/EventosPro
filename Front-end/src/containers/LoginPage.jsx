import { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import { AuthContext } from '../context/AuthContext';
import {
  Container,
  Form,
  Title,
  InputGroup,
  Label,
  Input,
  Button,
  LinkText,
} from '../components/LoginStyles';



function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(true);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      const response = await api.post('/api/users/login', { email, password, rememberMe});

      // Extrai o token e dados do usuário da resposta
      const { token, ...userData } = response.data;

      // Armazena o token e atualiza o contexto
      localStorage.setItem('authToken', token);
      login(userData, token);

      login(response.data);

      navigate('/home');
    } catch (error) {
      setError(error.response?.data?.message || 'Falha no login. Verifique suas credenciais.');
      console.error('Login error:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container>
      <Form onSubmit={handleLogin}>
        <Title>Login</Title>

        {error && <div style={{ color: 'red', marginBottom: '1rem' }}>{error}</div>}

        <InputGroup>
          <Label>Email:</Label>
          <Input
            type="email"
            value={email}
            required
            onChange={(e) => setEmail(e.target.value)}
            disabled={loading}
          />
        </InputGroup>

        <InputGroup>
          <Label>Senha:</Label>
          <Input
            type="password"
            value={password}
            required
            onChange={(e) => setPassword(e.target.value)}
            disabled={loading}
          />
        </InputGroup>

        <InputGroup>
          <Label>
            <input
              type="checkbox"
              checked={rememberMe}
              onChange={(e) => setRememberMe(e.target.checked)}
              disabled={loading}
            />
            Lembrar de mim
          </Label>
        </InputGroup>

        <Button type="submit" disabled={loading}>
          {loading ? 'Carregando...' : 'Entrar'}
        </Button>

        <LinkText>
          Não tem conta? <a href="/register">Cadastre-se</a>
        </LinkText>
        
        <LinkText>
          Esqueceu sua senha? <a href="/forgot-password">Recuperar senha</a>
        </LinkText>
      </Form>
    </Container>
  );
}

export default LoginPage;

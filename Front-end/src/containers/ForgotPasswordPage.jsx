import{ useState } from 'react';
import api from '../services/api';
import {
  Container,
  AuthForm,
  Title,
  FormGroup,
  Input,
  SubmitButton,
  BackLink
} from '../components/ForgotPasswordStyles';

function ForgotPasswordPage() {
  const [email, setEmail] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleForgotPassword = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    try {
      await api.post('/api/users/forgot-password', { email });
      alert('Se o email existir, você receberá instruções para resetar sua senha.');
    } catch (error) {
      console.error('Error forgot password:', error.response?.data || error.message);
      alert(error.response?.data || 'Erro no processamento.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Container>
      <AuthForm onSubmit={handleForgotPassword}>
        <Title>Recuperar Senha</Title>
        
        <FormGroup>
          <label>Email:</label>
          <Input
            type="email"
            value={email}
            required
            onChange={(e) => setEmail(e.target.value)}
            placeholder="exemplo@email.com"
          />
        </FormGroup>

        <SubmitButton type="submit" disabled={isSubmitting}>
          {isSubmitting ? 'Enviando...' : 'Enviar Instruções'}
        </SubmitButton>

        <BackLink href="/login">
          ← Voltar para o Login
        </BackLink>
      </AuthForm>
    </Container>
  );
}

export default ForgotPasswordPage;

import { useLocation, useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import api from '../services/api';
import {Container, FormCard, Title, FormGroup, InputLabel, PasswordInput, SubmitButton, LinkText} from '../components/ResetPasswordStyles'


function ResetPasswordPage() {
  const [newPassword, setNewPassword] = useState('');
  const [confirmNewPassword, setConfirmNewPassword] = useState('');
  const navigate = useNavigate();
  
  const location = useLocation();
  const [email, setEmail] = useState('');
  const [token, setToken] = useState('');

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    setToken(params.get('token') || '');
    setEmail(params.get('email') || '');
  }, [location]);

  const handleResetPassword = async (e) => {
    e.preventDefault();
    try {
      await api.post('/users/reset-password', { email, token, newPassword, confirmNewPassword });
      alert('Senha redefinida com sucesso!');
      navigate('/login');
    } catch (error) {
      console.error('Error resetting password:', error.response?.data || error.message);
      alert(error.response?.data || 'Erro no reset da senha.');
    }
  };

  return (
    <Container>
      <FormCard>
        <Title>Redefinir Senha</Title>
        <form onSubmit={handleResetPassword}>
          <FormGroup>
            <InputLabel>Nova Senha</InputLabel>
            <PasswordInput
              type="password"
              value={newPassword}
              required
              onChange={(e) => setNewPassword(e.target.value)}
              placeholder="Digite sua nova senha"
            />
          </FormGroup>

          <FormGroup>
            <InputLabel>Confirmar Nova Senha</InputLabel>
            <PasswordInput
              type="password"
              value={confirmNewPassword}
              required
              onChange={(e) => setConfirmNewPassword(e.target.value)}
              placeholder="Confirme sua nova senha"
            />
          </FormGroup>

          <SubmitButton type="submit">Redefinir Senha</SubmitButton>
        </form>

        <LinkText>
          Lembrou da senha? <a href="/login">Faça login</a>
        </LinkText>
      </FormCard>
    </Container>
  );
}

export default ResetPasswordPage;

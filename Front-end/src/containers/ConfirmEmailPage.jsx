import { useState, useEffect, useCallback } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import api from '../services/api';
import { Container, Heading, StatusMessage } from '../components/ConfirmEmailStyles';


function ConfirmEmailPage() {
  const navigate = useNavigate();
  const location = useLocation();

  const [status, setStatus] = useState(null);

  const searchParams = new URLSearchParams(location.search);
  const token = searchParams.get('token');
  const email = searchParams.get('email');

  const confirmEmail = useCallback(async (token, email) => {
    try {
      await api.post('/users/confirm-email', { email, token });
      setStatus('Email confirmado com sucesso!');
      setTimeout(() => navigate('/login'), 2000);
    } catch (error) {
      console.error('Error in email confirmation:', error.response?.data || error.message);
      setStatus('Erro ao confirmar o e-mail.');
    }
  }, [navigate]);

  useEffect(() => {
    if (token && email) {
      confirmEmail(token, email);
    }
  }, [token, email, confirmEmail]);

  return (
    <Container>
      <Heading>Confirmação de E-mail</Heading>
      {status ? (
        <StatusMessage success={status === 'Email confirmado com sucesso!'}>{status}</StatusMessage>
      ) : (
        <StatusMessage>Confirmando seu e-mail...</StatusMessage>
      )}
    </Container>
  );
}

export default ConfirmEmailPage;
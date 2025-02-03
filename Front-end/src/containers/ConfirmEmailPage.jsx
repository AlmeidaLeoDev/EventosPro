import { useState, useEffect, useCallback } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import api from '../services/api';

function ConfirmEmailPage() {
  const navigate = useNavigate();
  const location = useLocation();

  const [status, setStatus] = useState(null);

  const searchParams = new URLSearchParams(location.search);
  const token = searchParams.get('token');
  const email = searchParams.get('email'); 

  // Usando useCallback para garantir que a função confirmEmail não seja recriada em cada renderização
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
    <div className="container">
      <h2>Confirmação de E-mail</h2>
      {status ? (
        <p>{status}</p>
      ) : (
        <p>Confirmando seu e-mail...</p>
      )}
    </div>
  );
}

export default ConfirmEmailPage;

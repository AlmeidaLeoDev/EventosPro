import { useLocation, useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import api from '../services/api';

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
    <div className="container">
      <h2>Resetar Senha</h2>
      <form onSubmit={handleResetPassword} className="auth-form">
        <div>
          <label>Nova Senha:</label>
          <input type="password" value={newPassword} required onChange={(e) => setNewPassword(e.target.value)} />
        </div>
        <div>
          <label>Confirmar Nova Senha:</label>
          <input type="password" value={confirmNewPassword} required onChange={(e) => setConfirmNewPassword(e.target.value)} />
        </div>
        <button type="submit">Resetar Senha</button>
      </form>
    </div>
  );
}

export default ResetPasswordPage;

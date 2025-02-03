import{ useState } from 'react';
import api from '../services/api';

function ForgotPasswordPage() {
  const [email, setEmail] = useState('');

  const handleForgotPassword = async (e) => {
    e.preventDefault();
    try {
      await api.post('/users/forgot-password', { email });
      alert('Se o email existir, você receberá instruções para resetar sua senha.');
    } catch (error) {
      console.error('Error forgot password:', error.response?.data || error.message);
      alert(error.response?.data || 'Erro no processamento.');
    }
  };

  return (
    <div className="container">
      <h2>Recuperar Senha</h2>
      <form onSubmit={handleForgotPassword} className="auth-form">
        <div>
          <label>Email:</label>
          <input type="email" value={email} required onChange={(e) => setEmail(e.target.value)} />
        </div>
        <button type="submit">Enviar Instruções</button>
      </form>
    </div>
  );
}

export default ForgotPasswordPage;

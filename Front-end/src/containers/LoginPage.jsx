import { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import { AuthContext } from '../context/AuthContext';

function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const response = await api.post('/users/login', { email, password, rememberMe: true });
      login(response.data);
      navigate('/');
    } catch (error) {
      console.error('Login error:', error.response?.data || error.message);
      alert('Falha ao realizar login.');
    }
  };

  return (
    <div className="container">
      <h2>Login</h2>
      <form onSubmit={handleLogin} className="auth-form">
        <div>
          <label>Email:</label>
          <input type="email" value={email} required onChange={(e) => setEmail(e.target.value)} />
        </div>
        <div>
          <label>Senha:</label>
          <input type="password" value={password} required onChange={(e) => setPassword(e.target.value)} />
        </div>
        <button type="submit">Entrar</button>
      </form>
      <p>
        Não tem conta? <a href="/register">Cadastre-se</a>
      </p>
      <p>
        Esqueceu sua senha? <a href="/forgot-password">Recuperar senha</a>
      </p>
    </div>
  );
}

export default LoginPage;

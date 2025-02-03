import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';

function RegisterPage() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      const payload = { name, email, password, confirmPassword };
      await api.post('/users/register', payload);
      alert('Cadastro realizado com sucesso! Verifique seu email para confirmação.');
      navigate('/login');
    } catch (error) {
      console.error('Error in registration:', error.response?.data || error.message);
      alert(error.response?.data || 'Erro no cadastro.');
    }
  };

  return (
    <div className="container">
      <h2>Cadastro</h2>
      <form onSubmit={handleRegister} className="auth-form">
        <div>
          <label>Nome:</label>
          <input type="text" value={name} required onChange={(e) => setName(e.target.value)} />
        </div>
        <div>
          <label>Email:</label>
          <input type="email" value={email} required onChange={(e) => setEmail(e.target.value)} />
        </div>
        <div>
          <label>Senha:</label>
          <input type="password" value={password} required onChange={(e) => setPassword(e.target.value)} />
        </div>
        <div>
          <label>Confirmar Senha:</label>
          <input type="password" value={confirmPassword} required onChange={(e) => setConfirmPassword(e.target.value)} />
        </div>
        <button type="submit">Cadastrar</button>
      </form>
      <p>
        Já tem conta? <a href="/login">Entre</a>
      </p>
    </div>
  );
}

export default RegisterPage;

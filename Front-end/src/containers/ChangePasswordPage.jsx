import { useState } from 'react';
import api from '../services/api';

function ChangePasswordPage() {
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmNewPassword, setConfirmNewPassword] = useState('');

  const handleChangePassword = async (e) => {
    e.preventDefault();
    try {
      await api.post('/users/change-password', { currentPassword, newPassword, confirmNewPassword });
      alert('Senha alterada com sucesso!');
    } catch (error) {
      console.error('Error changing password:', error.response?.data || error.message);
      alert(error.response?.data || 'Erro ao alterar senha.');
    }
  };

  return (
    <div className="container">
      <h2>Alterar Senha</h2>
      <form onSubmit={handleChangePassword} className="auth-form">
        <div>
          <label>Senha Atual:</label>
          <input type="password" value={currentPassword} required onChange={(e) => setCurrentPassword(e.target.value)} />
        </div>
        <div>
          <label>Nova Senha:</label>
          <input type="password" value={newPassword} required onChange={(e) => setNewPassword(e.target.value)} />
        </div>
        <div>
          <label>Confirmar Nova Senha:</label>
          <input type="password" value={confirmNewPassword} required onChange={(e) => setConfirmNewPassword(e.target.value)} />
        </div>
        <button type="submit">Alterar Senha</button>
      </form>
    </div>
  );
}

export default ChangePasswordPage;

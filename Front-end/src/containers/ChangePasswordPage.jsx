import { useState } from 'react';
import api from '../services/api';
import { Container, Title, Form, Label, Input, Button } from '../components/ChangePasswordStyle';

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
    <Container>
      <Title>Alterar Senha</Title>
      <Form onSubmit={handleChangePassword}>
        <div>
          <Label>Senha Atual:</Label>
          <Input type="password" value={currentPassword} required onChange={(e) => setCurrentPassword(e.target.value)} />
        </div>
        <div>
          <Label>Nova Senha:</Label>
          <Input type="password" value={newPassword} required onChange={(e) => setNewPassword(e.target.value)} />
        </div>
        <div>
          <Label>Confirmar Nova Senha:</Label>
          <Input type="password" value={confirmNewPassword} required onChange={(e) => setConfirmNewPassword(e.target.value)} />
        </div>
        <Button type="submit">Alterar Senha</Button>
      </Form>
    </Container>
  );
}

export default ChangePasswordPage;

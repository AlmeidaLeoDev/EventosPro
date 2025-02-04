import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';

import {
  Container,
  Form,
  Title,
  InputGroup,
  Label,
  Input,
  Button,
  LinkText,
} from '../components/RegisterStyles';

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
    <Container>
      <Form onSubmit={handleRegister}>
        <Title>Cadastro</Title>
        <InputGroup>
          <Label>Nome:</Label>
          <Input
            type="text"
            value={name}
            required
            onChange={(e) => setName(e.target.value)}
          />
        </InputGroup>
        <InputGroup>
          <Label>Email:</Label>
          <Input
            type="email"
            value={email}
            required
            onChange={(e) => setEmail(e.target.value)}
          />
        </InputGroup>
        <InputGroup>
          <Label>Senha:</Label>
          <Input
            type="password"
            value={password}
            required
            onChange={(e) => setPassword(e.target.value)}
          />
        </InputGroup>
        <InputGroup>
          <Label>Confirmar Senha:</Label>
          <Input
            type="password"
            value={confirmPassword}
            required
            onChange={(e) => setConfirmPassword(e.target.value)}
          />
        </InputGroup>
        <Button type="submit">Cadastrar</Button>
        <LinkText>
          Já tem conta? <a href="/login">Entre</a>
        </LinkText>
      </Form>
    </Container>
  );
}

export default RegisterPage;

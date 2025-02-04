import { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import { AuthContext } from '../context/AuthContext';
import {
  Container,
  Form,
  Title,
  InputGroup,
  Label,
  Input,
  Button,
  LinkText,
} from '../components/LoginStyles';



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
    <Container>
      <Form onSubmit={handleLogin}>
        <Title>Login</Title>
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
        <Button type="submit">Entrar</Button>
        <LinkText>
          Não tem conta? <a href="/register">Cadastre-se</a>
        </LinkText>
        <LinkText>
          Esqueceu sua senha? <a href="/forgot-password">Recuperar senha</a>
        </LinkText>
      </Form>
    </Container>
  );
}

export default LoginPage;

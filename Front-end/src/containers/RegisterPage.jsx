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
  ErrorMessage,
} from '../components/RegisterStyles';

function RegisterPage() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [passwordErrors, setPasswordErrors] = useState([]);
  const [confirmPasswordError, setConfirmPasswordError] = useState('');
  const navigate = useNavigate();

  const validatePassword = (pass) => {
    const errors = [];
    
    if (pass.length < 8) {
      errors.push("A senha deve ter pelo menos 8 caracteres.");
    }
    if (!/[A-Z]/.test(pass)) {
      errors.push("A senha deve conter pelo menos uma letra maiúscula.");
    }
    if (!/[a-z]/.test(pass)) {
      errors.push("A senha deve conter pelo menos uma letra minúscula.");
    }
    if (!/[0-9]/.test(pass)) {
      errors.push("A senha deve conter pelo menos um número.");
    }
    if (!/[!@#$%^&*(),.?":{}|<>]/.test(pass)) {
      errors.push("A senha deve conter pelo menos um caractere especial.");
    }
    
    return errors;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const passErrors = validatePassword(password);
    const confirmError = password !== confirmPassword ? 
      "As senhas não coincidem." : "";

    setPasswordErrors(passErrors);
    setConfirmPasswordError(confirmError);

    if (passErrors.length > 0 || confirmError) return;

    try {
      const payload = { name, email, password, confirmPassword };
      await api.post('/api/users/register', payload);
      alert('Cadastro realizado com sucesso! Verifique seu email para confirmação.');
      navigate('/login');
    } catch (error) {
      console.error('Error in registration:', error.response?.data || error.message);
      alert(error.response?.data || 'Erro no cadastro.');
    }
  };

  return (
    <Container>
      <Form onSubmit={handleSubmit}>
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
            onBlur={() => setPasswordErrors(validatePassword(password))}
          />
          {passwordErrors.length > 0 && (
            <div>
              {passwordErrors.map((error, index) => (
                <ErrorMessage key={index}>• {error}</ErrorMessage>
              ))}
            </div>
          )}
        </InputGroup>

        <InputGroup>
          <Label>Confirmar Senha:</Label>
          <Input
            type="password"
            value={confirmPassword}
            required
            onChange={(e) => setConfirmPassword(e.target.value)}
            onBlur={() => setConfirmPasswordError(
              password !== confirmPassword ? "As senhas não coincidem." : ""
            )}
          />
          {confirmPasswordError && (
            <ErrorMessage>• {confirmPasswordError}</ErrorMessage>
          )}
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
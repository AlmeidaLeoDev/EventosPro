import styled from 'styled-components';

export const Container = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: #f8f9fa;
  padding: 20px;
`;

export const AuthForm = styled.form`
  background: white;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 0 20px rgba(0,0,0,0.1);
  width: 100%;
  max-width: 400px;
`;

export const Title = styled.h2`
  color: #2d3436;
  text-align: center;
  margin-bottom: 1.5rem;
  font-size: 1.8rem;
`;

export const FormGroup = styled.div`
  margin-bottom: 1.5rem;

  label {
    display: block;
    margin-bottom: 0.5rem;
    color: #636e72;
    font-weight: 500;
  }
`;

export const Input = styled.input`
  width: 100%;
  padding: 0.8rem;
  border: 1px solid #dfe6e9;
  border-radius: 5px;
  font-size: 1rem;
  transition: border-color 0.3s ease;

  &:focus {
    outline: none;
    border-color: #6c5ce7;
    box-shadow: 0 0 0 3px rgba(108,92,231,0.1);
  }
`;

export const SubmitButton = styled.button`
  width: 100%;
  padding: 0.8rem;
  background: #6c5ce7;
  color: white;
  border: none;
  border-radius: 5px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.3s ease;

  &:hover {
    background: #5b4bc4;
  }
`;

export const BackLink = styled.a`
  display: block;
  text-align: center;
  margin-top: 1.5rem;
  color: #6c5ce7;
  text-decoration: none;
  font-size: 0.9rem;

  &:hover {
    text-decoration: underline;
  }
`;
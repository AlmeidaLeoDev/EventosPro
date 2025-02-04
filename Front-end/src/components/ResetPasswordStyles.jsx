import styled from 'styled-components';

export const Container = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  background-color: #f8f9fa;
  padding: 2rem;
`;

export const FormCard = styled.div`
  background: white;
  padding: 2.5rem;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
  width: 100%;
  max-width: 450px;
`;

export const Title = styled.h2`
  color: #2d3748;
  font-size: 1.8rem;
  margin-bottom: 2rem;
  text-align: center;
`;

export const FormGroup = styled.div`
  margin-bottom: 1.5rem;
`;

export const InputLabel = styled.label`
  display: block;
  color: #4a5568;
  font-size: 0.9rem;
  margin-bottom: 0.5rem;
  font-weight: 500;
`;

export const PasswordInput = styled.input`
  width: 100%;
  padding: 0.875rem;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  font-size: 1rem;
  transition: all 0.2s ease;
  background-color: #f7fafc;

  &:focus {
    outline: none;
    border-color: #4299e1;
    box-shadow: 0 0 0 3px rgba(66, 153, 225, 0.2);
    background-color: white;
  }
`;

export const SubmitButton = styled.button`
  width: 100%;
  padding: 0.875rem;
  background-color: #4299e1;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  margin-top: 1rem;

  &:hover {
    background-color: #3182ce;
  }

  &:active {
    transform: scale(0.98);
  }
`;

export const ErrorMessage = styled.div`
  color: #e53e3e;
  font-size: 0.875rem;
  margin-top: 0.5rem;
  text-align: center;
`;

export const LinkText = styled.p`
  text-align: center;
  margin-top: 1.5rem;
  color: #718096;

  a {
    color: #4299e1;
    text-decoration: none;
    font-weight: 500;

    &:hover {
      text-decoration: underline;
    }
  }
`;
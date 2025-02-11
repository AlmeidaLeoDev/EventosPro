import styled from 'styled-components';

export const Container = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  background-color: #f0f2f5;
  padding: 20px;
`;

export const Form = styled.form`
  background: white;
  padding: 40px;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 400px;
  text-align: center;
`;

export const Title = styled.h2`
  margin-bottom: 20px;
  color: #333;
  font-size: 24px;
`;

export const InputGroup = styled.div`
  margin-bottom: 20px;
  text-align: left;
`;

export const Label = styled.label`
  display: block;
  margin-bottom: 5px;
  color: #555;
  font-size: 14px;
`;

export const Input = styled.input`
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 5px;
  font-size: 16px;
  outline: none;
  transition: border-color 0.3s;

  &:focus {
    border-color: #007bff;
  }
`;

export const Button = styled.button`
  width: 100%;
  padding: 12px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 5px;
  font-size: 16px;
  cursor: pointer;
  transition: background-color 0.3s;

  &:hover {
    background-color: #0056b3;
  }
`;

export const LinkText = styled.p`
  margin-top: 15px;
  color: #555;
  font-size: 14px;

  a {
    color: #007bff;
    text-decoration: none;

    &:hover {
      text-decoration: underline;
    }
  }
`;

export const ErrorMessage = styled.span`
  display: block;
  color: #dc3545;
  font-size: 0.875rem;
  margin-top: 5px;
  text-align: left;
`;
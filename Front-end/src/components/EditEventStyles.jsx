import styled from 'styled-components';

export const Container = styled.div`
  padding: 2rem;
  max-width: 800px;
  margin: 0 auto;
`;

export const StyledHeading = styled.h1`
  color: #333;
  text-align: center;
  margin-bottom: 2rem;
`;

export const FormContainer = styled.form`
  background: #f8f9fa;
  padding: 2rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);

  label {
    display: block;
    margin-bottom: 1rem;
    font-weight: 500;
  }

  input {
    width: 100%;
    padding: 0.75rem;
    border: 1px solid #ddd;
    border-radius: 4px;
    margin-top: 0.5rem;
  }
`;

export const FormActions = styled.div`
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 2rem;

  button {
    padding: 0.75rem 1.5rem;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    transition: opacity 0.3s;

    &:hover {
      opacity: 0.9;
    }

    &.danger {
      background: #dc3545;
      color: white;
    }

    &:not(.danger) {
      background: #007bff;
      color: white;
    }
  }
`;
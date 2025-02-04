import styled from 'styled-components';


export const Container = styled.div`
  display: flex;
  min-height: 100vh;
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  padding: 2rem;
  justify-content: center;
  align-items: center;
`;

export const FormCard = styled.div`
  background: white;
  padding: 2.5rem;
  border-radius: 16px;
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 500px;
  text-align: center;
`;

export const Title = styled.h2`
  color: #2d3748;
  font-size: 1.8rem;
  margin-bottom: 2rem;
  font-weight: 600;
  letter-spacing: -0.5px;
`;

export const FormGroup = styled.div`
  margin-bottom: 2rem;
  position: relative;
`;

export const Label = styled.label`
  display: block;
  color: #4a5568;
  font-size: 1rem;
  margin-bottom: 1rem;
  font-weight: 500;
`;

export const StyledSelect = styled.select`
  width: 100%;
  padding: 1rem;
  border: 2px solid #e2e8f0;
  border-radius: 8px;
  font-size: 1rem;
  appearance: none;
  background-color: #f8fafc;
  background-image: url("data:image/svg+xml;charset=UTF-8,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='%234a5568' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'%3e%3cpolyline points='6 9 12 15 18 9'%3e%3c/polyline%3e%3c/svg%3e");
  background-repeat: no-repeat;
  background-position: right 1rem center;
  background-size: 1em;
  transition: all 0.3s ease;

  &:focus {
    outline: none;
    border-color: #4299e1;
    box-shadow: 0 0 0 3px rgba(66, 153, 225, 0.15);
    background-color: white;
  }
`;

export const ErrorMessage = styled.span`
  color: #e53e3e;
  font-size: 0.875rem;
  margin-top: 0.5rem;
  display: block;
`;

export const SubmitButton = styled.button`
  width: 100%;
  padding: 1rem;
  background-color: ${props => props.disabled ? '#cbd5e0' : '#4299e1'};
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 500;
  cursor: ${props => props.disabled ? 'not-allowed' : 'pointer'};
  transition: all 0.3s ease;

  &:hover {
    background-color: ${props => props.disabled ? '#cbd5e0' : '#3182ce'};
  }
`;

export const StatusMessage = styled.div`
  margin-top: 1.5rem;
  padding: 1rem;
  border-radius: 8px;
  background-color: ${({ type }) => (type === 'success' ? '#f0fff4' : '#fff5f5')};
  color: ${({ type }) => (type === 'success' ? '#38a169' : '#c53030')};
  border: 1px solid ${({ type }) => (type === 'success' ? '#c6f6d5' : '#fed7d7')};
`;
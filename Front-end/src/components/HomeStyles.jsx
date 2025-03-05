import styled from 'styled-components';

export const ModalOverlay = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
`;

export const ModalContent = styled.div`
  background: white;
  padding: 2rem;
  border-radius: 8px;
  min-width: 300px;
  max-width: 500px;
  width: 90%;

  h3 {
    margin-bottom: 1rem;
    color: #333;
  }

  p {
    margin-bottom: 1.5rem;
    color: #666;
  }
`;

export const ModalActions = styled.div`
  display: flex;
  gap: 1rem;
  justify-content: flex-end;

  button {
    padding: 0.5rem 1rem;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    transition: opacity 0.2s;

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

const theme = {
    primary: '#007bff',
    danger: '#dc3545',
    text: '#333',
    background: '#f8f9fa'
  };
  
export const Container = styled.div`
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;
  background: ${theme.background};
  padding: 20px;
  @media (max-width: 768px) {
    padding: 10px;
  }
  `;

export const Header = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
  padding: 20px 0;
  border-bottom: 1px solid #eee;
`;

export const Title = styled.h1`
  color: #333;
  font-size: 28px;
  margin: 0;
`;

export const PrimaryButton = styled.button`
  background-color: #007bff;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;
  font-size: 14px;

  &:hover {
    background-color: #0056b3;
  }
`;

export const LogoutButton = styled(PrimaryButton)`
  background-color: #dc3545;

  &:hover {
    background-color: #bb2d3b;
  }
`;

export const CalendarWrapper = styled.div`
  margin-top: 30px;
  padding: 20px;
  background: white;
  border-radius: 10px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.05);
`;

export const CreateEventButton = styled(PrimaryButton)`
  margin-bottom: 20px;
`;

export const LoadingText = styled.p`
  text-align: center;
  color: #666;
  font-size: 18px;
`;
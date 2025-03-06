import styled from 'styled-components';

export const Header = styled.div`
  position: relative;
  height: 80px;
  margin-bottom: 30px;
  padding: 0 30px;
  background: #000000;
  display: grid;
  grid-template-columns: auto 1fr auto;
  align-items: center;
  gap: 20px;

  @media (max-width: 1024px) {
    padding: 0 20px;
    gap: 15px;
  }

  @media (max-width: 768px) {
    height: auto;
    min-height: 70px;
    padding: 10px 15px;
    grid-template-columns: 1fr auto;
    grid-template-areas: 
      "logo logout"
      "title title";
  }
`;

export const Logo = styled.img`
  height: auto;
  width: 60px;
  max-width: 180px;
  object-fit: contain;
  
  @media (max-width: 768px) {
    grid-area: logo;
    width: 45px;
    max-width: 140px;
  }

  @media (max-width: 480px) {
    height: 50px;
    max-width: 120px;
  }
`;

export const Title = styled.h1`
  color: white;
  font-size: 28px;
  margin: 0;
  text-align: center;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  padding: 0 20px;

  @media (max-width: 1024px) {
    font-size: 24px;
  }

  @media (max-width: 768px) {
    grid-area: title;
    font-size: 20px;
    padding: 10px 0 0;
    white-space: normal;
    text-align: left;
  }

  @media (max-width: 480px) {
    font-size: 18px;
    line-height: 1.3;
  }
`;


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
  @media (max-width: 768px) {
    grid-area: logout;
    padding: 8px 12px;
    font-size: 14px;
  }

  @media (max-width: 480px) {
    padding: 6px 10px;
    font-size: 12px;
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
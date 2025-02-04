import styled from 'styled-components';

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
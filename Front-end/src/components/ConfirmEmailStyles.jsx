import styled from 'styled-components';

export const Container = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-color: #f9f9f9;
`;

export const Heading = styled.h2`
  font-size: 24px;
  font-weight: bold;
  color: #333; 
`;

export const StatusMessage = styled.p`
  font-size: 16px;
  color: ${(props) => (props.success ? 'green' : 'red')};
`;
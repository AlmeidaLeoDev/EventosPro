import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';

export const FormContainer = styled.form`
  max-width: 600px;
  margin: 2rem auto;
  padding: 2rem;
  background: #ffffff;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
`;

export const FormGroup = styled.div`
  margin-bottom: 1.5rem;
`;

export const Label = styled.label`
  display: block;
  margin-bottom: 0.5rem;
  color: #2d3748;
  font-size: 0.9rem;
  font-weight: 500;
`;

export const Input = styled.input`
  width: 100%;
  padding: 0.8rem;
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

  &[type='datetime-local'] {
    &::-webkit-calendar-picker-indicator {
      filter: invert(0.5);
      cursor: pointer;
    }
  }
`;

export const SubmitButton = styled.button`
  width: 100%;
  padding: 0.8rem;
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

export const ErrorMessage = styled.span`
  color: #e53e3e;
  font-size: 0.875rem;
  margin-top: 0.25rem;
  display: block;
`;

function EventForm({ initialData = {}, onSubmit }) {
  const [description, setDescription] = useState('');
  const [startTime, setStartTime] = useState('');
  const [endTime, setEndTime] = useState('');

  useEffect(() => {
    if (initialData) {
      setDescription(initialData.description || '');
      setStartTime(initialData.startTime || '');
      setEndTime(initialData.endTime || '');
    }
  }, [initialData]);

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({ description, startTime, endTime });
  };

  return (
    <FormContainer onSubmit={handleSubmit}>
      <FormGroup>
        <Label>Descrição do Evento</Label>
        <Input
          type="text"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
          placeholder="Ex: Reunião de equipe"
        />
      </FormGroup>

      <FormGroup>
        <Label>Início</Label>
        <Input
          type="datetime-local"
          value={startTime}
          onChange={(e) => setStartTime(e.target.value)}
          required
        />
      </FormGroup>

      <FormGroup>
        <Label>Término</Label>
        <Input
          type="datetime-local"
          value={endTime}
          onChange={(e) => setEndTime(e.target.value)}
          required
        />
      </FormGroup>

      <SubmitButton type="submit">Salvar Evento</SubmitButton>
    </FormContainer>
  );
}

EventForm.propTypes = {
  initialData: PropTypes.shape({
    description: PropTypes.string,
    startTime: PropTypes.string,
    endTime: PropTypes.string,
  }),
  onSubmit: PropTypes.func.isRequired,
};

export default EventForm;

import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { useLocation } from 'react-router-dom';
import api from '../services/api'; 
import { Container, FormCard, Title, FormGroup, Label, StyledSelect, ErrorMessage, SubmitButton, StatusMessage } from '../components/InviteRespondStyles';

const InviteRespondPage = () => {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const inviteId = queryParams.get('inviteId');

  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm();
  const [status, setStatus] = useState(null); // Estado para mensagens

  const onSubmit = async (data) => {
    try {
      const payload = { InviteId: inviteId, Response: data.response };
      await api.post('/api/events/invite-respond', payload);
      setStatus({ type: 'success', message: 'Resposta registrada com sucesso!' });
    } catch (error) {
      console.error('Error responding to invitation:', error);
      setStatus({ type: 'error', message: error.response?.data || 'Falha ao responder o convite.' });
    }
  };

  return (
    <Container>
      <FormCard>
        <Title>Responder ao Convite</Title>
        
        <form onSubmit={handleSubmit(onSubmit)}>
          <FormGroup>
            <Label htmlFor="response">Selecione sua resposta</Label>
            <StyledSelect id="response" {...register('response', { required: 'Selecione uma opção' })}>
              <option value="">Escolha uma opção</option>
              <option value="accept">✅ Aceitar Convite</option>
              <option value="decline">❌ Recusar Convite</option>
            </StyledSelect>
            {errors.response && <ErrorMessage>{errors.response.message}</ErrorMessage>}
          </FormGroup>

          {status && <StatusMessage type={status.type}>{status.message}</StatusMessage>}

          <SubmitButton type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Enviando...' : 'Confirmar Resposta'}
          </SubmitButton>
        </form>
      </FormCard>
    </Container>
  );
};

export default InviteRespondPage;

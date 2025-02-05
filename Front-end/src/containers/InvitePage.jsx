import api from '../services/api';
import { useForm } from 'react-hook-form';

import {FormContainer, StyledForm, FormGroup, Label, Input, ErrorMessage, SubmitButton} from '../components/InviteStyles'

const InviteForm = () => {
    const {
      register,
      handleSubmit,
      formState: { errors, isSubmitting },
      reset,
    } = useForm();
  
    const onSubmit = async (data) => {
      try {
        await api.post('/api/events/invite', data);
        alert('Convite enviado com sucesso!');
        reset();
      } catch (error) {
        console.error('Erro ao enviar convite:', error);
        alert(error.response?.data || 'Falha ao enviar convite.');
      }
    };
  
    return (
      <FormContainer>
        <h2>Enviar Convite</h2>
        <StyledForm onSubmit={handleSubmit(onSubmit)}>
          <FormGroup>
            <Label htmlFor="InvitedUserEmail">E-mail do Usuário Convidado:</Label>
            <Input
              type="email"
              id="InvitedUserEmail"
              {...register('InvitedUserEmail', { required: 'O e-mail é obrigatório' })}
            />
            {errors.InvitedUserEmail && (
              <ErrorMessage>{errors.InvitedUserEmail.message}</ErrorMessage>
            )}
          </FormGroup>
  
          <FormGroup>
            <Label htmlFor="EventId">ID do Evento:</Label>
            <Input
              type="text"
              id="EventId"
              {...register('EventId', { required: 'O ID do evento é obrigatório' })}
            />
            {errors.EventId && (
              <ErrorMessage>{errors.EventId.message}</ErrorMessage>
            )}
          </FormGroup>
  
          <SubmitButton type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Enviando...' : 'Enviar Convite'}
          </SubmitButton>
        </StyledForm>
      </FormContainer>
    );
  };
  
export default InviteForm;
import { useForm } from 'react-hook-form';
import api from '../services/api'; // Ajuste o caminho conforme sua estrutura de pastas

const InviteForm = () => {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm();

  const onSubmit = async (data) => {
    try {
      // data deverá conter: { InvitedUserEmail, EventId }
      await api.post('/api/events/invite', data);
      alert('Convite enviado com sucesso!');
      reset();
    } catch (error) {
      console.error('Erro ao enviar convite:', error);
      // Exibe a mensagem de erro vinda da API, se houver.
      alert(error.response?.data || 'Falha ao enviar convite.');
    }
  };

  return (
    <div>
      <h2>Enviar Convite</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div>
          <label htmlFor="InvitedUserEmail">E-mail do Usuário Convidado:</label>
          <input
            type="email"
            id="InvitedUserEmail"
            {...register('InvitedUserEmail', { required: 'O e-mail é obrigatório' })}
          />
          {errors.InvitedUserEmail && (
            <p style={{ color: 'red' }}>{errors.InvitedUserEmail.message}</p>
          )}
        </div>

        <div>
          <label htmlFor="EventId">ID do Evento:</label>
          <input
            type="text"
            id="EventId"
            {...register('EventId', { required: 'O ID do evento é obrigatório' })}
          />
          {errors.EventId && (
            <p style={{ color: 'red' }}>{errors.EventId.message}</p>
          )}
        </div>

        <button type="submit" disabled={isSubmitting}>
          Enviar Convite
        </button>
      </form>
    </div>
  );
};

export default InviteForm;

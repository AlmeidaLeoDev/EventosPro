import { useForm } from 'react-hook-form';
import { useLocation } from 'react-router-dom';
import api from './api'; 

const InviteResponsePage = () => {
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const inviteId = queryParams.get('inviteId');

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm();

  const onSubmit = async (data) => {
    try {
      // Monta o payload de resposta ao convite.
      // Espera-se um modelo com as propriedades: InviteId e Response.
      const payload = { InviteId: inviteId, Response: data.response };
      await api.post('/api/events/invite-respond', payload);
      alert('Resposta registrada com sucesso!');
    } catch (error) {
      console.error('Error responding to invitation:', error);
      alert(error.response?.data || 'Falha ao responder o convite.');
    }
  };

  return (
    <div>
      <h2>Responder ao Convite</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div>
          <label htmlFor="response">Aceitar ou Recusar?</label>
          <select
            id="response"
            {...register('response', { required: 'Selecione uma opção' })}
          >
            <option value="">Selecione uma opção</option>
            <option value="accept">Aceitar</option>
            <option value="decline">Recusar</option>
          </select>
          {errors.response && (
            <p style={{ color: 'red' }}>{errors.response.message}</p>
          )}
        </div>
        <button type="submit" disabled={isSubmitting}>
          Enviar Resposta
        </button>
      </form>
    </div>
  );
};

export default InviteResponsePage;

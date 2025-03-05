import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../services/api';
import { Container, StyledHeading, FormContainer, FormActions } from '../components/EditEventStyles';

function EditEventPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    description: '',
    startTime: '',
    endTime: ''
  });

  useEffect(() => {
    const fetchEvent = async () => {
      try {
        const response = await api.get(`/api/events/${id}`);
        const event = response.data;
        
        // Corrigir a formatação das datas para datetime-local
        const adjustDateForInput = (dateString) => {
          const date = new Date(dateString);
          return new Date(date.getTime() - date.getTimezoneOffset() * 60000)
            .toISOString()
            .slice(0, 16);
        };
  
        setFormData({
          description: event.description,
          startTime: adjustDateForInput(event.startTime),
          endTime: adjustDateForInput(event.endTime)
        });
      } catch (error) {
        console.error('Erro ao carregar:', error);
      }
    };
    fetchEvent();
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const payload = {
        Id: parseInt(id),
        Description: formData.description,
        StartTime: new Date(formData.startTime).toISOString(),
        EndTime: new Date(formData.endTime).toISOString()
      };
  
      console.log('Payload enviado:', payload); // Adicione este log
  
      const response = await api.put(`/api/events/update-event/${id}`, payload);
      console.log('Resposta da API:', response.data); // Log da resposta
      
      navigate('/home', { state: { refresh: true } });
    } catch (error) {
      console.error('Erro completo:', error); // Log completo do erro
      console.log('Resposta do servidor:', error.response); // Detalhes da resposta
      alert(error.response?.data?.message || 'Erro ao atualizar evento');
    }
  };

  const handleDelete = async () => {
    const confirmDelete = window.confirm('Excluir este evento permanentemente?');
    if (confirmDelete) {
      try {
        await api.delete(`/api/events/delete-event?id=${id}`);
        navigate('/home');
      } catch (error) {
        console.error('Erro ao excluir:', error);
      }
    }
  };

  return (
    <Container>
      <StyledHeading>Editar Evento</StyledHeading>
      
      <FormContainer onSubmit={handleSubmit}>
        <label>
          Descrição:
          <input
            type="text"
            value={formData.description}
            onChange={(e) => setFormData({ ...formData, description: e.target.value })}
            required
          />
        </label>
        
        <label>
          Data/Hora Início:
          <input
            type="datetime-local"
            value={formData.startTime}
            onChange={(e) => setFormData({ ...formData, startTime: e.target.value })}
            required
          />
        </label>
        
        <label>
          Data/Hora Fim:
          <input
            type="datetime-local"
            value={formData.endTime}
            onChange={(e) => setFormData({ ...formData, endTime: e.target.value })}
            required
          />
        </label>
        
        <FormActions>
          <button type="submit">Salvar Alterações</button>
          <button type="button" onClick={handleDelete} className="danger">
            Excluir Evento
          </button>
          <button type="button" onClick={() => navigate('/home')}>
            Cancelar
          </button>
        </FormActions>
      </FormContainer>
    </Container>
  );
}

export default EditEventPage;
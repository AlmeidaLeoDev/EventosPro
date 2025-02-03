import { useState, useEffect, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import EventForm from '../components/EventForm';
import api from '../services/api';

function EditEventPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [eventData, setEventData] = useState(null);

  const fetchEvent = useCallback(async () => {
    try {
      const response = await api.get(`/events/${id}`);
      setEventData(response.data);
    } catch (error) {
      console.error('Error when searching for event:', error);
      alert('Evento não encontrado');
      navigate('/');
    }
  }, [id, navigate]);

  useEffect(() => {
    fetchEvent();
  }, [fetchEvent]);

  const handleUpdateEvent = async (formData) => {
    try {
      await api.put(`/events/${id}`, { id, ...formData });
      navigate('/');
    } catch (error) {
      console.error('Error updating event:', error);
      alert('Erro ao atualizar evento');
    }
  };

  const handleDeleteEvent = async () => {
    if (window.confirm('Tem certeza que deseja excluir este evento?')) {
      try {
        await api.delete(`/events/${id}`);
        navigate('/');
      } catch (error) {
        console.error('Error deleting event:', error);
        alert('Erro ao excluir evento');
      }
    }
  };

  return (
    <div className="container">
      <h2>Detalhes do Evento</h2>
      {eventData ? (
        <>
          <EventForm initialData={eventData} onSubmit={handleUpdateEvent} />
          <button onClick={handleDeleteEvent} className="delete-btn">
            Excluir Evento
          </button>
        </>
      ) : (
        <p>Carregando...</p>
      )}
    </div>
  );
}

export default EditEventPage;

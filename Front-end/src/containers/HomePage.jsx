import { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import api from '../services/api';
import {
  Container,
  Header,
  Title,
  LogoutButton,
  CalendarWrapper,
  CreateEventButton,
  ModalOverlay,
  ModalContent,
  ModalActions
} from '../components/HomeStyles';

function HomePage() {
  const [events, setEvents] = useState([]);
  const [selectedEvent, setSelectedEvent] = useState(null);
  const [showActionModal, setShowActionModal] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();

  const fetchEvents = async () => {
    try {
      const response = await api.get('/api/events/range', {
        params: {
          start: new Date().toISOString(),
          end: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString()
        }
      });

      const mappedEvents = response.data.map(event => ({
        id: event.id,
        title: event.description,
        start: event.startTime,
        end: event.endTime,
        color: event.status === 'Cancelled' ? '#ff0000' : '#007bff'
      }));

      setEvents(mappedEvents);
    } catch (error) {
      console.error('Error:', error.response?.data);
    }
  };

  useEffect(() => {
    fetchEvents();
    
    // Resetar estado de navegação
    if (location.state?.refresh) {
      navigate(location.pathname, { replace: true, state: {} });
    }
  }, [location.state]); // Recarrega quando o estado muda

  const handleEventClick = (info) => {
    setSelectedEvent(info.event);
    setShowActionModal(true);
  };

  const handleDelete = async () => {
    try {
      await api.delete(`/api/events/delete-event?id=${selectedEvent.id}`);
      
      // Atualização imediata do estado local
      setEvents(prevEvents => prevEvents.filter(event => event.id !== selectedEvent.id));
      
      // Fechar modal e resetar seleção
      setShowActionModal(false);
      setSelectedEvent(null);
  
      // Forçar recarga adicional para garantir sincronização
      await fetchEvents(); 
  
    } catch (error) {
      console.error('Erro ao excluir:', error.response?.data);
      alert('Falha ao excluir evento');
    }
  };

  return (
    <Container>
      <Header>
        <Title>Meu Calendário de Eventos</Title>
        <LogoutButton onClick={() => {
          localStorage.removeItem("authToken");
          navigate("/login");
        }}>Logout</LogoutButton>
      </Header>

      <CreateEventButton onClick={() => navigate("/create-event")}>
        Criar Novo Evento
      </CreateEventButton>

      <CalendarWrapper>
        <FullCalendar
          plugins={[dayGridPlugin, interactionPlugin]}
          initialView="dayGridMonth"
          events={events}
          eventClick={handleEventClick}
          height="auto"
          headerToolbar={{
            start: 'title',
            center: '',
            end: 'today prev,next'
          }}
          timeZone="UTC"
          eventTimeFormat={{
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
          }}
        />
      </CalendarWrapper>

      {showActionModal && (
        <ModalOverlay>
          <ModalContent>
            <h3>{selectedEvent.title}</h3>
            <p>O que deseja fazer com este evento?</p>
            <ModalActions>
              <button onClick={() => navigate(`/edit-event/${selectedEvent.id}`)}>
                Editar
              </button>
              <button onClick={handleDelete} className="danger">
                Excluir
              </button>
              <button onClick={() => setShowActionModal(false)}>
                Cancelar
              </button>
            </ModalActions>
          </ModalContent>
        </ModalOverlay>
      )}
    </Container>
  );
}

export default HomePage;
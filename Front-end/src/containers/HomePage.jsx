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
  CreateEventButton
} from '../components/HomeStyles';

function HomePage() {
  const [events, setEvents] = useState([]);
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
          eventClick={(info) => navigate(`/edit-event/${info.event.id}`)}
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
    </Container>
  );
}

export default HomePage;
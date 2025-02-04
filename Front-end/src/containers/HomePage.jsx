import { useEffect, useState } from 'react';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import { useNavigate } from 'react-router-dom';
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

  const fetchEvents = async () => {
    try {
      const response = await api.get('/events');
      const mappedEvents = response.data.events.map((event) => ({
        id: event.id,
        title: event.description,
        start: event.startTime,
        end: event.endTime,
      }));
      setEvents(mappedEvents);
    } catch (error) {
      console.error('Error when fetching events:', error);
    }
  };

  useEffect(() => {
    fetchEvents();
  }, []);

  const handleEventClick = (clickInfo) => {
    navigate(`/edit-event/${clickInfo.event.id}`);
  };

  const handleLogout = () => {
    localStorage.removeItem("authToken");
    sessionStorage.removeItem("authToken");
    navigate("/login");
  };

  return (
    <Container>
      <Header>
        <Title>Meu Calendário de Eventos</Title>
        <LogoutButton onClick={handleLogout}>Logout</LogoutButton>
      </Header>

      <CreateEventButton onClick={() => navigate("/create-event")}>
        Criar Novo Evento
      </CreateEventButton>

      <CalendarWrapper>
        <FullCalendar
          plugins={[dayGridPlugin]}
          initialView="dayGridMonth"
          events={events}
          eventClick={handleEventClick}
          height="auto"
          headerToolbar={{
            start: 'title',
            center: '',
            end: 'today prev,next'
          }}
          eventColor="#007bff"
          eventTextColor="white"
        />
      </CalendarWrapper>
    </Container>
  );
}

export default HomePage;
import React, { useEffect, useState } from 'react';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';

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

  return (
    <div className="container">
      <h1>Meu Calendário de Eventos</h1>
      <button onClick={() => navigate('/create-event')}>Criar Novo Evento</button>
      <FullCalendar
        plugins={[dayGridPlugin, timeGridPlugin]}
        initialView="dayGridMonth"
        events={events}
        eventClick={handleEventClick}
        height="auto"
      />
    </div>
  );
}

export default HomePage;

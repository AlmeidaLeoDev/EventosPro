import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import api from '../services/api';
import EventForm from '../components/EventFormStyles';
import { Container, StyledHeading, CalendarWrapper } from '../components/CreateEventStyles';

function CreateEventPage() {
  const navigate = useNavigate();
  
  const [startTime, setStartTime] = useState('');
  const [endTime, setEndTime] = useState('');
  
  const handleCreateEvent = async (formData) => {
    try {
      await api.post('/events', formData);
      navigate('/');
    } catch (error) {
      console.error('Error creating event:', error);
      alert('Erro ao criar evento');
    }
  };

  const handleDateSelect = (info) => {
    const start = info.startStr; 
    const end = info.endStr;  
    setStartTime(start);  
    setEndTime(end); 
  };

  return (
    <Container>
      <StyledHeading>Criar Evento</StyledHeading>

      <CalendarWrapper>
        <FullCalendar
          plugins={[dayGridPlugin, timeGridPlugin]}
          initialView="dayGridMonth"
          select={handleDateSelect}
          selectable={true}
        />
      </CalendarWrapper>

      <EventForm
        onSubmit={handleCreateEvent}
        initialData={{ startTime, endTime }}
      />
    </Container>
  );
}

export default CreateEventPage;

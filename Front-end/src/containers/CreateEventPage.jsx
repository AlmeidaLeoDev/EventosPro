import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import api from '../services/api';
import EventForm from '../components/EventFormStyles';
import { Container, StyledHeading, CalendarWrapper } from '../components/CreateEventStyles';

function CreateEventPage() {
  const navigate = useNavigate();
  const [selectedDates, setSelectedDates] = useState({
    start: '',
    end: ''
  });

  const handleCreateEvent = async (formData) => {
    try {
      // Converter para objetos Date
      const startDate = new Date(formData.startTime);
      const endDate = new Date(formData.endTime);
      
      // Validar datas
      if (startDate >= endDate) {
        alert('A data final deve ser posterior à inicial');
        return;
      }

      const payload = {
        description: formData.description,
        startTime: startDate.toISOString(),
        endTime: endDate.toISOString()
      };

      await api.post('/api/events/create-event', payload);
      navigate('/home'); // Redirecionar para a home
    } catch (error) {
      console.error('Erro detalhado:', error.response?.data);
      alert(error.response?.data?.message || 'Erro ao criar evento');
    }
  };

  const handleDateSelect = (info) => {
    // Usar datas diretamente do FullCalendar
    setSelectedDates({
      start: info.startStr,
      end: info.endStr
    });
  };

  return (
    <Container>
      <StyledHeading>Criar Evento</StyledHeading>

      <CalendarWrapper>
        <FullCalendar
          plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
          initialView="dayGridMonth"
          selectable={true}
          select={handleDateSelect}
          headerToolbar={{
            start: 'prev,next today',
            center: 'title',
            end: 'dayGridMonth,timeGridWeek'
          }}
          timeZone="UTC"
        />
      </CalendarWrapper>

      <EventForm
        onSubmit={handleCreateEvent}
        initialData={{
          startTime: selectedDates.start,
          endTime: selectedDates.end
        }}
      />
    </Container>
  );
}

export default CreateEventPage;
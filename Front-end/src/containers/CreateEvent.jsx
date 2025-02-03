import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import EventForm from '../components/EventForm';
import api from '../api/api';

function CreateEventPage() {
  const navigate = useNavigate();
  
  const [startTime, setStartTime] = useState('');
  const [endTime, setEndTime] = useState('');
  
  const handleCreateEvent = async (formData) => {
    try {
      const response = await api.post('/events', formData);
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
    <div className="container">
      <h2>Criar Evento</h2>

      <FullCalendar
        plugins={[dayGridPlugin, timeGridPlugin]}
        initialView="dayGridMonth"
        select={handleDateSelect}  
        selectable={true} 
      />

      <EventForm
        onSubmit={handleCreateEvent}
        initialData={{ startTime, endTime }} 
      />
    </div>
  );
}

export default CreateEventPage;

import React, { useState, useEffect } from 'react';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';

function EventForm({ initialData = {}, onSubmit }) {
  const [description, setDescription] = useState('');
  const [startTime, setStartTime] = useState('');
  const [endTime, setEndTime] = useState('');

  useEffect(() => {
    if (initialData) {
      setDescription(initialData.description || '');
      setStartTime(initialData.startTime ? initialData.startTime.slice(0, 16) : '');
      setEndTime(initialData.endTime ? initialData.endTime.slice(0, 16) : '');
    }
  }, [initialData]);

  const handleDateClick = (info) => {
    setStartTime(info.dateStr);
    setEndTime(info.dateStr);
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit({ description, startTime, endTime });
  };

  return (
    <form onSubmit={handleSubmit} className="event-form">
      <div>
        <label>Descrição:</label>
        <input
          type="text"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
        />
      </div>
      <div>
        <label>Data/Hora de Início:</label>
        <input
          type="datetime-local"
          value={startTime}
          onChange={(e) => setStartTime(e.target.value)}
          required
        />
      </div>
      <div>
        <label>Data/Hora de Término:</label>
        <input
          type="datetime-local"
          value={endTime}
          onChange={(e) => setEndTime(e.target.value)}
          required
        />
      </div>
      <div>
        <label>Selecione Data no Calendário:</label>
        <FullCalendar
          plugins={[dayGridPlugin, timeGridPlugin]}
          initialView="dayGridMonth"
          dateClick={handleDateClick}
        />
      </div>
      <button type="submit">Salvar</button>
    </form>
  );
}

export default EventForm;

import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

function EventForm({ initialData = {}, onSubmit }) {
  const [description, setDescription] = useState('');
  const [startTime, setStartTime] = useState('');
  const [endTime, setEndTime] = useState('');

  useEffect(() => {
    if (initialData) {
      setDescription(initialData.description || '');
      setStartTime(initialData.startTime || '');
      setEndTime(initialData.endTime || '');
    }
  }, [initialData]);

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
      <button type="submit">Salvar</button>
    </form>
  );
}

EventForm.propTypes = {
  initialData: PropTypes.shape({
    description: PropTypes.string,
    startTime: PropTypes.string,
    endTime: PropTypes.string,
  }),
  onSubmit: PropTypes.func.isRequired,
};

export default EventForm;

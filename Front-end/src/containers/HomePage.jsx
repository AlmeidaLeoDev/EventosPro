import { useEffect, useState } from 'react';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
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

  
  // 🔹 Função de logout: Remove o token de autenticação e redireciona para a página de login
  const handleLogout = () => {
    localStorage.removeItem("authToken"); // Remove o token do localStorage
    sessionStorage.removeItem("authToken"); // Remove o token do sessionStorage (caso esteja armazenado temporariamente)
    navigate("/login"); // Redireciona para a tela de login
  };

  return (
    <div className="container">
      {/* 🔹 Cabeçalho com título e botão de logout alinhados */}
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <h1>Meu Calendário de Eventos</h1>
        {/* 🔹 Botão de Logout para sair da conta */}
        <button onClick={handleLogout} style={{ backgroundColor: "red", color: "white" }}>
          Logout
        </button>
      </div>

      {/* Botão para criar um novo evento */}
      <button onClick={() => navigate("/create-event")}>Criar Novo Evento</button>

      {/* Componente do calendário */}
      <FullCalendar
        plugins={[dayGridPlugin]}
        initialView="dayGridMonth"
        events={events}
        eventClick={handleEventClick}
        height="auto"
      />
    </div>
  );
}

export default HomePage;

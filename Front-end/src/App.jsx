import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Navigate } from 'react-router-dom';
import ProtectedRoute from './components/ProtectedRoute'


// Authentication containers
import LoginPage from './containers/LoginPage';
import RegisterPage from './containers/RegisterPage';
import ConfirmEmailPage from './containers/ConfirmEmailPage';
import ForgotPasswordPage from './containers/ForgotPasswordPage';
import ResetPasswordPage from './containers/ResetPasswordPage';
import ChangePasswordPage from './containers/ChangePasswordPage'; 

// Event containers
import HomePage from './containers/HomePage';
import CreateEventPage from './containers/CreateEventPage';
import EditEventPage from './containers/EditEventPage';

// Invite pages
import InvitePage from './containers/InvitePage';
import InviteRespondPage from './containers/InviteRespondPage';

// Global styles
import './styles/styles';


function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Navigate to="/login" />} />

        {/* Authentication routes */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/confirm-email" element={<ConfirmEmailPage />} />
        <Route path="/forgot-password" element={<ForgotPasswordPage/>} />
        <Route path="/reset-password" element={<ResetPasswordPage />} />
        <Route path="/change-password" element={<ProtectedRoute><ChangePasswordPage /></ProtectedRoute>} />

        {/* Events routes */}
        <Route path="/home" element={<ProtectedRoute><HomePage /></ProtectedRoute>} />
        <Route path="/create-event" element={<ProtectedRoute><CreateEventPage /></ProtectedRoute>} />
        <Route path="/update-event/:id" element={<ProtectedRoute><EditEventPage /></ProtectedRoute>} />
        <Route path="/edit-event/:id" element={ // Corrigido o nome do parâmetro
          <ProtectedRoute>
            <EditEventPage />
          </ProtectedRoute>
        } />

        {/* Invites routes */}
        <Route path="/invite" element={<ProtectedRoute><InvitePage /></ProtectedRoute>} />
        <Route path="/invite-respond" element={<ProtectedRoute><InviteRespondPage /></ProtectedRoute>} />
      </Routes>
    </Router>
  );
}



export default App;

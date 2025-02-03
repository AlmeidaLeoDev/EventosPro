import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

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
import InviteForm from './components/InviteForm';
import InviteResponsePage from './containers/InviteResponsePage';

// Global styles
import '';

function App() {
  return (
    <Router>
      <Routes>
        {/* Authentication routes */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/confirm-email" element={<ConfirmEmailPage />} />
        <Route path="/forgot-password" element={<ForgotPasswordPage />} />
        <Route path="/reset-password" element={<ResetPasswordPage />} />
        <Route path="/change-password" element={<ChangePasswordPage />} />

        {/* Events routes */}
        <Route path="/" element={<HomePage />} />
        <Route path="/create-event" element={<CreateEventPage />} />
        <Route path="/edit-event/:id" element={<EditEventPage />} />

        {/* Invites routes */}
        <Route path="/send-invite" element={<InviteForm />} />
        <Route path="/respond-invite" element={<InviteResponsePage />} />
      </Routes>
    </Router>
  );
}

export default App;

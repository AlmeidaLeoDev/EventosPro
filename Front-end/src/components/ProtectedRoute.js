import PropTypes from 'prop-types'; 
import { Navigate } from 'react-router-dom';

const ProtectedRoute = ({ children }) => {
  const isAuthenticated = () => {
    const token = localStorage.getItem('token');
    return !!token;
  };

  if (!isAuthenticated()) {
    return <Navigate to="" replace />;
  }

  return children;
};

ProtectedRoute.propTypes = {
  children: PropTypes.node.isRequired
};

export default ProtectedRoute;
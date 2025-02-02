// src/api/api.js
import axios from 'axios';

const api = axios.create({
  baseURL: 'https://35c4-2804-56c-a50e-f700-60be-aeae-8795-15c3.ngrok-free.app',
});

api.interceptors.request.use(
    (config) => {
      const token = localStorage.getItem('authToken');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error) => Promise.reject(error)
  );

export default api; 



import axios from 'axios';

const api = axios.create({
  baseURL: 'https://927b-2804-56c-a41a-f300-7cfd-6855-46a5-1022.ngrok-free.app',
  headers: {
    'Content-Type': 'application/json',
    'ngrok-skip-browser-warning': 'true',
    'Accept': 'application/json',
    'Access-Control-Allow-Origin': '*'
  },
  withCredentials: true,
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

  api.interceptors.response.use(
    (response) => response,
    async (error) => {
      if (error.response?.status === 401) {
        localStorage.removeItem('authToken');
      }
      return Promise.reject(error);
    }
  );

export default api; 



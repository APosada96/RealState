import axios from 'axios';

const api = axios.create({
    baseURL: 'https://localhost:7207/api', // Cambia según tu backend
  });
  
  export default api;
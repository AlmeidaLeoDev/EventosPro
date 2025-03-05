import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    host: "0.0.0.0",
    port: 5173,
    strictPort: true,
    cors: true,
    allowedHosts: ["b8c7-2804-56c-a41a-f300-7cfd-6855-46a5-1022.ngrok-free.app", 
    ],
    proxy: {
      // Todas as chamadas para /api serão redirecionadas para o back-end
      "/api": {
        target: "https://localhost:7247",
        changeOrigin: true,
        secure: false
      }
    }
  }
})

import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    host: "0.0.0.0",
    port: 5173,
    strictPort: true,
    cors: true,
    allowedHosts: ["8a97-2804-56c-a56d-8600-5ce4-b558-312d-70e6.ngrok-free.app", 
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

import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    host: "0.0.0.0",
    port: 5173,
    strictPort: true,
    cors: true,
    allowedHosts: ["b3ba-2804-56c-a41a-f300-5429-a608-5b71-574e.ngrok-free.app", 
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

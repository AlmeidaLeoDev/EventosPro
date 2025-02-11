import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    host: "0.0.0.0",
    port: 5173,
    strictPort: true,
    cors: true,
    allowedHosts: ["49dd-2804-56c-a472-2c00-59bd-862e-bdcc-6e92.ngrok-free.app", 
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

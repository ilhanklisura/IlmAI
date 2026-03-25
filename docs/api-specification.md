# IlmAI — API Specification

## Backend API (port 5030)

| Method  | Endpoint             | Description                  |
| ------- | -------------------- | ---------------------------- |
| POST    | /api/auth/login      | User login                   |
| POST    | /api/auth/register   | User registration            |
| POST    | /api/auth/refresh    | Token refresh                |
| GET     | /api/chat/sessions   | List chat sessions           |
| POST    | /api/chat/send       | Send message (proxies to AI) |
| GET     | /api/quran/search    | Search Qur'an verses         |
| GET     | /api/hadith/search   | Search Hadith                |
| GET     | /api/tafsir/{ayahId} | Get tafsir for ayah          |
| GET     | /api/daily           | Get daily ayah/hadith        |
| POST    | /api/search          | Semantic search              |
| GET/PUT | /api/settings        | User settings                |

## AI Server (port 5031)

| Method | Endpoint                  | Description     |
| ------ | ------------------------- | --------------- |
| POST   | /api/rag                  | RAG Q&A         |
| POST   | /api/rag/global           | Global RAG Q&A  |
| POST   | /api/search               | Semantic search |
| POST   | /api/documents/upload     | Upload document |
| POST   | /api/documents/{id}/index | Index document  |
| GET    | /api/health               | Health check    |

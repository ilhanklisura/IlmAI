# IlmAI — Architecture

## High-Level Flow

```
User → Frontend (Vue 3) → Backend API (.NET 8) → AI Server (.NET 8) → PostgreSQL + pgvector → OpenAI API
```

## Components

### Frontend (`frontend/ilmai-web/`)
- Vue 3 + TypeScript + Vuetify 3
- Pinia state management
- vue-i18n for multilingual support
- Handles UI, routing, and API calls

### Backend API (`backend/ilmai-api/`)
- ASP.NET Core 8
- JWT authentication
- PostgreSQL via EF Core
- Proxies AI requests to AI Server
- Manages users, sessions, settings

### AI Server (`ai-server/ilmai-ai/`)
- ASP.NET Core 8
- OpenAI integration (GPT-4o-mini + text-embedding-3-small)
- RAG pipeline: embed → retrieve → prompt → generate
- pgvector for semantic search
- Microsoft Semantic Kernel

### Database (PostgreSQL + pgvector)
- Application data: users, roles, chat history, settings
- Knowledge base: Qur'an, Hadith, Tafsir chunks with embeddings

# IlmAI — AI-Powered Islamic Knowledge Assistant for Balkans

> A domain-specific AI assistant focused on Islamic knowledge for users in Bosnia & Herzegovina and the wider Balkan region.

## Overview

IlmAI is a full-stack AI application that allows users to ask questions about Islam in natural language and receive **source-grounded answers** based on:

- Qur'an verses
- Hadith collections
- Tafsir explanations
- Curated Islamic scholarly texts

The system supports **Bosnian** and **English** languages.

## Architecture

```
User → Frontend (Vue 3) → Backend API (.NET 8) → AI Server (.NET 8) → PostgreSQL + pgvector → OpenAI API
```

| Component   | Technology                       | Port |
| ----------- | -------------------------------- | ---- |
| Frontend    | Vue 3 + Vuetify 3 + TypeScript   | 5173 |
| Backend API | ASP.NET Core 8                   | 5030 |
| AI Server   | ASP.NET Core 8 + Semantic Kernel | 5031 |
| Database    | PostgreSQL + pgvector            | 5432 |

## Quick Start

### Prerequisites

- .NET 8 SDK
- Node.js 20+
- PostgreSQL 16+ with pgvector extension
- OpenAI API Key

### Development

```bash
# Frontend
cd frontend/ilmai-web
npm install
npm run dev

# Backend API
cd backend/ilmai-api
dotnet run

# AI Server
cd ai-server/ilmai-ai
dotnet run
```

### Docker

```bash
docker-compose up -d
```

## Project Structure

```
ilmai/
├── frontend/ilmai-web/    # Vue 3 + Vuetify frontend
├── backend/ilmai-api/     # ASP.NET Core main API
├── ai-server/ilmai-ai/    # AI/RAG server
├── database/              # Migrations, datasets, seeds
├── scripts/               # Data preparation utilities
├── docker/                # Dockerfiles + nginx
├── docs/                  # Documentation
└── .github/               # CI/CD
```

## License

This project is developed as an undergraduate Software Engineering thesis.

# IlmAI — Deployment Guide

## Docker Deployment

```bash
# Build and start all services
docker-compose up -d --build

# Check status
docker-compose ps

# View logs
docker-compose logs -f
```

## VPS Production Layout

```
/opt/ilmai/
├── docker-compose.yml
├── .env
├── frontend/
├── backend/
├── ai-server/
└── postgres/
```

## Environment Variables

| Variable               | Description         |
| ---------------------- | ------------------- |
| POSTGRES_PASSWORD      | PostgreSQL password |
| OPENAI_API_KEY         | OpenAI API key      |
| ASPNETCORE_ENVIRONMENT | .NET environment    |

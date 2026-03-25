# IlmAI — Database Schema

## Application Data

- **users** — user accounts
- **roles** — user roles
- **user_roles** — many-to-many user-role mapping
- **user_settings** — user preferences (language, theme)
- **chat_sessions** — conversation sessions
- **chat_messages** — individual messages in sessions
- **favorite_items** — bookmarked content
- **search_history** — past searches

## Knowledge Base (AI Server)

- **documents** — source documents metadata
- **document_chunks** — text chunks with embedding vectors
- **quran_chunks** — Qur'an verse chunks
- **hadith_chunks** — Hadith text chunks
- **tafsir_chunks** — Tafsir explanation chunks

All chunk tables include a `vector(1536)` column for embeddings with HNSW index.

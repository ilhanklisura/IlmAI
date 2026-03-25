# IlmAI — RAG Pipeline

## Pipeline Steps

1. **Receive** user question
2. **Detect** language (Bosnian/English)
3. **Generate** question embedding via OpenAI text-embedding-3-small
4. **Search** vector similarity over Qur'an, Hadith, Tafsir chunks (pgvector cosine)
5. **Retrieve** top-K relevant chunks
6. **Construct** grounded prompt with context and sources
7. **Send** prompt to GPT-4o-mini
8. **Generate** grounded answer
9. **Return** answer + source citations

## Scoring

- **Strong match**: score ≥ 0.80
- **Weak match**: 0.70 ≤ score < 0.80
- **No match**: score < 0.70

## Prompt Template

The system prompt instructs the LLM to answer ONLY from provided context and cite sources.

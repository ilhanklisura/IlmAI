# IlmAI — Data Preparation Scripts

## Prerequisites

```bash
pip install psycopg2-binary pandas openai tqdm
```

## Scripts

| Script                    | Purpose                                |
| ------------------------- | -------------------------------------- |
| `import_quran.py`         | Import Qur'an CSV data into PostgreSQL |
| `import_hadith.py`        | Import Hadith CSV data                 |
| `import_tafsir.py`        | Import Tafsir texts                    |
| `normalize_text.py`       | Clean and normalize text data          |
| `chunk_documents.py`      | Split texts into chunks for RAG        |
| `generate_embeddings.py`  | Generate OpenAI embeddings for chunks  |
| `rebuild_vector_index.py` | Rebuild pgvector HNSW indexes          |

## Usage

```bash
# 1. Import raw data
python import_quran.py
python import_hadith.py

# 2. Normalize and chunk
python normalize_text.py
python chunk_documents.py

# 3. Generate embeddings
python generate_embeddings.py

# 4. Rebuild indexes (if needed)
python rebuild_vector_index.py
```

## Environment Variables

```
DATABASE_URL=postgresql://ilmai:password@localhost:5432/ilmai
OPENAI_API_KEY=sk-...
```

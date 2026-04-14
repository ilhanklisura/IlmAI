#!/bin/bash
set -e

echo "Waiting for PostgreSQL to be ready..."
until pg_isready -d "$DATABASE_URL"; do
  sleep 4
done
echo "PostgreSQL is ready."

# 1. Apply schema migrations if they haven't been applied by Postgres init
echo "Ensuring database schema is up to date..."
for f in /app/database/migrations/*.sql; do
    echo "Applying migration: $f"
    psql "$DATABASE_URL" -f "$f" || echo "Warning: Migration $f failed, continuing..."
done

# Check if quran_ayahs has data
# We check this after applying schema to avoid "relation does not exist" errors
ROW_COUNT=$(psql "$DATABASE_URL" -t -c "SELECT count(*) FROM quran_ayahs;")
ROW_COUNT=$(echo $ROW_COUNT | xargs)

if [ "$ROW_COUNT" -gt 0 ]; then
    echo "Database appears to be already populated ($ROW_COUNT ayahs found). Skipping ingestion to save resources/OpenAI costs."
else
    echo "Starting data ingestion pipeline..."

    # 1. Import Quran data
    echo "Importing Quran..."
    python3 /app/scripts/import_quran.py

    # 2. Chunk Documents
    echo "Chunking Quran..."
    python3 /app/scripts/chunk_documents.py

    # 3. Generate Embeddings
    echo "Generating embeddings..."
    python3 /app/scripts/generate_embeddings.py
    
    # 4. Seed other reference data (if applicable, e.g. tafsir, hadith etc)
    # The SQL files could be executed here if they just contain inserts.
    if [ -f "/app/database/seed/seed_quran.sql" ]; then
        echo "Running seed_quran.sql..."
        psql "$DATABASE_URL" -f /app/database/seed/seed_quran.sql
    fi

    echo "Data ingestion pipeline complete!"
fi

# Keep container alive if you want or just exit
echo "Data seeder finished successfully. Exiting."
exit 0

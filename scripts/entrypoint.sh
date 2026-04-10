#!/bin/bash
set -e

echo "Waiting for PostgreSQL to be ready..."
until pg_isready -d "$DATABASE_URL"; do
  sleep 4
done
echo "PostgreSQL is ready."

# Check if tables exist, or seed data. Postgres initdb handles the schema.
# Data seeder will insert if data is missing.

# Check if quran_ayahs has data
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

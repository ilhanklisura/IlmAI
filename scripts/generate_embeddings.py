"""IlmAI: Generate OpenAI embeddings for all chunks."""
import os
import psycopg2
from openai import OpenAI

DATABASE_URL = os.getenv("DATABASE_URL", "postgresql://ilmai:IlmAI_Dev_2026!@localhost:5432/ilmai")
OPENAI_API_KEY = os.getenv("OPENAI_API_KEY", "")
EMBEDDING_MODEL = "text-embedding-3-small"
BATCH_SIZE = 100


def get_embedding(client: OpenAI, text: str) -> list:
    """Generate embedding for a single text."""
    response = client.embeddings.create(input=text, model=EMBEDDING_MODEL)
    return response.data[0].embedding


def generate_embeddings_for_table(table: str, content_col: str = "content"):
    """Generate embeddings for all rows in a chunk table that don't have them yet."""
    client = OpenAI(api_key=OPENAI_API_KEY)
    conn = psycopg2.connect(DATABASE_URL)
    cur = conn.cursor()

    cur.execute(f"SELECT id, {content_col} FROM {table} WHERE embedding IS NULL")
    rows = cur.fetchall()
    total = len(rows)
    print(f"Generating embeddings for {total} rows in {table}...")

    for i, (chunk_id, content) in enumerate(rows):
        try:
            embedding = get_embedding(client, content)
            cur.execute(
                f"UPDATE {table} SET embedding = %s WHERE id = %s",
                (str(embedding), chunk_id),
            )
            if (i + 1) % BATCH_SIZE == 0:
                conn.commit()
                print(f"  Progress: {i + 1}/{total}")
        except Exception as e:
            print(f"  Error on {chunk_id}: {e}")

    conn.commit()
    cur.close()
    conn.close()
    print(f"Done: {table}")


if __name__ == "__main__":
    if not OPENAI_API_KEY:
        print("ERROR: Set OPENAI_API_KEY environment variable")
        exit(1)

    generate_embeddings_for_table("quran_chunks")
    generate_embeddings_for_table("hadith_chunks")
    generate_embeddings_for_table("tafsir_chunks")
    generate_embeddings_for_table("document_chunks")

"""IlmAI: Rebuild pgvector HNSW indexes."""
import os
import psycopg2

DATABASE_URL = os.getenv("DATABASE_URL", "postgresql://ilmai:IlmAI_Dev_2026!@localhost:5432/ilmai")

INDEXES = [
    ("idx_document_chunks_embedding", "document_chunks", "embedding"),
    ("idx_quran_chunks_embedding", "quran_chunks", "embedding"),
    ("idx_hadith_chunks_embedding", "hadith_chunks", "embedding"),
    ("idx_tafsir_chunks_embedding", "tafsir_chunks", "embedding"),
]


def rebuild_indexes():
    conn = psycopg2.connect(DATABASE_URL)
    cur = conn.cursor()

    for idx_name, table, column in INDEXES:
        print(f"Rebuilding {idx_name} on {table}.{column}...")
        cur.execute(f"DROP INDEX IF EXISTS {idx_name}")
        cur.execute(
            f"CREATE INDEX {idx_name} ON {table} USING hnsw ({column} vector_cosine_ops)"
        )
        print(f"  Done: {idx_name}")

    conn.commit()
    cur.close()
    conn.close()
    print("All indexes rebuilt.")


if __name__ == "__main__":
    rebuild_indexes()

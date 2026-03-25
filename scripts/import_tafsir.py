"""IlmAI: Import Tafsir texts into PostgreSQL."""
import os
import psycopg2

DATABASE_URL = os.getenv("DATABASE_URL", "postgresql://ilmai:IlmAI_Dev_2026!@localhost:5432/ilmai")
DATA_DIR = os.path.join(os.path.dirname(__file__), "..", "database", "datasets", "tafsir", "tafsir_source_files")


def import_tafsir():
    if not os.path.exists(DATA_DIR):
        print(f"Directory not found: {DATA_DIR}")
        return

    conn = psycopg2.connect(DATABASE_URL)
    cur = conn.cursor()

    for filename in os.listdir(DATA_DIR):
        if not filename.endswith(".txt"):
            continue

        filepath = os.path.join(DATA_DIR, filename)
        with open(filepath, "r", encoding="utf-8") as f:
            content = f.read()

        source_name = os.path.splitext(filename)[0]
        cur.execute(
            "INSERT INTO tafsir_sources (name, language) VALUES (%s, %s) ON CONFLICT (name) DO NOTHING RETURNING id",
            (source_name, "bs"),
        )
        row = cur.fetchone()
        if row:
            source_id = row[0]
            cur.execute(
                "INSERT INTO tafsir_entries (source_id, surah_number, ayah_start, ayah_end, content) VALUES (%s, 1, 1, 1, %s)",
                (source_id, content),
            )

    conn.commit()
    cur.close()
    conn.close()
    print("Tafsir import complete")


if __name__ == "__main__":
    import_tafsir()

"""IlmAI: Import Hadith data from CSV into PostgreSQL."""
import os
import csv
import psycopg2

DATABASE_URL = os.getenv("DATABASE_URL", "postgresql://ilmai:IlmAI_Dev_2026!@localhost:5432/ilmai")
DATA_DIR = os.path.join(os.path.dirname(__file__), "..", "database", "datasets", "hadith")

COLLECTIONS = {
    "bukhari.csv": "Sahih al-Bukhari",
    "muslim.csv": "Sahih Muslim",
    "riyad_us_salihin.csv": "Riyad us-Salihin",
}


def import_hadith(filename: str, collection_name: str):
    filepath = os.path.join(DATA_DIR, filename)
    if not os.path.exists(filepath):
        print(f"File not found: {filepath}")
        return

    conn = psycopg2.connect(DATABASE_URL)
    cur = conn.cursor()

    cur.execute("SELECT id FROM hadith_collections WHERE name = %s", (collection_name,))
    row = cur.fetchone()
    if not row:
        print(f"Collection not found: {collection_name}")
        return
    collection_id = row[0]

    with open(filepath, "r", encoding="utf-8") as f:
        reader = csv.DictReader(f)
        for entry in reader:
            cur.execute(
                """
                INSERT INTO hadiths (collection_id, hadith_number, chapter, text_bosnian, text_english, grade, narrator)
                VALUES (%s, %s, %s, %s, %s, %s, %s)
                """,
                (
                    collection_id,
                    int(entry.get("number", 0)),
                    entry.get("chapter", ""),
                    entry.get("text_bs", ""),
                    entry.get("text_en", ""),
                    entry.get("grade", ""),
                    entry.get("narrator", ""),
                ),
            )

    conn.commit()
    cur.close()
    conn.close()
    print(f"Imported {collection_name} from {filename}")


if __name__ == "__main__":
    for filename, name in COLLECTIONS.items():
        import_hadith(filename, name)

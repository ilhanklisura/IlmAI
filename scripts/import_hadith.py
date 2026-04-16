"""IlmAI: Import Hadith data from CSV into PostgreSQL."""
import os
import csv
import psycopg2
import re

DATABASE_URL = os.getenv("DATABASE_URL", "postgresql://ilmai:IlmAI_Dev_2026!@localhost:5432/ilmai")
DATA_DIR = os.path.join(os.path.dirname(__file__), "..", "database", "datasets", "hadiths")

def extract_hadith_number(ref):
    # e.g., 'Book 1, Hadith 1' -> 1
    match = re.search(r'Hadith (\d+)', str(ref))
    return int(match.group(1)) if match else 0

def import_hadith():
    filename = "Sahih%20al-Bukhari.csv"
    collection_name = "Sahih al-Bukhari"
    filepath = os.path.join(DATA_DIR, filename)
    
    if not os.path.exists(filepath):
        print(f"File not found: {filepath}")
        return

    conn = psycopg2.connect(DATABASE_URL)
    cur = conn.cursor()

    cur.execute("SELECT id FROM hadith_collections WHERE name = %s", (collection_name,))
    row = cur.fetchone()
    
    if not row:
        print(f"Collection not found: {collection_name}. Make sure migrations have run.")
        return
    collection_id = row[0]

    with open(filepath, "r", encoding="utf-8") as f:
        reader = csv.DictReader(f)
        for entry in reader:
            number = extract_hadith_number(entry.get("In-book reference", ""))
            
            cur.execute(
                """
                INSERT INTO hadiths (collection_id, hadith_number, chapter, text_arabic, text_english, grade, narrator)
                VALUES (%s, %s, %s, %s, %s, %s, %s)
                """,
                (
                    collection_id,
                    number,
                    entry.get("Chapter_Title_English", ""),
                    entry.get("Arabic_Text", ""),
                    entry.get("English_Text", ""),
                    entry.get("Grade", ""),
                    "Imam al-Bukhari"
                ),
            )

    conn.commit()
    cur.close()
    conn.close()
    print(f"Imported {collection_name} from {filename}")

if __name__ == "__main__":
    import_hadith()

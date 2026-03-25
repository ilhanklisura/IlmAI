"""IlmAI: Import Qur'an data from CSV into PostgreSQL."""
import os
import csv
import psycopg2

DATABASE_URL = os.getenv("DATABASE_URL", "postgresql://ilmai:IlmAI_Dev_2026!@localhost:5432/ilmai")
DATA_DIR = os.path.join(os.path.dirname(__file__), "..", "database", "datasets", "quran")


def import_quran(language: str, filename: str):
    filepath = os.path.join(DATA_DIR, filename)
    if not os.path.exists(filepath):
        print(f"File not found: {filepath}")
        return

    conn = psycopg2.connect(DATABASE_URL)
    cur = conn.cursor()

    with open(filepath, "r", encoding="utf-8") as f:
        reader = csv.DictReader(f)
        for row in reader:
            cur.execute(
                """
                INSERT INTO quran_ayahs (surah_number, ayah_number, text_bosnian, text_english)
                VALUES (%s, %s, %s, %s)
                ON CONFLICT (surah_number, ayah_number) DO UPDATE SET
                    text_bosnian = EXCLUDED.text_bosnian,
                    text_english = EXCLUDED.text_english
                """,
                (
                    int(row.get("surah", 0)),
                    int(row.get("ayah", 0)),
                    row.get("text_bs", ""),
                    row.get("text_en", ""),
                ),
            )

    conn.commit()
    cur.close()
    conn.close()
    print(f"Imported Qur'an data from {filename}")


if __name__ == "__main__":
    import_quran("bs", "quran_bs.csv")
    import_quran("en", "quran_en.csv")

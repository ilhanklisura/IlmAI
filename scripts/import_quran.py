"""IlmAI: Import Qur'an data from datasets into PostgreSQL."""
import os
import csv
import json
import psycopg2

DATABASE_URL = os.getenv("DATABASE_URL", "postgresql://ilmai:IlmAI_Dev_2026!@localhost:5432/ilmai")
DATASETS_DIR = os.path.join(os.path.dirname(__file__), "..", "database", "datasets")
BOSNIAN_CSV = os.path.join(DATASETS_DIR, "bosnian_rwwad_v2.0.4-csv.1.csv")
ENGLISH_DIR = os.path.join(DATASETS_DIR, "quranjson-master", "source", "translation", "en")


def import_quran():
    if not os.path.exists(BOSNIAN_CSV):
        print(f"File not found: {BOSNIAN_CSV}")
        return

    conn = psycopg2.connect(DATABASE_URL)
    cur = conn.cursor()

    current_surah = None
    english_translation = {}
    arabic_translation = {}

    with open(BOSNIAN_CSV, "r", encoding="utf-8") as f:
        # First 11 lines are headers/comments, let's skip until we hit 'id,sura,aya'
        for line in f:
            if line.startswith("id,sura,aya"):
                break
                
        reader = csv.DictReader(f, fieldnames=["id", "sura", "aya", "translation", "footnotes"])

        for row in reader:
            try:
                sura = int(row["sura"])
                aya = int(row["aya"])
            except ValueError:
                continue
                
            text_bs = row.get("translation", "").strip()
            
            if sura != current_surah:
                current_surah = sura
                
                # Load english translation
                en_file = os.path.join(ENGLISH_DIR, f"en_translation_{sura}.json")
                if os.path.exists(en_file):
                    with open(en_file, "r", encoding="utf-8") as enf:
                        en_data = json.load(enf)
                        english_translation = en_data.get("verse", {})
                else:
                    english_translation = {}

                # Load arabic text
                ar_file = os.path.join(DATASETS_DIR, "quranjson-master", "source", "surah", f"surah_{sura}.json")
                if os.path.exists(ar_file):
                    with open(ar_file, "r", encoding="utf-8") as arf:
                        ar_data = json.load(arf)
                        arabic_translation = ar_data.get("verse", {})
                else:
                    arabic_translation = {}
            
            verse_key = f"verse_{aya}"
            text_en = english_translation.get(verse_key, "")
            text_ar = arabic_translation.get(verse_key, "")

            cur.execute(
                """
                INSERT INTO quran_ayahs (surah_number, ayah_number, text_arabic, text_bosnian, text_english)
                VALUES (%s, %s, %s, %s, %s)
                ON CONFLICT (surah_number, ayah_number) DO UPDATE SET
                    text_arabic = EXCLUDED.text_arabic,
                    text_bosnian = EXCLUDED.text_bosnian,
                    text_english = EXCLUDED.text_english
                """,
                (sura, aya, text_ar, text_bs, text_en),
            )

    conn.commit()
    cur.close()
    conn.close()
    print("Imported Qur'an data successfully")


if __name__ == "__main__":
    import_quran()

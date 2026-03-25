"""IlmAI: Chunk documents for RAG pipeline."""
import os
import psycopg2

DATABASE_URL = os.getenv("DATABASE_URL", "postgresql://ilmai:IlmAI_Dev_2026!@localhost:5432/ilmai")
CHUNK_SIZE = 500
CHUNK_OVERLAP = 50


def chunk_text(text: str, chunk_size: int = CHUNK_SIZE, overlap: int = CHUNK_OVERLAP) -> list:
    """Split text into overlapping chunks."""
    words = text.split()
    chunks = []
    start = 0
    while start < len(words):
        end = start + chunk_size
        chunk = " ".join(words[start:end])
        chunks.append(chunk)
        start += chunk_size - overlap
    return chunks


def chunk_quran():
    """Chunk Qur'an ayahs into groups for embedding."""
    conn = psycopg2.connect(DATABASE_URL)
    cur = conn.cursor()

    cur.execute("SELECT surah_number, ayah_number, text_bosnian, text_english FROM quran_ayahs ORDER BY surah_number, ayah_number")
    rows = cur.fetchall()

    # Group by surah, chunk every 5 ayahs
    current_surah = None
    buffer_bs = []
    buffer_en = []
    ayah_start = 1

    for surah, ayah, text_bs, text_en in rows:
        if surah != current_surah:
            if buffer_bs:
                _save_quran_chunk(cur, current_surah, ayah_start, ayah - 1, " ".join(buffer_bs), "bs")
                _save_quran_chunk(cur, current_surah, ayah_start, ayah - 1, " ".join(buffer_en), "en")
            current_surah = surah
            buffer_bs = []
            buffer_en = []
            ayah_start = ayah

        buffer_bs.append(text_bs or "")
        buffer_en.append(text_en or "")

        if len(buffer_bs) >= 5:
            _save_quran_chunk(cur, surah, ayah_start, ayah, " ".join(buffer_bs), "bs")
            _save_quran_chunk(cur, surah, ayah_start, ayah, " ".join(buffer_en), "en")
            buffer_bs = []
            buffer_en = []
            ayah_start = ayah + 1

    conn.commit()
    cur.close()
    conn.close()
    print("Qur'an chunking complete")


def _save_quran_chunk(cur, surah, ayah_start, ayah_end, content, language):
    cur.execute(
        "INSERT INTO quran_chunks (surah_number, ayah_start, ayah_end, content, language) VALUES (%s, %s, %s, %s, %s)",
        (surah, ayah_start, ayah_end, content, language),
    )


if __name__ == "__main__":
    chunk_quran()

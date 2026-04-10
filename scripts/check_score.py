import os
import psycopg2
from openai import OpenAI

DATABASE_URL = os.environ.get('DATABASE_URL')
client = OpenAI(api_key=os.environ.get('OPENAI_API_KEY'))

response = client.embeddings.create(input="Šta se dešava sa nevjernicima i koji im je kraj?", model="text-embedding-3-small")
query_emb = response.data[0].embedding

conn = psycopg2.connect(DATABASE_URL)
cur = conn.cursor()

cur.execute("""
    SELECT surah_number, ayah_start, ayah_end,
           1 - (embedding <=> %s::vector) AS score, content
    FROM quran_chunks 
    WHERE embedding IS NOT NULL 
    ORDER BY embedding <=> %s::vector
    LIMIT 5;
""", (str(query_emb), str(query_emb)))

print("Top results:")
for row in cur.fetchall():
    print(f"Surah {row[0]}, Ayah {row[1]}-{row[2]}, Score: {row[3]:.4f}")
    print(f"{row[4][:100]}...\n")

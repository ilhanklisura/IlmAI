-- IlmAI: Tafsir schema
CREATE TABLE IF NOT EXISTS tafsir_sources (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL UNIQUE,
    author VARCHAR(200),
    language VARCHAR(10) DEFAULT 'bs'
);

CREATE TABLE IF NOT EXISTS tafsir_entries (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    source_id INT NOT NULL REFERENCES tafsir_sources(id),
    surah_number INT NOT NULL,
    ayah_start INT NOT NULL,
    ayah_end INT NOT NULL,
    content TEXT NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS tafsir_chunks (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    source_id INT NOT NULL REFERENCES tafsir_sources(id),
    surah_number INT NOT NULL,
    ayah_start INT NOT NULL,
    ayah_end INT NOT NULL,
    content TEXT NOT NULL,
    language VARCHAR(10) DEFAULT 'bs',
    embedding vector(1536),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_tafsir_chunks_embedding
    ON tafsir_chunks USING hnsw (embedding vector_cosine_ops);

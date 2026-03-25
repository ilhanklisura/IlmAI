-- IlmAI: Qur'an schema
CREATE TABLE IF NOT EXISTS quran_surahs (
    id SERIAL PRIMARY KEY,
    surah_number INT NOT NULL UNIQUE,
    name_arabic VARCHAR(100),
    name_transliteration VARCHAR(100),
    name_bosnian VARCHAR(200),
    name_english VARCHAR(200),
    revelation_type VARCHAR(20), -- 'Meccan' or 'Medinan'
    ayah_count INT NOT NULL
);

CREATE TABLE IF NOT EXISTS quran_ayahs (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    surah_number INT NOT NULL,
    ayah_number INT NOT NULL,
    text_arabic TEXT,
    text_bosnian TEXT,
    text_english TEXT,
    juz INT,
    hizb INT,
    page INT,
    UNIQUE(surah_number, ayah_number)
);

CREATE TABLE IF NOT EXISTS quran_chunks (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    surah_number INT NOT NULL,
    ayah_start INT NOT NULL,
    ayah_end INT NOT NULL,
    content TEXT NOT NULL,
    language VARCHAR(10) DEFAULT 'bs',
    embedding vector(1536),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_quran_chunks_embedding
    ON quran_chunks USING hnsw (embedding vector_cosine_ops);

-- IlmAI: Hadith schema
CREATE TABLE IF NOT EXISTS hadith_collections (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL UNIQUE,
    name_arabic VARCHAR(200),
    author VARCHAR(200),
    hadith_count INT
);

CREATE TABLE IF NOT EXISTS hadiths (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    collection_id INT NOT NULL REFERENCES hadith_collections(id),
    hadith_number INT NOT NULL,
    chapter VARCHAR(500),
    text_arabic TEXT,
    text_bosnian TEXT,
    text_english TEXT,
    grade VARCHAR(50), -- 'sahih', 'hasan', 'daif'
    narrator VARCHAR(500)
);

CREATE TABLE IF NOT EXISTS hadith_chunks (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    collection_id INT NOT NULL REFERENCES hadith_collections(id),
    hadith_number INT NOT NULL,
    content TEXT NOT NULL,
    language VARCHAR(10) DEFAULT 'bs',
    embedding vector(1536),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_hadith_chunks_embedding
    ON hadith_chunks USING hnsw (embedding vector_cosine_ops);

-- Default collections
INSERT INTO hadith_collections (name, author) VALUES
    ('Sahih al-Bukhari', 'Imam al-Bukhari'),
    ('Sahih Muslim', 'Imam Muslim'),
    ('Riyad us-Salihin', 'Imam an-Nawawi')
ON CONFLICT (name) DO NOTHING;

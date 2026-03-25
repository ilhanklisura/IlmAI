-- IlmAI: Seed reference data
-- This file seeds initial reference data for the application

-- Qur'an surahs (first 10 for initial seed)
INSERT INTO quran_surahs (surah_number, name_arabic, name_transliteration, name_bosnian, name_english, revelation_type, ayah_count) VALUES
    (1, 'الفاتحة', 'Al-Fatihah', 'Otvaranje', 'The Opening', 'Meccan', 7),
    (2, 'البقرة', 'Al-Baqarah', 'Krava', 'The Cow', 'Medinan', 286),
    (3, 'آل عمران', 'Ali Imran', 'Imranova porodica', 'Family of Imran', 'Medinan', 200),
    (4, 'النساء', 'An-Nisa', 'Žene', 'The Women', 'Medinan', 176),
    (5, 'المائدة', 'Al-Ma''idah', 'Trpeza', 'The Table Spread', 'Medinan', 120),
    (6, 'الأنعام', 'Al-An''am', 'Stoka', 'The Cattle', 'Meccan', 165),
    (7, 'الأعراف', 'Al-A''raf', 'Bedemi', 'The Heights', 'Meccan', 206),
    (8, 'الأنفال', 'Al-Anfal', 'Plijen', 'The Spoils of War', 'Medinan', 75),
    (9, 'التوبة', 'At-Tawbah', 'Pokajanje', 'The Repentance', 'Medinan', 129),
    (10, 'يونس', 'Yunus', 'Junus', 'Jonah', 'Meccan', 109)
ON CONFLICT (surah_number) DO NOTHING;

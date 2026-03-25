"""IlmAI: Normalize and clean text data."""
import re
import unicodedata


def normalize_arabic(text: str) -> str:
    """Remove diacritics from Arabic text for better matching."""
    arabic_diacritics = re.compile(r'[\u0610-\u061A\u064B-\u065F\u0670\u06D6-\u06DC\u06DF-\u06E4\u06E7\u06E8\u06EA-\u06ED]')
    return arabic_diacritics.sub('', text)


def normalize_bosnian(text: str) -> str:
    """Normalize Bosnian text."""
    text = unicodedata.normalize('NFC', text)
    text = re.sub(r'\s+', ' ', text).strip()
    return text


def clean_text(text: str) -> str:
    """General text cleaning."""
    text = re.sub(r'\n{3,}', '\n\n', text)
    text = re.sub(r' {2,}', ' ', text)
    return text.strip()


if __name__ == "__main__":
    print("Text normalization utilities ready.")
    print("Import and use: from normalize_text import normalize_bosnian, clean_text")

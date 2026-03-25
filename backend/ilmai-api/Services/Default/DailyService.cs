namespace IlmAI.Api.Services.Default;

using IlmAI.Api.Models.Response.Daily;

public class DailyService : IDailyService
{
    public Task<DailyContentResponse> GetDailyContentAsync(string language)
    {
        return Task.FromResult(new DailyContentResponse
        {
            Ayah = new DailyItem
            {
                SurahName = "Al-Fatihah",
                AyahReference = "1:1",
                TextArabic = "بِسْمِ اللَّهِ الرَّحْمَٰنِ الرَّحِيمِ",
                TextTranslation = language == "bs"
                    ? "U ime Allaha, Milostivog, Samilosnog"
                    : "In the name of Allah, the Most Gracious, the Most Merciful",
                Explanation = language == "bs"
                    ? "Ova Bismila je otvaranje Kur'ana i podsjeća nas da svaki čin započinjemo sa Allahovim imenom."
                    : "This Bismillah is the opening of the Quran and reminds us to begin every act in the name of Allah."
            },
            Hadith = new DailyItem
            {
                HadithCollection = "Sahih Bukhari",
                HadithNumber = 1,
                TextArabic = "إِنَّمَا الأَعْمَالُ بِالنِّيَّاتِ",
                TextTranslation = language == "bs"
                    ? "Djela se cijene prema namjerama"
                    : "Actions are judged by intentions",
                Explanation = language == "bs"
                    ? "Ovaj hadis nas podsjeća da je iskrenost osnova svakog ibadeta."
                    : "This hadith reminds us that sincerity is the foundation of every worship."
            },
            Date = DateTime.UtcNow.Date
        });
    }
}

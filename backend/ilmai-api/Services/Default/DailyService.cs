namespace IlmAI.Api.Services.Default;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IlmAI.Api.Models.Data;
using IlmAI.Api.Models.Response.Daily;

public class DailyService : IDailyService
{
    private readonly AppDbContext _db;

    public DailyService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DailyContentResponse> GetDailyContentAsync(string language)
    {
        var queryAyahs = _db.QuranAyahs.Where(a => 
            (a.TextBosnian != null && a.TextBosnian.Length <= 350) || 
            (a.TextEnglish != null && a.TextEnglish.Length <= 350));
            
        var queryHadiths = _db.Hadiths.Where(h => 
            (h.TextBosnian != null && h.TextBosnian.Length <= 450) || 
            (h.TextEnglish != null && h.TextEnglish.Length <= 450));

        var totalAyahs = await queryAyahs.CountAsync();
        var totalHadiths = await queryHadiths.CountAsync();

        var ayahIndex = totalAyahs > 0 ? DateTime.UtcNow.DayOfYear % totalAyahs : 0;
        var hadithIndex = totalHadiths > 0 ? DateTime.UtcNow.DayOfYear % totalHadiths : 0;

        var dailyAyah = totalAyahs > 0 ? await queryAyahs.OrderBy(a => a.SurahNumber).ThenBy(a => a.AyahNumber).Skip(ayahIndex).FirstOrDefaultAsync() : null;
        var dailyHadith = totalHadiths > 0 ? await queryHadiths.Include(h => h.Collection).OrderBy(h => h.Id).Skip(hadithIndex).FirstOrDefaultAsync() : null;

        string ayahSurahName = "";
        if (dailyAyah != null)
        {
            var surah = await _db.QuranSurahs.FirstOrDefaultAsync(s => s.SurahNumber == dailyAyah.SurahNumber);
            ayahSurahName = language == "bs" ? (surah?.NameBosnian ?? "Surah") : (surah?.NameEnglish ?? "Surah");
        }

        var res = new DailyContentResponse
        {
            Date = DateTime.UtcNow.Date
        };

        if (dailyAyah != null)
        {
            res.Ayah = new DailyItem
            {
                SurahName = ayahSurahName,
                AyahReference = $"{dailyAyah.SurahNumber}:{dailyAyah.AyahNumber}",
                TextArabic = dailyAyah.TextArabic ?? "",
                TextTranslation = language == "bs" ? (dailyAyah.TextBosnian ?? "") : (dailyAyah.TextEnglish ?? ""),
                Explanation = ""
            };
        }
        else
        {
            // Fallback
            res.Ayah = new DailyItem
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
            };
        }

        if (dailyHadith != null)
        {
            res.Hadith = new DailyItem
            {
                HadithCollection = dailyHadith.Collection?.Name ?? "Hadith",
                HadithNumber = dailyHadith.HadithNumber,
                TextArabic = dailyHadith.TextArabic ?? "",
                TextTranslation = language == "bs" ? (dailyHadith.TextBosnian ?? "") : (dailyHadith.TextEnglish ?? ""),
                Explanation = !string.IsNullOrEmpty(dailyHadith.Narrator) ?
                    (language == "bs" ? $"Prenosi {dailyHadith.Narrator}" : $"Narrated by {dailyHadith.Narrator}") : ""
            };
        }
        else
        {
            // Fallback
            res.Hadith = new DailyItem
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
            };
        }

        return res;
    }
}

namespace IlmAI.AI.Services;
using IlmAI.AI.Data;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

public record RetrievedChunk(string DocumentTitle, string Content, string? StartTime, string? EndTime, double Score);

public interface IRetrievalService : IService
{
    Task<List<RetrievedChunk>> RetrieveAsync(string query, Vector queryEmbedding, int topK, double scoreThreshold, string language = "bs", CancellationToken ct = default);
}

public class RetrievalService : IRetrievalService
{
    private readonly AIDataContext _ctx;
    public RetrievalService(AIDataContext ctx) { _ctx = ctx; }

    public async Task<List<RetrievedChunk>> RetrieveAsync(string query, Vector queryEmbedding, int topK, double scoreThreshold, string language = "bs", CancellationToken ct = default)
    {
        var results = new List<RetrievedChunk>();
        
        var q = query.ToLowerInvariant();
        bool isHadithQuery = q.Contains("hadith") || q.Contains("hadis") || q.Contains("bukhari") || q.Contains("muslim");
        bool isQuranQuery = q.Contains("quran") || q.Contains("kuran") || q.Contains("surah") || q.Contains("ajet") || q.Contains("ayah");

        // Search Quran chunks (Exclude if purely a hadith query)
        if (!isHadithQuery || isQuranQuery)
        {
            var quranResults = await _ctx.QuranChunks
                .Where(c => c.Embedding != null && c.Language == language)
                .OrderBy(c => c.Embedding!.CosineDistance(queryEmbedding))
                .Take(topK)
                .Select(c => new { c.Content, c.SurahNumber, c.AyahStart, c.AyahEnd,
                    Score = 1 - c.Embedding!.CosineDistance(queryEmbedding) })
                .ToListAsync(ct);

            results.AddRange(quranResults
                .Where(r => r.Score >= scoreThreshold)
                .Select(r => new RetrievedChunk($"Quran {r.SurahNumber}:{r.AyahStart}-{r.AyahEnd}", r.Content, null, null, r.Score + (isQuranQuery ? 0.1 : 0))));
        }

        // Search Hadith chunks (Exclude if purely a quran query)
        if (!isQuranQuery || isHadithQuery)
        {
            var hadithResults = await _ctx.HadithChunks
                .Where(c => c.Embedding != null && c.Language == language)
                .OrderBy(c => c.Embedding!.CosineDistance(queryEmbedding))
                .Take(topK)
                .Select(c => new { c.Content, c.CollectionId, c.HadithNumber,
                    Score = 1 - c.Embedding!.CosineDistance(queryEmbedding) })
                .ToListAsync(ct);

            results.AddRange(hadithResults
                .Where(r => r.Score >= scoreThreshold)
                .Select(r => new RetrievedChunk($"Hadith {r.CollectionId}:{r.HadithNumber}", r.Content, null, null, r.Score + (isHadithQuery ? 0.1 : 0))));
        }

        // Search Tafsir chunks
        if (!isHadithQuery)
        {
            var tafsirResults = await _ctx.TafsirChunks
                .Where(c => c.Embedding != null && c.Language == language)
                .OrderBy(c => c.Embedding!.CosineDistance(queryEmbedding))
                .Take(topK)
                .Select(c => new { c.Content, c.SurahNumber, c.AyahStart, c.AyahEnd,
                    Score = 1 - c.Embedding!.CosineDistance(queryEmbedding) })
                .ToListAsync(ct);

            results.AddRange(tafsirResults
                .Where(r => r.Score >= scoreThreshold)
                .Select(r => new RetrievedChunk($"Tafsir {r.SurahNumber}:{r.AyahStart}-{r.AyahEnd}", r.Content, null, null, r.Score + (isQuranQuery ? 0.05 : 0))));
        }

        // Search general document chunks
        var docResults = await _ctx.DocumentChunks
            .Include(c => c.Document)
            .Where(c => c.Embedding != null && c.Document.Language == language)
            .OrderBy(c => c.Embedding!.CosineDistance(queryEmbedding))
            .Take(topK)
            .Select(c => new { c.Content, DocumentTitle = c.Document.Title,
                Score = 1 - c.Embedding!.CosineDistance(queryEmbedding) })
            .ToListAsync(ct);

        results.AddRange(docResults
            .Where(r => r.Score >= scoreThreshold)
            .Select(r => new RetrievedChunk(r.DocumentTitle, r.Content, null, null, r.Score)));

        return results.OrderByDescending(r => r.Score).Take(topK).ToList();
    }
}

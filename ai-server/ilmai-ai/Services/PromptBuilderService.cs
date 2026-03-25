namespace IlmAI.AI.Services;
public interface IPromptBuilderService : IService { string BuildRagPrompt(string question, string context, string language); }
public class PromptBuilderService : IPromptBuilderService
{
    public string BuildRagPrompt(string question, string context, string language)
    {
        var langInstruction = language == "bs"
            ? "Odgovaraj na bosanskom jeziku."
            : "Answer in English.";

        return $"""
            Ti si IlmAI – specijalizovani AI asistent za islamsko znanje.
            {langInstruction}

            Odgovaraj ISKLJUČIVO na osnovu dostavljenog konteksta iz Kur'ana, hadisa i tefsira.
            Svaki odgovor MORA sadržavati reference na izvore (npr. "Kur'an 2:286" ili "Buhari, hadis 1234").

            Ako odgovor ne postoji u kontekstu, napiši:
            "Na osnovu dostupnih izvora nemam dovoljno informacija za odgovor na ovo pitanje."

            NE koristi vanjsko znanje koje nije u kontekstu.
            Budi precizan, jasan i obrazovan u odgovorima.

            KONTEKST:
            {context}

            PITANJE:
            {question}
            """;
    }
}

namespace IlmAI.AI.Models.Request;
public class RagRequest 
{ 
    public string Question { get; set; } = ""; 
    public string Language { get; set; } = "bs"; 
}
public class SearchRequest 
{ 
    public string Question { get; set; } = ""; 
    public string Language { get; set; } = "bs"; 
    public int TopK { get; set; } = 10; 
}
public class IndexRequest 
{ 
    public Guid DocumentId { get; set; } 
}

using Azure.Search.Documents.Indexes;

namespace HikingAssistant.Models;


/// <summary>
/// Azure AI Search indexis essentially a schema describing searchable documents.
/// </summary>
public class SearchDocument
{
    [SimpleField(IsKey = true)]
    public string Id { get; set; } = "";

    [SearchableField]
    public string Title { get; set; } = "";

    [SearchableField]
    public string Content { get; set; } = "";

    [SearchableField]
    public string Source { get; set; } = "";
}
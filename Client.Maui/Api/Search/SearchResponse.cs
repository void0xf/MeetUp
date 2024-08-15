namespace Client.Maui.Api.Search;

public class SearchResponse
{
    public List<Event> Results { get; set; }
    public int PageCount { get; set; }
    public int TotalCount { get; set; }
}

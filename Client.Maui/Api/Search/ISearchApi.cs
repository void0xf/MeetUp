using Refit;

namespace Client.Maui.Api.Search;

public interface ISearchApi
{
    [Get("/search")]
    Task<ApiResponse<SearchResponse>> SearchAsync(
        [Query] string searchTerm = "",
        [Query] int page = 1,
        [Query] int pageSize = 10
    );

    [Get("/MeetEvent")]
    Task<List<Event>> SearchMeetAsync();
}

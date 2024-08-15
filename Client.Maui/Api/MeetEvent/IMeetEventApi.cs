using Refit;

namespace Client.Maui.Api.MeetEvent
{
    public interface IMeetEventApi
    {
        [Get("/MeetEvent/AddUserToParticipantList/{Id}")]
        Task<IApiResponse> JoinToEvent(string Id, [Header("Authorization")] string authorization);

        [Post("/MeetEvent")]
        Task<ApiResponse<Event>> CreateEventAsync(
            [Body] CreateEventDto eventDto,
            [Header("Authorization")] string authorization
        );

        [Get("/MeetEvent/me")]
        Task<List<Event>> GetMyEvents([Header("Authorization")] string authorization);
    }

    public class CreateEventDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime EventStartDate { get; set; }

        public DateTime EventEndDate { get; set; }

        public string Location { get; set; }

        public string Author { get; set; }

        public string Visibility { get; set; }

        public List<string> Images { get; set; }
    }
}

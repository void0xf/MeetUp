using Refit;

namespace Client.Maui.Api.Users
{
    public interface IMeetEventClient
    {
        [Get("/api/MeetEvent")]
        Task<ApiResponse<ICollection<MeetEventDto>>> GetMeetEvents();
    }
}

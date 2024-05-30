using Refit;

namespace Client.Maui.Api.Users
{
    public interface IAuth
    {
        [Headers("Content-Type: application/x-www-form-urlencoded")]
        [Post("/connect/token")]
        Task<HttpResponseMessage> Execute(
            [Body(BodySerializationMethod.UrlEncoded)] AuthTokenRequest config
        );

        [Get("/api/MeetEvent")]
        Task<ApiResponse<MeetEventDto>> GetMeetEvents();
    }
}

using Refit;

namespace Client.Maui.Api.Users
{
    public interface IUserApi
    {
        [Get("/users/{username}")]
        Task<UserInfo> GetUserInfo(string username, [Header("Authorization")] string authorization);

        [Post("/users")]
        Task<ApiResponse<DtoUserInfo>> CreateUser(
            DtoUserInfo userInfo,
            [Header("Authorization")] string authorization
        );
    }
}

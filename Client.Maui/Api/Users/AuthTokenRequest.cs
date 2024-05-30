using Refit;

namespace Client.Maui.Api.Users
{
    public class AuthTokenRequest
    {
        [AliasAs("grant_type")]
        public string GrantType { get; set; }

        [AliasAs("username")]
        public string Username { get; set; }

        [AliasAs("password")]
        public string Password { get; set; }

        [AliasAs("client_id")]
        public string ClientId { get; set; }

        [AliasAs("client_secret")]
        public string ClientSecret { get; set; }

        [AliasAs("scope")]
        public string Scope { get; set; }
    }
}

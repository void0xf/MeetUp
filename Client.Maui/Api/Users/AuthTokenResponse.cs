using Refit;

namespace Client.Maui.Api.Users;

public class MeetEventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime EventStartDate { get; set; }
    public DateTime EventEndDate { get; set; }
    public string Location { get; set; }
    public string Author { get; set; }
    public string Visibility { get; set; }
    public List<string> Images { get; set; }
}

public class AuthTokenResponse
{
    [AliasAs("access_token")]
    public string AccessToken { get; set; }

    [AliasAs("expires_in")]
    public int ExpiresIn { get; set; }

    [AliasAs("token_type")]
    public string TokenType { get; set; }

    // Add any additional properties from the JSON response
    [AliasAs("scope")]
    public string Scope { get; set; }
}

namespace Client.Maui.Api.Users;

public class UserInfo
{
    public string Description { get; set; }
    public string Fullname { get; set; }
    public string Username { get; set; }
    public string WhoCanMessage { get; set; }
    public List<string> ChatsId { get; set; } = new List<string>();
}

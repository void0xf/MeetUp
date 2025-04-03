namespace UserService.DTOs
{
    // Base DTO with common properties
    public class UserInfoDto
    {
        public string Description { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
    }

    // DTO for creating a new user
    public class CreateUserDto
    {
        public string Description { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
    }

    // DTO for updating an existing user
    public class UpdateUserDto
    {
        public string Description { get; set; }
        public string Fullname { get; set; }
        public string WhoCanMessage { get; set; }
        public List<string> ChatsId { get; set; }
    }

    // DTO for returning user information in responses
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string WhoCanMessage { get; set; }
        public List<string> ChatsId { get; set; }
    }
}

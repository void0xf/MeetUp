namespace Client.Maui.Api
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string Location { get; set; }
        public string Author { get; set; }
        public string Visibility { get; set; }
        public List<string> Participants { get; set; }
        public string ConversationId { get; set; }
        public List<string> Images { get; set; }
    }
}

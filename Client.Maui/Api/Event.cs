namespace Client.Maui.Api
{
    public class Event
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string Location { get; set; }
        public string Author { get; set; }
        public string Visibility { get; set; }
        public List<string> Images { get; set; }
        public string Id { get; set; }
    }
}

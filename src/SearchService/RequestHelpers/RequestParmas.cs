namespace SearchService.RequestHelpers
{
    public class RequestParmas
    {
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 3;
        public string OrderBy { get; set; }
        public string FilterBy { get; set; }
    }
}

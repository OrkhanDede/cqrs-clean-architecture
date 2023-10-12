namespace Core.Models
{
    public class PagingParameters
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 25;
        public bool IsAll { get; set; }
    }
}

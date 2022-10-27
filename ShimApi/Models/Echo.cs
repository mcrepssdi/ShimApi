namespace ShimApi.Models
{
    public class Echo
    {
        public string CommitId { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool CanConnect { get; set; }
    }
}
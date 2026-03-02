namespace Zenit.Share.Business.Requests
{
    public class ResendEmailRequest
    {
        public required string From { get; set; }
        public required string To { get; set; }
        public required string Subject { get; set; }
        public string? HtmlBody { get; set; }
    }
}
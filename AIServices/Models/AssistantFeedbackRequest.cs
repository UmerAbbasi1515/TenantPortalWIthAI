namespace AIServices.Models
{
    public class AssistantFeedbackRequest
    {
        public string ConversationId { get; set; }
        public bool Helpful { get; set; }
        public string? Comment { get; set; }
        public int UserId { get; set; }
        public string? portalName { get; set; }
    }
}

namespace AIServices.Models
{
    public class AIResponseModel<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Response { get; set; } = default;
        public string[] Suggestions { get; set; } = Array.Empty<string>();

        public string? ConversationId { get; set; }

        public string? ErrorCode { get; set; }
    }
    public class AIResponseModel : AIResponseModel<string>
    {
    }

    public sealed class AIConversationDTO
    {
        public Guid ConversationId { get; set; }

        public bool FeedbackSubmitted { get; set; }

        public IReadOnlyList<AIConversationMessageDTO> Messages { get; set; } = Array.Empty<AIConversationMessageDTO>();
    }

    public class AIConversationMessageDTO
    {
        public long Id { get; set; }

        public int UserId { get; set; }

        public string ConversationId { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string? Content { get; set; }

        public string? ToolCallId { get; set; }

        public string? ToolCallsJson { get; set; }

        public string? ActionText { get; set; }

        public string? ActionUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class ConversationListItemVM
    {
        public string ConversationId { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? FirstMessagePreview { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

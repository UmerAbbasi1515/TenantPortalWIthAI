namespace AIServices.Models
{
    public sealed class AssistantHelpContentItem
    {
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> Keywords { get; set; } = [];
        public string Body { get; set; } = string.Empty;
        public string ActionText { get; set; }
        public string ActionUrl { get; set; }
    }
}

using System;

public class FaqApiResponse
{
    public string? Status { get; set; }
    public List<QuestionData>? FaqCategories { get; set; }
    public string? Message { get; set; }
}

public class FaqsViewModelResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public List<QuestionData>? Data { get; set; }
    }

    public class QuestionData
    {
        public string? CategoryId { get; set; }
        public string? Title { get; set; }
        public string? TitleAR { get; set; }
    }
//
public class FaqDetailApiResponse
{
    public string? Status { get; set; }
    public List<FaqData>? Faq { get; set; }
    public string? Message { get; set; }
}
public class FaqsDetailViewModelResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public List<FaqData>? Data { get; set; }
    }

    public class FaqData
    {
        public string? Title { get; set; }
        public string? TitleAR { get; set; }
        public string? Description { get; set; }
        public string? DescriptionAR { get; set; }
    }




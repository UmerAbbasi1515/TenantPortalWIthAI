using AIServices.Constants;
using AIServices.Models;
using System.Text.RegularExpressions;

namespace AIServices.Helpers
{
    public class AIAssistantHelper
    {

        public static bool IsBlockedInput(string question)
        {
            var normalized = question.Trim();
            return AIAssistantConstant.InjectionSignals.Any(signal => normalized.Contains(signal, StringComparison.OrdinalIgnoreCase));
        }
        public static bool TryResolveHelp(string question, out AIResponseModel response)
        {
            var normalized = question.Trim();
            var (Item, Score) = FindBestMatch(normalized);

            if (Item is null)
            {
                response = null!;
                return false;
            }

            var hasHowToSignal = IsHowToQuestion(normalized);

            if (Score >= 2 || (Score == 1 && hasHowToSignal))
            {
                response = BuildContentResponse(Item);
                return true;
            }

            if (hasHowToSignal)
            {
                response = BuildNoGuideResponse();
                return true;
            }

            response = null!;
            return false;
        }
        static (AssistantHelpContentItem Item, int Score) FindBestMatch(string normalized)
        {
            AssistantHelpContentItem bestItem = null;
            var bestScore = 0;

            foreach (var item in AIAssistantConstant.HelpContent)
            {
                if (string.IsNullOrWhiteSpace(item.Key) || string.IsNullOrWhiteSpace(item.Body)) continue;

                var score = item.Keywords.Count(keyword =>
                    !string.IsNullOrWhiteSpace(keyword) && normalized.Contains(keyword, StringComparison.OrdinalIgnoreCase));

                if (score > bestScore)
                {
                    bestScore = score;
                    bestItem = item;
                }
            }

            return (bestItem, bestScore);
        }
        static AIResponseModel BuildContentResponse(AssistantHelpContentItem item)
        {
            return new AIResponseModel
            {
                IsSuccess = true,
                Message = "Response generated successfully.",
                Response = item.Body,
                Suggestions = new[]
                {
                    "Which landlords can I access?",
                    "How do I search for a landlord?"
                }
            };
        }

        private static AIResponseModel BuildNoGuideResponse()
        {
            var support = AIAssistantConstant.SupportContact;

            return new AIResponseModel
            {
                IsSuccess = true,
                Message = $"There is no guide available for against this question.",
                Response = $"I do not have a step-by-step guide for that yet. Please contact {support} and they will help you.",
                Suggestions = new[]
                {
                    "Which landlords can I access?",
                    "How do I search for a landlord?"
                }
            };
        }

        static bool IsHowToQuestion(string normalized)
        {
            return Regex.IsMatch(normalized,
                @"\b(how\b|steps|step-by-step|procedure|guide|create|creating|add|adding|new|make|making|register|insert|set up|setup|form)\b");
        }
        public static AIResponseModel BlockedInputResponse() => new()
        {
            IsSuccess = true,
            Message = "Such type of Input keyword are not allowed.",
            Response = "I can't process that request. Please ask a question about landlords, properties, units, contracts, rents, or balances.",
            Suggestions = new[]
                        {
                            "Which landlords can I access?",
                            "Which properties can I access?"
                        }
        };
    }
}

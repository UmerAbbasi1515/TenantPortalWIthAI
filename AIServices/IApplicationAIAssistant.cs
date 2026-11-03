using AIServices.Helpers;
using AIServices.Models;
using UBSAIAgent.Models.ManualAgent;
using UBSAIAgent.Services;

namespace AIServices
{
    public interface IApplicationAIAssistant
    {
        public Task<AIResponseModel> CreateOrUpdateViewEmbeddings(CancellationToken cancellationToken);
        public Task<AIResponseModel> SyncLocalViewsVectorsWithQdrant(CancellationToken cancellationToken);
        Task<AIResponseModel> AskAgentAsync(string message, int userId, string portalName, CancellationToken cancellationToken);
        Task<AIResponseModel<List<string>>> GetSuggestionsAsync(int userId, string portalName, string? module, CancellationToken cancellationToken);
        Task<AIResponseModel<AIConversationDTO>> GetConversationByIdAsync(string ConversationId, int userId, string portalName, int take, CancellationToken cancellationToken);
        Task<AIResponseModel> EndConversationAsync(string? ConversationId, int userId, string portalName, CancellationToken cancellationToken);
        Task<AIResponseModel> SaveFeedBacksAsync(AssistantFeedbackRequest request, CancellationToken cancellationToken);
        Task<AIResponseModel<AIConversationDTO>> GetActiveConversationMessagesAsync(int userId, string portalName, int messageCount, CancellationToken cancellationToken);

        // Conversation list
        Task<AIResponseModel<IReadOnlyList<ConversationListItemVM>>> GetConversationsAsync(int userId, string portalName, CancellationToken cancellationToken);

        // Conversation delete
        Task<AIResponseModel> DeleteConversationAsync(string ConversationId, int userId, string portalName, CancellationToken cancellationToken);
    } 
    public class ApplicationAIAssistant : IApplicationAIAssistant
    {
        private readonly IAIAgentService _aiAgentService;

        public ApplicationAIAssistant(IAIAgentService aiAgentService)
        {
            _aiAgentService = aiAgentService;
        }

        public async Task<AIResponseModel> AskAgentAsync(string message, int userId, string portalName, CancellationToken cancellationToken)
        {
            try
            {
                if (AIAssistantHelper.IsBlockedInput(message))
                {
                    return AIAssistantHelper.BlockedInputResponse();
                }

                if (AIAssistantHelper.TryResolveHelp(message, out var guide))
                {
                    return guide;
                }

                var response = await _aiAgentService.AskAgentAsync(message, userId, portalName, cancellationToken);
                
                var result = new AIResponseModel {
                    ConversationId = response.ConversationId,
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    Response = response.Response,
                    Suggestions = response.Suggestions,
                    ErrorCode = response.ErrorCode
                 };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<AIResponseModel<List<string>>> GetSuggestionsAsync(int userId, string portalName,string? module, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _aiAgentService.GetSuggestionsAsync(userId, portalName, module, cancellationToken);

                var result = new AIResponseModel<List<string>>
                {
                    ConversationId = response.ConversationId,
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    Response = response.Response,
                    Suggestions = response.Suggestions,
                    ErrorCode = response.ErrorCode
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<AIResponseModel<AIConversationDTO>> GetConversationByIdAsync(string ConversationId, int userId, string portalName, int take, CancellationToken cancellationToken)
        {
            try
            {
                if (!Guid.TryParse(ConversationId, out var conversationGuid))
                {
                    return new AIResponseModel<AIConversationDTO>
                    {
                        IsSuccess = false,
                        Message = "Invalid conversation id."
                    };
                }

                var AIOutput = await _aiAgentService.GetConversationDetailByIdAsync(conversationGuid, userId, portalName, take, cancellationToken);

                var result = AIOutput != null ? new AIResponseModel<AIConversationDTO>
                {
                    ConversationId = AIOutput.ConversationId,
                    IsSuccess = AIOutput.IsSuccess,
                    Message = AIOutput.Message,
                    ErrorCode = AIOutput.ErrorCode,
                    Suggestions = AIOutput.Suggestions,
                    Response = AIOutput.Response != null ? new AIConversationDTO
                    {
                        ConversationId = AIOutput.Response.ConversationId,
                        FeedbackSubmitted = AIOutput.Response.FeedbackSubmitted,
                        Messages = AIOutput.Response.Messages?.Select(m => new AIConversationMessageDTO
                        {
                            Id = m.Id,
                            UserId = m.UserId,
                            ConversationId = m.ConversationId,
                            Role = m.Role,
                            Content = m.Content,
                            ToolCallId = m.ToolCallId,
                            ToolCallsJson = m.ToolCallsJson,
                            ActionText = m.ActionText,
                            ActionUrl = m.ActionUrl,
                            CreatedAt = m.CreatedAt
                        }).ToList() ?? new List<AIConversationMessageDTO>() // Falls back to an empty list if Messages is null
                    } : null // Sets the inner Response object to null if the source Response is null
                } : null; // Sets the entire result to null if the root response is null

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<AIResponseModel> EndConversationAsync(string? ConversationId, int userId, string portalName, CancellationToken cancellationToken)
        {
            try
            {
                var conversationGuid = string.IsNullOrWhiteSpace(ConversationId) ? (Guid?)null : Guid.Parse(ConversationId);
                var response = await _aiAgentService.EndConversationAsync(conversationGuid, userId, portalName, cancellationToken);

                var result = new AIResponseModel
                {
                    ConversationId = response.ConversationId,
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    Response = response.Response,
                    Suggestions = response.Suggestions,
                    ErrorCode = response.ErrorCode
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<AIResponseModel> SaveFeedBacksAsync(AssistantFeedbackRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _aiAgentService.SaveFeedBacksAsync(Guid.Parse(request.ConversationId),request.UserId,request.portalName,request.Helpful,request.Comment, cancellationToken);

                var result = new AIResponseModel
                {
                    ConversationId = response.ConversationId,
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    Response = response.Response,
                    Suggestions = response.Suggestions,
                    ErrorCode = response.ErrorCode
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AIResponseModel<AIConversationDTO>> GetActiveConversationMessagesAsync(int userId, string portalName, int messageCount, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _aiAgentService.GetActiveConversationMessagesAsync(userId, portalName, messageCount, cancellationToken);

                var result = new AIResponseModel<AIConversationDTO>
                {
                    ConversationId = response.Response?.ConversationId.ToString(),
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    ErrorCode = response.ErrorCode,
                    Response = response.Response != null ? new AIConversationDTO
                    {
                        ConversationId = response.Response.ConversationId,
                        FeedbackSubmitted = response.Response.FeedbackSubmitted,
                        Messages = response.Response.Messages?.Select(m => new AIConversationMessageDTO
                        {
                            Id = m.Id,
                            UserId = m.UserId,
                            ConversationId = m.ConversationId,
                            Role = m.Role,
                            Content = m.Content,
                            ToolCallId = m.ToolCallId,
                            ToolCallsJson = m.ToolCallsJson,
                            ActionText = m.ActionText,
                            ActionUrl = m.ActionUrl,
                            CreatedAt = m.CreatedAt
                        }).ToList() ?? new List<AIConversationMessageDTO>()
                    } : null
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AIResponseModel<IReadOnlyList<ConversationListItemVM>>> GetConversationsAsync(int userId, string portalName, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _aiAgentService.GetConversationsAsync(userId, portalName, cancellationToken);

                var result = new AIResponseModel<IReadOnlyList<ConversationListItemVM>>
                {
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    ErrorCode = response.ErrorCode,
                    Response = response.Response?.Select(c => new ConversationListItemVM
                    {
                        ConversationId = c.ConversationId.ToString(),
                        Summary = c.Summary,
                        FirstMessagePreview = c.FirstMessagePreview,
                        UpdatedAt = c.UpdatedAt
                    }).ToList() ?? new List<ConversationListItemVM>()
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AIResponseModel> DeleteConversationAsync(string ConversationId, int userId, string portalName, CancellationToken cancellationToken)
        {
            try
            {
                if (!Guid.TryParse(ConversationId, out var conversationGuid))
                {
                    return new AIResponseModel
                    {
                        IsSuccess = false,
                        Message = "Invalid conversation id."
                    };
                }

                var response = await _aiAgentService.DeleteConversationAsync(conversationGuid, userId, portalName, cancellationToken);
                return new AIResponseModel
                {
                    IsSuccess = response.IsSuccess,
                    Message = response.Message,
                    ErrorCode = response.ErrorCode,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AIResponseModel> CreateOrUpdateViewEmbeddings(CancellationToken cancellationToken)
        {
            try
            {
                await _aiAgentService.CreateOrUpdateViewEmbeddings(cancellationToken);
                var result = new AIResponseModel
                {
                    IsSuccess = true,
                    Message = "Embeddings generated successfully.",
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AIResponseModel> SyncLocalViewsVectorsWithQdrant(CancellationToken cancellationToken)
        {
            try
            {
                await _aiAgentService.SyncLocalRecordWithQdrant(cancellationToken);
                var result = new AIResponseModel
                {
                    IsSuccess = true,
                    Message = "Local data embeddings sync with Qdrant successfully.",
                };
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

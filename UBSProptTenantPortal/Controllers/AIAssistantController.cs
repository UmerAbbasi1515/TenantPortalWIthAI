
using AIServices;
using AIServices.Constants;
using AIServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pms.Controllers
{
    [AllowAnonymous]
    [Route("AIAssistant")]
    public class AIAssistantController : BaseController
    {
        private const string PortalName = "PMS";
        private const string ConversationSessionKey = AIAssistantSessionKeys.Conversation;
        private const string LoginIdSessionKey = AIAssistantSessionKeys.LoginId;
        private const int DefaultRestoreMessageCount = 50;
        private const int MaxRestoreMessageCount = 200;

        private readonly IApplicationAIAssistant _agentService;
        public AIAssistantController(IApplicationAIAssistant applicationAIAssistant)
        {
            _agentService = applicationAIAssistant;
        }

        private string? GetSessionConversationId()
        {
            var value = HttpContext.Session.GetString(ConversationSessionKey);
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private async Task<AIResponseModel<AIConversationDTO>?> RestoreSessionConversationAsync(int take, CancellationToken cancellationToken)
        {
            var conversationId = GetSessionConversationId();

            if (conversationId is not null)
            {
                return await _agentService.GetConversationByIdAsync(conversationId, UserID, PortalName, take, cancellationToken);
            }

            if (HttpContext.Session.GetString(LoginIdSessionKey) != GetCurrentLoginId())
            {
                return null;
            }

            var active = await _agentService.GetActiveConversationMessagesAsync(UserID, PortalName, take, cancellationToken);

            if (active?.Response?.ConversationId is Guid adopted && adopted != Guid.Empty)
            {
                HttpContext.Session.SetString(ConversationSessionKey, adopted.ToString());

                return active;
            }

            return null;
        }

        private string GetCurrentLoginId()
        {
            var loginHistoryId = HttpContext.Session.GetInt32(AIAssistantSessionKeys.LoginHistoryId);

            return loginHistoryId?.ToString() ?? UserID.ToString();
        }

        private async Task EndPreviousConversationsAsync()
        {
            var loginId = GetCurrentLoginId();

            if (HttpContext.Session.GetString(LoginIdSessionKey) == loginId)
            {
                return;
            }

            HttpContext.Session.SetString(LoginIdSessionKey, loginId);

            if (GetSessionConversationId() is null)
            {
                await _agentService.EndConversationAsync(null, UserID, PortalName, CancellationToken.None);
            }
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetConversations")]
        public async Task<IActionResult> GetConversations()
        {
            var result = await _agentService.GetConversationsAsync(UserID, PortalName, CancellationToken.None);
            return Ok(result);
        }

        [HttpGet("GetConversationMessages")]
        public async Task<IActionResult> GetConversationMessages([FromQuery] string conversationId, [FromQuery] int messageCount = 10)
        {
            if (string.IsNullOrWhiteSpace(conversationId))
            {
                return Ok(new AIResponseModel<AIConversationDTO> { IsSuccess = true, Message = "No conversation id provided." });
            }

            // Selecting a thread from history makes it the active conversation, so subsequent
            // messages continue in that same thread.
            if (Guid.TryParse(conversationId, out _))
            {
                HttpContext.Session.SetString(ConversationSessionKey, conversationId);
            }

            // History click loads the last N messages of the thread (the agent applies TOP(@Take) to this value).
            var take = Math.Clamp(messageCount, 1, MaxRestoreMessageCount);
            var result = await _agentService.GetConversationByIdAsync(conversationId, UserID, PortalName, take, CancellationToken.None);
            return Ok(result);
        }

        [HttpDelete("DeleteConversation")]
        public async Task<IActionResult> DeleteConversation([FromQuery] string conversationId)
        {
            if (string.IsNullOrWhiteSpace(conversationId))
            {
                return Ok(new AIResponseModel { IsSuccess = false, Message = "No conversation id provided." });
            }

            var result = await _agentService.DeleteConversationAsync(conversationId, UserID, PortalName, CancellationToken.None);
            if (!result.IsSuccess)
            {
                return Ok(result);
            }

            // If the user deleted the active session thread, drop the cursor so the next Ask
            // starts a fresh conversation instead of continuing into a deleted one.
            if (string.Equals(GetSessionConversationId(), conversationId, StringComparison.OrdinalIgnoreCase))
            {
                HttpContext.Session.Remove(ConversationSessionKey);
            }

            return Ok(result);
        }

        [HttpGet("Ask")]
        public async Task<IActionResult> Ask([FromQuery] string prompt)
        {
            await EndPreviousConversationsAsync();

            var result = await _agentService.AskAgentAsync(prompt, UserID, PortalName, CancellationToken.None);

            if (!string.IsNullOrWhiteSpace(result?.ConversationId))
            {
                HttpContext.Session.SetString(ConversationSessionKey, result.ConversationId!);
            }

            return Ok(result);
        }
        [HttpGet("Questions")]
        public async Task<IActionResult> Questions()
        {
            var result = await _agentService.GetSuggestionsAsync(UserID, PortalName, null, CancellationToken.None);
            return Ok(result);
        }
        [HttpGet("GetConversation")]
        public async Task<IActionResult> GetConversation()
        {
            var conversationId = GetSessionConversationId();
            if (conversationId is null)
            {
                return Ok(new AIResponseModel { IsSuccess = true, Message = "No conversation for this session." });
            }

            var result = await _agentService.GetConversationByIdAsync(conversationId, UserID, PortalName, DefaultRestoreMessageCount, CancellationToken.None);
            return Ok(result);
        }
        [HttpGet("EndConversation")]
        public async Task<IActionResult> EndConversation()
        {
            var conversationId = GetSessionConversationId();

            try
            {
                var result = await _agentService.EndConversationAsync(conversationId, UserID, PortalName, CancellationToken.None);
                return Ok(result);
            }
            finally
            {
                HttpContext.Session.Remove(ConversationSessionKey);
            }
        }
        [HttpGet("SaveFeedBack")]
        public async Task<IActionResult> SaveFeedBack([FromForm] AssistantFeedbackRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ConversationId))
            {
                request.ConversationId = GetSessionConversationId() ?? string.Empty;
            }

            if (!Guid.TryParse(request.ConversationId, out _))
            {
                return Ok(new AIResponseModel { IsSuccess = false, Message = "No conversation for this session." });
            }

            request.UserId = UserID;
            request.portalName = PortalName;
            var result = await _agentService.SaveFeedBacksAsync(request, CancellationToken.None);
            return Ok(result);
        }

        [HttpGet("GetActiveConversationMessages")]
        public async Task<IActionResult> GetActiveConversationMessages([FromQuery] int messageCount = DefaultRestoreMessageCount)
        {
            await EndPreviousConversationsAsync();

            var take = Math.Clamp(messageCount, 1, MaxRestoreMessageCount);

            var result = await RestoreSessionConversationAsync(take, CancellationToken.None);

            return Ok(result ?? new AIResponseModel<AIConversationDTO>
            {
                IsSuccess = true,
                Message = "No conversation for this session."
            });
        }

        [AllowAnonymous]
        [HttpGet("GenerateEmbiddingForViews")]
        public async Task<IActionResult> GenerateEmbiddingForViews()
        {
            var result = await _agentService.CreateOrUpdateViewEmbeddings(CancellationToken.None);
            return RedirectToAction("Login", "Account", new { CallFrom = "System" });
        }

        [AllowAnonymous]
        [HttpGet("SyncLocalViewsVectorsWithQdrant")]
        public async Task<IActionResult> SyncLocalViewsVectorsWithQdrant()
        {
            var result = await _agentService.SyncLocalViewsVectorsWithQdrant(CancellationToken.None);
            return RedirectToAction("Login", "Account", new { CallFrom = "System" });
        }
    }
}

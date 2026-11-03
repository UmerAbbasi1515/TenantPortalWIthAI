only.js


document.addEventListener("DOMContentLoaded", function () {
    "use strict";

    // --- DOM Elements ---
    var root = document.getElementById("aiAssistant");
    if (!root) return;

    var panel = document.getElementById("aiAssistantPanel");
    var launcher = document.getElementById("aiAssistantLauncher");
    var closeBtn = document.getElementById("aiAssistantClose");
    var clearBtn = document.getElementById("aiAssistantClear");
    var conversation = document.getElementById("aiAssistantConversation");
    var starter = document.getElementById("aiAssistantStarter");
    var questionsContainer = document.getElementById("aiAssistantQuestions");
    var form = document.getElementById("aiAssistantForm");
    var input = document.getElementById("aiAssistantInput");

    if (!panel || !launcher || !closeBtn || !clearBtn ||
        !conversation || !starter || !questionsContainer || !form || !input) {
        return;
    }

    // --- API Endpoints ---
    var API = {
        QUESTIONS: "/AIAssistant/Questions",
        ASK: "/AIAssistant/Ask",
        GET_CONVERSATION: "/AIAssistant/GetConversation",
        GET_ACTIVE_CONVERSATION: "/AIAssistant/GetActiveConversationMessages",
        END_CONVERSATION: "/AIAssistant/EndConversation",
        FEEDBACK: "/AIAssistant/SaveFeedBack"
    };

    var isSending = false;
    var currentConversationId = null;
    var hasRestored = false;
    var RESTORE_MESSAGE_COUNT = 50;

    // --- Toggle Panel ---
    function openPanel() {
        root.classList.add("is-open");
        input.focus();

        if (hasRestored) {
            return;
        }

        hasRestored = true;

        restoreConversation().then(loadQuestions);
    }

    function closePanel() {
        root.classList.remove("is-open");
    }

    // --- Fetch Suggested Questions from API ---
    function loadQuestions() {
        questionsContainer.innerHTML = '<span class="ai-assistant__loading">Loading suggestions...</span>';
        showStarter();

        fetch(API.QUESTIONS)
            .then(function (res) { return res.json(); })
            .then(function (data) {
                var questions = data.Response || [];
                renderQuestions(questions);
            })
            .catch(function () {
                questionsContainer.innerHTML = '<span class="ai-assistant__loading">Unable to load suggestions.</span>';
            });
    }

    function showStarter() {
        var hasMessages = conversation.querySelector(".ai-assistant__message") !== null;

        starter.classList.toggle("d-none", hasMessages);
    }

    function clearMessages() {
        var messages = conversation.querySelectorAll(".ai-assistant__message");

        Array.prototype.forEach.call(messages, function (node) {
            node.remove();
        });

        showStarter();
    }

    function restoreConversation() {
        return fetch(API.GET_ACTIVE_CONVERSATION + "?messageCount=" + RESTORE_MESSAGE_COUNT)
            .then(function (res) { return res.json(); })
            .then(function (data) {
                if (!data.IsSuccess || !data.Response || !data.Response.Messages || data.Response.Messages.length === 0) {
                    return;
                }

                currentConversationId = data.Response.ConversationId;
                clearMessages();
                data.Response.Messages.forEach(function (msg) {
                    appendMessage(msg.Content, msg.Role);
                });

                showStarter();
                scrollToBottom();
            })
            .catch(function () { });
    }

    // --- Render Question Buttons ---
    function renderQuestions(questions) {
        questionsContainer.innerHTML = "";

        if (!questions || questions.length === 0) {
            starter.classList.add("d-none");
            return;
        }

        questions.forEach(function (text) {
            var btn = document.createElement("button");
            btn.type = "button";
            btn.className = "ai-assistant__question";
            btn.textContent = text;
            btn.addEventListener("click", function () {
                sendMessage(text);
            });
            questionsContainer.appendChild(btn);
        });
    }

    // --- Send Message ---
    function sendMessage(text) {
        var prompt = (text || input.value).trim();
        if (!prompt || isSending) return;

        isSending = true;
        input.value = "";
        starter.classList.add("d-none");

        appendMessage(prompt, "user");
        showTyping();

        var url = API.ASK + "?prompt=" + encodeURIComponent(prompt);

        fetch(url)
            .then(function (res) { return res.json(); })
            .then(function (data) {
                removeTyping();

                if (data.IsSuccess && data.Response) {
                    if (data.ConversationId) {
                        currentConversationId = data.ConversationId;
                    }
                    appendMessage(data.Response, "assistant");

                    if (data.Suggestions && data.Suggestions.length > 0) {
                        renderQuestions(data.Suggestions);
                    }
                    showStarter();
                } else {
                    appendMessage("Unable to process your request at this time.", "assistant", true);
                }
            })
            .catch(function () {
                removeTyping();
                appendMessage("Error connecting to server. Please try again.", "assistant", true);
            })
            .finally(function () {
                isSending = false;
            });
    }

    // --- Append Chat Message ---
    function appendMessage(text, sender, isError) {
        var wrapper = document.createElement("div");
        wrapper.className = "ai-assistant__message";
        if (sender === "user") wrapper.className += " ai-assistant__message--user";
        if (isError) wrapper.className += " ai-assistant__message--error";

        var bubble = document.createElement("div");
        bubble.className = "ai-assistant__bubble";
        bubble.textContent = text;

        wrapper.appendChild(bubble);
        conversation.insertBefore(wrapper, starter);
        scrollToBottom();

        return wrapper;
    }

    // --- Typing Indicator ---
    function showTyping() {
        var indicator = document.createElement("div");
        indicator.id = "aiTyping";
        indicator.className = "ai-assistant__message ai-assistant__typing";
        indicator.innerHTML = '<div class="ai-assistant__bubble"><span></span><span></span><span></span></div>';
        conversation.insertBefore(indicator, starter);
        scrollToBottom();
    }

    function removeTyping() {
        var el = document.getElementById("aiTyping");
        if (el) el.remove();
    }

    // --- Scroll ---
    function scrollToBottom() {
        conversation.scrollTop = conversation.scrollHeight;
    }

    // --- Clear Conversation ---
    function clearConversation() {
        fetch(API.END_CONVERSATION, { method: "GET" })
            .then(resetPanel)
            .catch(resetPanel);
    }

    function resetPanel() {
        currentConversationId = null;
        hasRestored = true;
        clearMessages();
        loadQuestions();
    }

    // --- Event Listeners ---
    launcher.addEventListener("click", openPanel);
    closeBtn.addEventListener("click", closePanel);
    clearBtn.addEventListener("click", clearConversation);

    form.addEventListener("submit", function (e) {
        e.preventDefault();
        sendMessage();
    });

    input.addEventListener("keydown", function (e) {
        if (e.key === "Enter" && !e.shiftKey) {
            e.preventDefault();
            sendMessage();
        }
    });
});

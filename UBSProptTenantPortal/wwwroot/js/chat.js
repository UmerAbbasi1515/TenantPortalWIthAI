document.addEventListener("DOMContentLoaded", function () {
    "use strict";

    var input = document.getElementById("chatPromptInput");
    var sendBtn = document.getElementById("chatSendBtn");
    var questionsContainer = document.getElementById("assistantSuggestedQuestions");
    var suggestionsWrap = document.getElementById("assistantSuggestionsWrap");
    var messagesContainer = document.getElementById("chatMessages");
    var userList = document.getElementById("chatUserList");
    var userSearchInput = document.getElementById("userSearchInput");
    var userSearchBtn = document.getElementById("userSearchBtn");
    var newChatBtn = document.getElementById("assistantNewChatBtn");
    var sidebar = document.getElementById("chatgptSidebar");
    var sidebarToggleBtn = document.getElementById("sidebarToggleBtn");

    var API = {
        QUESTIONS: "/AIAssistant/Questions",
        MSG: "/Chat/SendMessage",
        GET_USERS: "/Chat/GetUsers",
        GET_CHAT: "/Chat/UserChat",
        END_CONVERSATION: "/AIAssistant/EndConversation"
    };

    var isSending = false;
    var currentUserId = null;

    function scrollToBottom() {
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }

    function renderMessage(text, name, user) {
        var msgDiv = document.createElement("div");
        msgDiv.className = "message message--" + user;

        var avatar = document.createElement("div");
        avatar.className = "message-avatar";
        let initials = name
            .trim()
            .split(/\s+/)
            .map(word => word.charAt(0).toUpperCase())
            .join("");
        avatar.textContent = initials;

        var bubble = document.createElement("div");
        bubble.className = "message-bubble";
        bubble.textContent = text;

        msgDiv.appendChild(avatar);
        msgDiv.appendChild(bubble);

        messagesContainer.appendChild(msgDiv);
        scrollToBottom();
        return msgDiv;
    }

    function showThinking() {
        var msgDiv = document.createElement("div");
        msgDiv.className = "message message--assistant message--thinking";
        msgDiv.setAttribute("role", "status");
        msgDiv.setAttribute("aria-label", "Assistant is thinking");

        var avatar = document.createElement("div");
        avatar.className = "message-avatar";
        avatar.textContent = "A";

        var bubble = document.createElement("div");
        bubble.className = "message-bubble";
        bubble.innerHTML = '<span class="typing-dots"><span></span><span></span><span></span></span>';

        msgDiv.appendChild(avatar);
        msgDiv.appendChild(bubble);
        messagesContainer.appendChild(msgDiv);
        scrollToBottom();
        return msgDiv;
    }

    function clearMessages() {
        messagesContainer.innerHTML = "";
    }

    function removeEmptyState() {
        var empty = messagesContainer.querySelector(".message-empty");
        if (empty) {
            empty.remove();
        }
    }

    function showEmptyState() {
        if (messagesContainer.children.length === 0) {
            var empty = document.createElement("div");
            empty.className = "message-empty";
            empty.textContent = "Start a conversation by asking a question.";
            messagesContainer.appendChild(empty);
        }
    }

    function escapeHtml(text) {
        if (!text) return "";
        var div = document.createElement("div");
        div.textContent = text;
        return div.innerHTML;
    }

    function formatDate(value) {
        if (!value) return "";
        var date = new Date(value);
        if (isNaN(date.getTime())) return "";
        return date.toLocaleDateString();
    }

    function truncate(text, max) {
        if (!text) return "";
        return text.length > max ? text.slice(0, max) + "\u2026" : text;
    }

    function renderSuggestions(list) {
        if (!suggestionsWrap || !list || !list.length) return;

        questionsContainer.innerHTML = "";

        list.forEach(function (text) {
            if (!text) return;

            var btn = document.createElement("button");
            btn.type = "button";
            btn.textContent = text;
            btn.addEventListener("click", function () {
                sendMessage(text);
            });
            questionsContainer.appendChild(btn);
        });

        suggestionsWrap.classList.remove("is-hidden");
    }

    function hideSuggestions() {
        if (suggestionsWrap) {
            suggestionsWrap.classList.add("is-hidden");
        }
        questionsContainer.innerHTML = "";
    }

    function loadQuestions() {
        fetch(API.QUESTIONS)
            .then(function (res) { return res.json(); })
            .then(function (data) {
                renderSuggestions(data.Response || []);
            })
            .catch(function () {
            });
    }

    function loadUsers() {
        debugger;
        fetch(API.GET_USERS)
            .then(function (res) { return res.json(); })
            .then(function (data) {
                var users = data || [];
                if (!users.length) {
                    userList.innerHTML = '<li class="user-empty">No users yet.</li>';
                    return;
                }

                users.forEach(function (user) {
                    var item = document.createElement("li");
                    item.className = "user-item";
                    item.dataset.userId = user.UserId;

                    //if (currentConversationId && conv.ConversationId === currentConversationId) {
                    //    item.classList.add("active");
                    //}

                    var title = user.UserName + ' - ' + user.FirstName + ' ' + user.LastName;
                    //var date = formatDate(conv.UpdatedAt);

                    item.innerHTML =
                        '<div class="user-item__body">' +
                        '<div class="user-item__title">' + escapeHtml(title) + "</div>" +
                        "</div>" +
                        '<span class="dot-label bg-success"></span>';

                    item.addEventListener("click", function () {
                        selectUser(user.UserId);
                    });

                    var deleteBtn = item.querySelector(".user-delete");
                    if (deleteBtn) {
                        deleteBtn.addEventListener("click", function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            //deleteConversation(conv.ConversationId, item);
                        });
                    }

                    userList.appendChild(item);
                });

                filterConversations(userSearchInput ? userSearchInput.value : "");
            })
            .catch(function () {
                userList.innerHTML = '<li class="user-error">Failed to load user.</li>';
            });
    }

    function filterConversations(query) {
        var q = (query || "").toLowerCase().trim();
        var visible = 0;

        userList.querySelectorAll(".user-item").forEach(function (item) {
            var titleEl = item.querySelector(".user-item__title");
            var match = !q || (titleEl && titleEl.textContent.toLowerCase().indexOf(q) !== -1);
            item.style.display = match ? "" : "none";
            if (match) {
                visible++;
            }
        });

        var empty = userList.querySelector(".user-search-empty");
        if (visible === 0 && userList.querySelectorAll(".user-item").length > 0) {
            if (!empty) {
                empty = document.createElement("li");
                empty.className = "user-search-empty";
                empty.textContent = "No matching conversations.";
                userList.appendChild(empty);
            }
        } else if (empty) {
            empty.remove();
        }
    }

    function selectUser(userId) {
        if (isSending) return;

        userList.querySelectorAll(".user-item").forEach(function (item) {
            item.classList.toggle("active", item.dataset.user === userId);
        });
        $('.chat-main').show();
        $('#receiverId').val(userId);
        getChat(userId);


    }
    function getChat(userId) {
        clearMessages();

        fetch(API.GET_CHAT + "?userId=" + encodeURIComponent(userId) + "&messageCount=10")
            .then(function (res) { return res.json(); })
            .then(function (data) {
                debugger;
                if (data.Messages) {
                    data.Messages.forEach(function (msg) {
                        var user;
                        if (msg.SenderId === data.LoggedInUserId)
                            user = 'sender';
                        else
                            user = 'receiver';
                        renderMessage(msg.MessageText || "", msg.SenderNickName, user);
                    });
                }
                showEmptyState();
            })
            .catch(function () {
                showEmptyState();
            });
    }
    function newChat() {
        if (isSending) return;

        currentUserId = null;
        userList.querySelectorAll(".user-item").forEach(function (item) {
            item.classList.remove("active");
        });

        clearMessages();
        hideSuggestions();
        showEmptyState();

        fetch(API.END_CONVERSATION).then(function () {
            loadQuestions();
            loadUsers();
        }).catch(function () {
            loadQuestions();
            loadUsers();
        });
    }

    //function deleteConversation(conversationId, item) {
    //    if (!conversationId) return;

    //    AsyncConfirmYesNo(
    //        "Delete Conversation",
    //        "Are you sure you want to delete this conversation?",
    //        function () {
    //            fetch(API.DELETE_CONVERSATION + "?conversationId=" + encodeURIComponent(conversationId), {
    //                method: "DELETE"
    //            })
    //                .then(function (res) { return res.json(); })
    //                .then(function (data) {
    //                    if (data && data.IsSuccess) {
    //                        if (item && item.parentNode) {
    //                            item.remove();
    //                        }

    //                        if (currentuserId === conversationId) {
    //                            currentuserId = null;
    //                            clearMessages();
    //                            showEmptyState();
    //                        }

    //                        loadUsers();
    //                    } else {
    //                        window.alert((data && data.Message) || "Failed to delete conversation.");
    //                    }
    //                })
    //                .catch(function () {
    //                    window.alert("Failed to delete conversation.");
    //                });
    //        },
    //        function () {
    //            // User cancelled: do nothing
    //        }
    //    );
    //}

    function sendMessage(text) {
        var msg = (text || input.value).trim();

        if (!msg || isSending) return;

        // Get the selected receiver's ID
        var receiverId = document.getElementById("receiverId").value;

        isSending = true;
        input.value = "";
        input.disabled = true;
        sendBtn.disabled = true;

        removeEmptyState();

        // Show the sent message immediately
        // renderMessage(msg, "sender");

        fetch(API.MSG, {
            method: "POST",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            body: new URLSearchParams({
                message: msg,
                receiverId: receiverId
            })
        })
            .then(function (res) {
                if (!res.ok) {
                    throw new Error("Failed to send message");
                }

                return res.json();
            })
            .then(function (data) {
                if (data.IsSuccess) {
                    getChat(receiverId);
                    loadUsers();
                } else {
                    renderMessage(
                        "Unable to send your message. Please try again.",
                        "Receiver"
                    );
                }
            })
            .catch(function (error) {
                console.error(error);
                renderMessage(
                    "Error sending message. Please try again.",
                    "Receiver"
                );
            })
            .finally(function () {
                isSending = false;
                input.disabled = false;
                sendBtn.disabled = false;
                input.focus();
            });
    }

    //function setSidebarState(open) {
    //    if (!sidebar) return;

    //    if (open) {
    //        sidebar.classList.remove("is-collapsed");
    //        if (sidebarToggleBtn) {
    //            var icon = sidebarToggleBtn.querySelector("i");
    //            if (icon) icon.className = "fa-solid fa-chevron-left";
    //        }
    //    } else {
    //        sidebar.classList.add("is-collapsed");
    //        if (sidebarToggleBtn) {
    //            var icon = sidebarToggleBtn.querySelector("i");
    //            if (icon) icon.className = "fa-solid fa-chevron-right";
    //        }
    //    }
    //}

    //if (sidebar && sidebar.classList.contains("is-collapsed")) {
    //    setSidebarState(false);
    //} else {
    //    setSidebarState(true);
    //}

    //if (sidebarToggleBtn) {
    //    sidebarToggleBtn.addEventListener("click", function () {
    //        setSidebarState(sidebar.classList.contains("is-collapsed"));
    //    });
    //}

    loadUsers();
    loadQuestions();
    showEmptyState();

    sendBtn.addEventListener("click", function () {

        sendMessage();
    });

    if (newChatBtn) {
        newChatBtn.addEventListener("click", newChat);
    }

    if (userSearchInput) {
        userSearchInput.addEventListener("input", function () {
            filterConversations(this.value);
        });

        userSearchInput.addEventListener("keydown", function (e) {
            if (e.key === "Enter") {
                e.preventDefault();
                filterConversations(this.value);
            }
        });
    }

    if (userSearchBtn) {
        userSearchBtn.addEventListener("click", function () {
            filterConversations(userSearchInput ? userSearchInput.value : "");
        });
    }

    input.addEventListener("keydown", function (e) {
        if (e.key === "Enter" && !e.shiftKey) {
            e.preventDefault();
            sendMessage();
        }
    });
});

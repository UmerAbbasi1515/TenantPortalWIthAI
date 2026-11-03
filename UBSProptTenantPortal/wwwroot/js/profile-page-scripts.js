(function () {
    function initFaqsDetail() {
        const faqList = document.querySelector('.faq-list');
        if (!faqList) {
            return;
        }

        faqList.addEventListener('click', function (event) {
            const toggle = event.target.closest('.faq-header');
            if (!toggle) {
                return;
            }

            event.preventDefault();
            const item = toggle.closest('.faq-item');
            if (!item) {
                return;
            }

            const description = item.querySelector('.faq-description');
            if (!description) {
                return;
            }

            const isHidden = window.getComputedStyle(description).display === 'none';
            description.style.display = isHidden ? 'block' : 'none';
        });
    }

    document.addEventListener('DOMContentLoaded', initFaqsDetail);

    function clickOnTab(id) {
        if (!id) return;

        sessionStorage.setItem('activeNotificationTab', id);

        document.querySelectorAll('.nav-link').forEach(function (tab) {
            tab.classList.remove('active');
        });

        var activeTabId = id + '-tab';
        var activeTab = document.getElementById(activeTabId);
        if (activeTab) {
            activeTab.classList.add('active');
        }

        var readPanel = document.getElementById('ReadRequestID');
        var unreadPanel = document.getElementById('UnReadRequestID');

        if (readPanel) {
            readPanel.style.display = id === 'Read' ? 'block' : 'none';
        }
        if (unreadPanel) {
            unreadPanel.style.display = id === 'Unread' ? 'block' : 'none';
        }
    }

    function restoreTab() {
        var savedTab = sessionStorage.getItem('activeNotificationTab') || 'Read';
        clickOnTab(savedTab);
    }

    function toggleMenu(dotIcon) {
        if (!dotIcon) return;

        document.querySelectorAll('.menu-dropdown').forEach(function (menu) {
            menu.classList.remove('show');
        });

        var dropdown = dotIcon.nextElementSibling;
        if (dropdown) {
            dropdown.classList.toggle('show');
        }
    }

    function closeAllMenus() {
        document.querySelectorAll('.menu-dropdown').forEach(function (menu) {
            menu.classList.remove('show');
        });
    }

    function handleNotificationClick(event, notificationId) {
        if (!notificationId) return;

        setTimeout(function () {
            var activeTab = sessionStorage.getItem('activeNotificationTab');
            if (activeTab === 'Unread') {
                var item = document.querySelector('.dot[data-id="' + notificationId + '"]');
                if (item) {
                    var li = item.closest('li');
                    if (li) {
                        var lineBreak = li.nextElementSibling;
                        li.remove();
                        if (lineBreak && lineBreak.classList.contains('line')) {
                            lineBreak.remove();
                        }
                    }
                }
            }
        }, 3000);
    }

    function btnshowLoaderN(el) {
        if (!el) return;
        el.dataset.originalText = el.innerHTML;
        el.innerHTML = '<i class="fa fa-spinner fa-spin"></i> Loading...';
        el.style.pointerEvents = 'none';
    }

    function btnhideLoaderN(el) {
        if (!el) return;
        el.innerHTML = el.dataset.originalText || 'Mark as Read';
        el.style.pointerEvents = '';
    }

    //async function markAsReadFunction(el, NotificationId) {
    //    if (!el || !NotificationId) return;

    //    btnshowLoaderN(el);

    //    try {
    //        var response = await fetch('/Notifications/Notification/ReadNoti1fications', {
    //            method: 'POST',
    //            headers: { 'Content-Type': 'application/json' },
    //            body: JSON.stringify({ NotificationId: NotificationId.toString() })
    //        });

    //        if (response.ok) {
    //            closeAllMenus();

    //            document.querySelectorAll('.dot[data-id="' + NotificationId + '"]').forEach(function (d) {
    //                d.classList.remove('unread-dot');
    //            });

    //            var container = document.getElementById('UnReadRequestID');
    //            if (container) {
    //                var item = container.querySelector('.dot[data-id="' + NotificationId + '"]');
    //                if (item) {
    //                    var li = item.closest('li');
    //                    if (li) {
    //                        var lineBreak = li.nextElementSibling;
    //                        li.remove();
    //                        if (lineBreak && lineBreak.classList.contains('line')) {
    //                            lineBreak.remove();
    //                        }
    //                    }
    //                }
    //            }
    //        } else {
    //            console.error('Request failed:', response.statusText);
    //        }
    //    } catch (error) {
    //        console.error('Validation Error:', error);
    //    } finally {
    //        btnhideLoaderN(el);
    //    }
    //}

    async function markAsReadFunction(el, NotificationId) {
        if (!el || !NotificationId) return;

        btnshowLoaderN(el);

        try {
            var response = await fetch('/Notifications/Notification/ReadNoti1fications', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ NotificationId: NotificationId.toString() })
            });

            if (response.ok) {
                // ADDED: read the JSON safely (won't break if body is empty or not JSON)
                var jsonResponse = null;
                try {
                    jsonResponse = await response.json();
                } catch (e) {
                    jsonResponse = null;
                }

                closeAllMenus();

                document.querySelectorAll('.dot[data-id="' + NotificationId + '"]').forEach(function (d) {
                    d.classList.remove('unread-dot');
                });

                var container = document.getElementById('UnReadRequestID');
                if (container) {
                    var item = container.querySelector('.dot[data-id="' + NotificationId + '"]');
                    if (item) {
                        var li = item.closest('li');
                        if (li) {
                            var lineBreak = li.nextElementSibling;
                            li.remove();
                            if (lineBreak && lineBreak.classList.contains('line')) {
                                lineBreak.remove();
                            }
                        }
                    }
                }

                // ADDED: update header badge with the new unread count
                if (jsonResponse && jsonResponse.unreadCount !== undefined) {
                    updateNotificationBadge(jsonResponse.unreadCount);
                }
            } else {
                console.error('Request failed:', response.statusText);
            }
        } catch (error) {
            console.error('Validation Error:', error);
        } finally {
            btnhideLoaderN(el);
        }
    }

    // ADDED: plain JS helper (no jQuery dependency), put it in the same .js file
    function updateNotificationBadge(count) {
        var badge = document.getElementById('unreadNotificationBadge');
        if (!badge) return;
        badge.textContent = count;
        badge.style.display = (!count || parseInt(count) === 0) ? 'none' : '';
    }
    async function markAsArchiveFunction(el, NotificationId) {
        if (!el || !NotificationId) return;

        btnshowLoaderN(el);

        try {
            var response = await fetch('/Notifications/Notification/ArchiveNoti1fications', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ NotificationId: NotificationId.toString() })
            });

            if (response.ok) {
                var activeTabId = sessionStorage.getItem('activeNotificationTab') === 'Unread'
                    ? 'UnReadRequestID'
                    : 'ReadRequestID';

                var container = document.getElementById(activeTabId);
                if (container) {
                    var item = container.querySelector('.dot[data-id="' + NotificationId + '"]');
                    if (item) {
                        var li = item.closest('li');
                        if (li) {
                            var lineBreak = li.nextElementSibling;
                            li.remove();
                            if (lineBreak && lineBreak.classList.contains('line')) {
                                lineBreak.remove();
                            }
                        }
                    }
                }

                closeAllMenus();
            }
        } catch (error) {
            console.error('Validation Error:', error);
        } finally {
            btnhideLoaderN(el);
        }
    }

    function markAsRead(el, id) {
        markAsReadFunction(el, id);
    }

    function archive(el, id) {
        markAsArchiveFunction(el, id);
    }

    function getFaqsDetail() {
        var divDescription = document.getElementById('descriptionDivID');
        if (!divDescription) return;

        divDescription.style.display = divDescription.style.display === 'none' ? 'block' : 'none';
    }

    async function getProfileData(error) {
        var loadingIndicator = document.getElementById('loadingIndicator');

        try {
            if (error === 'Unauthorized') {
                window.location.href = '/Auth/AuthScreen';
                return;
            }

            if (loadingIndicator) {
                loadingIndicator.style.display = 'block';
            }

            var response = await fetch('/Profile/Profile', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            });

            if (!response.ok) {
                console.error('Error in response:', response.status);
            }
        } catch (err) {
            alert('An error occurred.');
            console.error(err);
        } finally {
            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }
        }
    }

    window.clickOnTab = clickOnTab;
    window.restoreTab = restoreTab;
    window.toggleMenu = toggleMenu;
    window.closeAllMenus = closeAllMenus;
    window.handleNotificationClick = handleNotificationClick;
    window.markAsRead = markAsRead;
    window.archive = archive;
    window.getFaqsDetail = getFaqsDetail;
    window.getProfileData = getProfileData;

    document.addEventListener('DOMContentLoaded', function () {
        restoreTab();

        document.querySelectorAll('a[data-tab]').forEach(function (tabLink) {
            tabLink.addEventListener('click', function (event) {
                event.preventDefault();
                clickOnTab(this.dataset.tab);
            });
        });

        document.querySelectorAll('.action-dots').forEach(function (dot) {
            dot.addEventListener('click', function (event) {
                event.stopPropagation();
                toggleMenu(this);
            });
        });

        document.querySelectorAll('[data-action="mark-as-read"]').forEach(function (button) {
            button.addEventListener('click', function (event) {
                event.preventDefault();
                event.stopPropagation();
                markAsRead(this, this.dataset.id);
            });
        });

        document.querySelectorAll('[data-action="archive"]').forEach(function (button) {
            button.addEventListener('click', function (event) {
                event.preventDefault();
                event.stopPropagation();
                archive(this, this.dataset.id);
            });
        });

        document.querySelectorAll('[data-action="notification-link"]').forEach(function (link) {
            link.addEventListener('click', function (event) {
                handleNotificationClick(event, this.dataset.id);
            });
        });

        document.querySelectorAll('.toggle-description').forEach(function (button) {
            button.addEventListener('click', function (event) {
                event.preventDefault();
                getFaqsDetail();
            });
        });

        document.addEventListener('click', function (e) {
            if (!e.target.closest('.action-menu')) {
                closeAllMenus();
            }
        });
    });
})();

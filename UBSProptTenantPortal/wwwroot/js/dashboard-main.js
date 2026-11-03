(function () {
    const config = window.dashboardMainConfig || {};

    function isBooleanTrue(value) {
        return value === true || value === "true" || value === "True";
    }

    function getNotifications() {
        return config.notifications?.popUp?.notifications || [];
    }

    function updateView(index) {
        const notifications = getNotifications();
        const notificationItem = notifications[index];
        const node = document.getElementById(`notification-${index}`);
        if (!notificationItem || !node) return;

        const title = node.getAttribute('data-title');
        const desc = node.getAttribute('data-description');
        const total = config.totalNotifications || 0;

        const titleEl = document.getElementById('modalTitle');
        const bodyEl = document.getElementById('notificationBody');
        const paginationEl = document.getElementById('paginationLabel');
        const prevBtn = document.getElementById('prevBtn');
        const nextBtn = document.getElementById('nextBtn');

        titleEl.innerText = title || '';
        bodyEl.innerHTML = `<p>${desc || ''}</p>`;
        paginationEl.innerText = `${index + 1} / ${total}`;

        prevBtn.disabled = index === 0;
        nextBtn.disabled = index === total - 1;

        markAsReadFunction(notificationItem.notificationId);
    }

    async function markAsReadFunction(NotificationId) {
        try {
            const response = await fetch('/Notifications/Notification/ReadNoti1fications', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ NotificationId: NotificationId?.toString() })
            });

            if (!response.ok) {
                console.error('Request failed:', response.statusText);
                return;
            }

            if (typeof closeAllMenus === 'function') {
                closeAllMenus();
            }

            const dots = document.querySelectorAll(`.dot[data-id='${NotificationId}']`);
            dots.forEach(d => d.classList.remove('unread-dot'));

            const activeTabId = sessionStorage.getItem('activeNotificationTab') === 'Unread'
                ? 'UnReadRequestID'
                : 'ReadRequestID';

            const container = document.getElementById(activeTabId);
            const item = container?.querySelector(`.dot[data-id='${NotificationId}']`)?.closest('li');
            if (item) {
                const lineBreak = item.nextElementSibling;
                item.remove();
                if (lineBreak?.classList.contains('line')) {
                    lineBreak.remove();
                }
            }
        } catch (error) {
            console.error('Validation Error:', error);
        }
    }

    function bindNotificationControls() {
        const prevBtn = document.getElementById('prevBtn');
        const nextBtn = document.getElementById('nextBtn');
        const total = config.totalNotifications || 0;

        if (!prevBtn || !nextBtn || total === 0) return;

        let currentIndex = 0;

        prevBtn.addEventListener('click', () => {
            if (currentIndex > 0) {
                currentIndex -= 1;
                updateView(currentIndex);
            }
        });

        nextBtn.addEventListener('click', () => {
            if (currentIndex < total - 1) {
                currentIndex += 1;
                updateView(currentIndex);
            }
        });

        updateView(currentIndex);
    }

    function initNotificationModal() {
        const total = config.totalNotifications || 0;
        const showPopUp = isBooleanTrue(config.showPopUp);

        if (total > 0 && showPopUp) {
            setTimeout(() => {
                const modalElement = document.getElementById('notificationModal');
                if (modalElement && typeof bootstrap !== 'undefined') {
                    const modal = new bootstrap.Modal(modalElement);
                    modal.show();
                }
            }, 300);
        }
    }

    function initDonutChart() {
        const rentalVal = Number(config.rentalVal || 0);
        const rentOutstanding = Number(config.rentOutstanding || 0);
        const receivedVal = rentalVal - rentOutstanding > 0 ? rentalVal - rentOutstanding : 0;
        const canvas = document.getElementById('myDonutChart');

        if (!canvas || typeof Chart === 'undefined') return;

        const ctx = canvas.getContext('2d');
        new Chart(ctx, {
            type: 'pie',
            data: {
                datasets: [{
                    label: '',
                    data: [receivedVal, rentOutstanding],
                    backgroundColor: ['rgba(0, 98, 255, 1)', 'rgba(255, 179, 0, 1)'],
                    borderColor: ['rgba(0, 98, 255, 1)', 'rgba(255, 179, 0, 1)'],
                    borderWidth: 2
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { display: true },
                    datalabels: {
                        color: '#ffffff',
                        anchor: 'center',
                        align: 'center',
                        formatter: (value) => value
                    }
                }
            },
            plugins: [ChartDataLabels]
        });
    }

    window.ClosePopUp = function () {
        const modalElement = document.getElementById('notificationModal');
        if (modalElement && typeof bootstrap !== 'undefined') {
            const modal = new bootstrap.Modal(modalElement);
            modal.hide();
        }
    };

    function initResetButton() {
        const resetButton = document.getElementById('resetButton');
        if (!resetButton) return;

        resetButton.addEventListener('click', function (e) {
            e.preventDefault();
            $('.seelct2Dropdown').val(0).trigger('change');
            $('.selectmultiple').val([]);
            $('.selectmultiple').multipleSelect('refresh');
        });
    }

    async function getDataofDashboardCards(error) {
        try {
            if (error === 'Unauthorized') {
                window.location.href = config.authScreenUrl;
                return;
            }

            const loadingIndicator = document.getElementById('loadingIndicator');
            if (loadingIndicator) {
                loadingIndicator.style.display = 'block';
            }

            const data = {
                getDataFunc: true,
                getPropertiesFunc: false,
                getContractsFunc: false,
                getIncvoiceFunc: false
            };

            const response = await fetch('/Home/SingleFucntionCall', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });

            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }

            if (!response.ok) {
                console.error('Error in response:', response.status);
            }
        } catch (error) {
            console.error('Error:', error);
            const loadingIndicator = document.getElementById('loadingIndicator');
            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }
        }
    }

    async function getPropertiesofDashboardCards(error) {
        try {
            if (error === 'Unauthorized') {
                window.location.href = config.authScreenUrl;
                return;
            }

            const loadingIndicator = document.getElementById('loadingIndicator');
            if (loadingIndicator) {
                loadingIndicator.style.display = 'block';
            }

            const data = {
                getDataFunc: false,
                getPropertiesFunc: true,
                getContractsFunc: false,
                getIncvoiceFunc: false
            };

            const response = await fetch('/Home/SingleFucntionCall', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });

            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }

            if (!response.ok) {
                console.error('Error in response:', response.status);
            }
        } catch (error) {
            console.error('Error:', error);
            const loadingIndicator = document.getElementById('loadingIndicator');
            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }
        }
    }

    function getContractsofDashboardCards() {
        if (config.mainDashboardUrl) {
            window.location.href = config.mainDashboardUrl;
        }
    }

    async function getInvoicesofDashboardCards(error) {
        try {
            if (error === 'Unauthorized') {
                window.location.href = config.authScreenUrl;
                return;
            }

            const loadingIndicator = document.getElementById('loadingIndicator');
            if (loadingIndicator) {
                loadingIndicator.style.display = 'block';
            }

            const data = {
                getDataFunc: false,
                getPropertiesFunc: false,
                getContractsFunc: false,
                getIncvoiceFunc: true
            };

            const response = await fetch('/Home/SingleFucntionCall', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });

            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }

            if (!response.ok) {
                console.error('Error in response:', response.status);
            }
        } catch (error) {
            console.error('Error:', error);
            const loadingIndicator = document.getElementById('loadingIndicator');
            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }
        }
    }

    function init() {
        initNotificationModal();
        bindNotificationControls();
        initDonutChart();
        initResetButton();
    }

    document.addEventListener('DOMContentLoaded', init);
    window.markAsReadFunction = markAsReadFunction;
})();

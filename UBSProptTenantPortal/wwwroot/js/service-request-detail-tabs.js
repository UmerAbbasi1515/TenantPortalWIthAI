// ===============================
// SERVICE REQUEST DETAIL - TAB MANAGEMENT
// ===============================

var documentsLoaded = false;
var communicationsLoaded = false;
var communicationRefreshInterval;

// caseNo should be available globally from Razor
// var caseNo = '@ViewData["ServiceReqNoTitle"]';

// ===============================
// LOADER HELPERS
// ===============================
function showLoader() {
    const loader = document.getElementById("loader");
    if (loader) loader.style.display = "block";
}

function hideLoader() {
    const loader = document.getElementById("loader");
    if (loader) loader.style.display = "none";
}

// ===============================
// TOAST / MESSAGE HANDLER
// ===============================
function showMsg(msg) {
    alert(msg); // replace with toastr if needed
}

// ===============================
// TAB MANAGEMENT
// ===============================
function showTab(tabId) {

    document.querySelectorAll('.tab-content').forEach(tab => {
        tab.style.display = 'none';
        tab.classList.remove('active');
    });

    const selectedTab = document.getElementById(tabId);
    if (selectedTab) {
        selectedTab.style.display = 'block';
        selectedTab.classList.add('active');
    }

    document.querySelectorAll('.nav-link').forEach(link => {
        link.classList.remove('active');
    });
}

// ===============================
// MAIN TAB
// ===============================
function loadMainInfo() {
    showLoader();
    showTab('mainInfo');

    document.getElementById('MainInfo-tab')?.classList.add('active');

    stopCommunicationRefresh();
    hideLoader();
}

// ===============================
// UNIT INFO
// ===============================
async function fetchUnitInfo(contractID) {

    const loader = document.getElementById('unitInfoloadingIndicator');
    const container = document.getElementById('unitInfoList');

    showMsg("Loading Unit Info for Contract: " + contractID);
    loader.style.display = 'block';

    try {
        const response = await fetch('/Contracts/GetContractUnites', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ ContractId: contractID })
        });

        if (!response.ok) throw new Error("Unit API failed");

        const html = await response.text();
        container.innerHTML = html;

    } catch (err) {
        console.error(err);
        showMsg("Error loading unit info");
    } finally {
        loader.style.display = 'none';
    }
}

// ===============================
// PAYMENT INFO
// ===============================
async function fetchPaymentInfo(contractID) {

    const loader = document.getElementById('paymentInfoloadingIndicator');
    const container = document.getElementById('paymentInfo');

    showMsg("Loading Payment Info...");
    loader.style.display = 'block';

    try {
        const response = await fetch('/Contracts/GetContractPayments', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ ContractId: contractID })
        });

        if (!response.ok) throw new Error("Payment API failed");

        const html = await response.text();
        container.innerHTML = html;

    } catch (err) {
        console.error(err);
        showMsg("Error loading payment info");
    } finally {
        loader.style.display = 'none';
    }
}

// ===============================
// CHEQUE INFO
// ===============================
async function fetchPaymentChequesInfo(transactionID) {

    const loader = document.getElementById('loadingIndicator');
    const container = document.getElementById('chequeInfo');

    showMsg("Cheque Transaction ID: " + transactionID);
    loader.style.display = 'block';

    try {
        const response = await fetch('/Contracts/GetContractPaymentsCheques', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ TransactionID: transactionID })
        });

        if (!response.ok) throw new Error("Cheque API failed");

        const html = await response.text();
        container.innerHTML = html;

    } catch (err) {
        console.error(err);
        showMsg("Error loading cheque info");
    } finally {
        loader.style.display = 'none';
    }
}

// ===============================
// CHARGES INFO
// ===============================
async function fetchChargesInfo(contractID) {

    const loader = document.getElementById('chargesInfoloadingIndicator');
    const container = document.getElementById('chargesInfo');

    showMsg("Loading Charges Info...");
    loader.style.display = 'block';

    try {
        const response = await fetch('/Contracts/GetContractCharges', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ ContractId: contractID })
        });

        if (!response.ok) throw new Error("Charges API failed");

        const html = await response.text();
        container.innerHTML = html;

    } catch (err) {
        console.error(err);
        showMsg("Error loading charges info");
    } finally {
        loader.style.display = 'none';
    }
}

// ===============================
// UNIT DETAIL NAVIGATION
// ===============================
function unitDetail(unitID, unitNo) {

    var pageName = "ContractDetail";

    var url = '@Url.Action("GetPropertyUnitDetailNew", "Properties")'
        + '?unitIDParam=' + encodeURIComponent(unitID)
        + '&pageName=' + pageName;

    window.location.href = url;
}

// ===============================
// COMMUNICATIONS TAB
// ===============================
async function loadCommunications() {
    showLoader();
    showTab('paymentInfo');

    document.getElementById('Payments-tab')?.classList.add('active');

    await refreshCommunications();
    startCommunicationRefresh();

    hideLoader();
}

// ===============================
// REFRESH CHAT
// ===============================
async function refreshCommunications() {
    try {
        const response = await fetch(
            '/ServiceRequests/ServiceRequest/_ServiceRequestCommunications?caseNo=' + caseNo
        );

        if (!response.ok) return;

        const html = await response.text();

        const temp = document.createElement("div");
        temp.innerHTML = html;

        const newChat = temp.querySelector("#ChatContainer");
        const oldChat = document.getElementById("ChatContainer");

        if (!newChat || !oldChat) return;

        const isNearBottom =
            oldChat.scrollTop + oldChat.clientHeight >= oldChat.scrollHeight - 50;

        oldChat.innerHTML = newChat.innerHTML;

        if (!communicationsLoaded || isNearBottom) {
            oldChat.scrollTop = oldChat.scrollHeight;
        }

        communicationsLoaded = true;

    } catch (err) {
        console.error("Chat refresh error:", err);
    }
}

// ===============================
// AUTO REFRESH CONTROL
// ===============================
function startCommunicationRefresh() {
    stopCommunicationRefresh();

    communicationRefreshInterval = setInterval(() => {
        refreshCommunications();
    }, 2000);
}

function stopCommunicationRefresh() {
    if (communicationRefreshInterval) {
        clearInterval(communicationRefreshInterval);
        communicationRefreshInterval = null;
    }
}

// ===============================
// SAFE CLEANUP EVENTS
// ===============================
window.addEventListener('beforeunload', function () {
    stopCommunicationRefresh();
});

document.addEventListener('visibilitychange', function () {
    if (document.hidden) {
        stopCommunicationRefresh();
    }
});
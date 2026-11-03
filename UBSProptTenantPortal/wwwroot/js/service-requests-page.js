// ServiceRequests Page Scripts

$(document).ready(function () {
    // Handle tab clicks
    $("#fm-tab").on("click", function () {
        $("#FromDate, #ToDate").val('');
        $("#FMRequestID").show();
        $("#PMRequestID").hide();
    });

    $("#pm-tab").on("click", function () {
        $("#FromDate, #ToDate").val('');
        $("#FMRequestID").hide();
        $("#PMRequestID").show();
    });

    function formatToMMDDYYYY(dateStr) {
        if (!dateStr) return "";
        const parts = dateStr.split('/'); // dd-mm-yyyy
        if (parts.length !== 3) return "";
        return `${parts[1]}-${parts[0]}-${parts[2]}`; // mm-dd-yyyy
    }

    // Search Button
    $("#btn-Search").on("click", function (e) {
        showLoader();
        debugger;
        e.preventDefault();
        let fromDate = $("#FromDate").val();
        let toDate = $("#ToDate").val();
        let activeTab = $(".nav-tabs .nav-link.active").attr("id");
        let type = activeTab.includes("fm-tab") ? "FM" : "PM";

        if (!fromDate && !toDate) {
            toastr.error("Please select dates");
            hideLoader();
            return;
        }
        let fromDateSend = formatToMMDDYYYY(fromDate);
        let toDateSend = formatToMMDDYYYY(toDate);

        $.ajax({
            url: '/ServiceRequests/ServiceRequest/ServiceRequestsFilter',
            type: 'POST',
            data: { fromDate: fromDateSend, toDate: toDateSend, type: type },
            success: function (result) {
                hideLoader();
                debugger;
                if (type == "FM") {
                    $("#fm").html(result);
                }
                if (type == "PM") {
                    $("#pm").html(result);
                }
            },
            error: function (xhr, status, error) {
                hideLoader();
                toastr.error("Something went wrong");
                console.error(xhr.responseText);
            }
        });
    });

    // Reset Button
    $("#resetButton").on("click", function (e) {
        debugger;
        showLoader();
        e.preventDefault();
        $("#FromDate, #ToDate").val('');

        // Get the currently active tab
        var activeTab = '';
        if ($('#fm-tab').hasClass('active')) {
            activeTab = 'FM';
        } else if ($('#pm-tab').hasClass('active')) {
            activeTab = 'PM';
        }

        // Redirect with the active tab parameter
        window.location.href = '@Url.Action("ServiceRequests", "ServiceRequest", new { area = "ServiceRequests" })' +
            (activeTab ? '?tab=' + activeTab : '?tab=FM');
    });
});

$('#FromDate').on('change', function () {
    debugger;
    fromDateErrorID.style.display = "none";
    validateDates();
});

$('#ToDate').on('change', function () {
    debugger;
    toDateErrorID.style.display = "none";
    validateDates();
});

function validateDates() {
    debugger;
    const fromDate = $('#FromDate').val();
    const toDate = $('#ToDate').val();
    if (fromDate && toDate) {
        const from = new Date(fromDate.split('/').reverse().join('/'));
        const to = new Date(toDate.split('/').reverse().join('/'));
        if (to <= from) {
            toastr.error('Error', "Please select valid date");
            $('#ToDate').val(''); // Clear the To Date field if the validation fails
        }
    }
}

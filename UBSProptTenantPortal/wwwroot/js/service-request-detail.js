// Service Request Detail - General Functions

// Initialize popup handlers
document.addEventListener('DOMContentLoaded', function () {
    window.showPopup = function () {
        document.getElementById('cancelModal').style.display = 'block';
        vendorSignatureCapture();
    };

    window.closePopup = function () {
        document.getElementById('cancelModal').style.display = 'none';
    };
});

// Cancel Service Request Detail
async function CancelServiceRequestDetail() {
    showLoader();
    const formData = new FormData();
    formData.append("caseNo", caseNo1);
    debugger;
    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/CancelServiceRequestDetail', {
            method: 'POST',
            body: formData
        });
        debugger;
        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                document.getElementById('cancelModal').style.display = 'none';
                toastr.success("Cancelled Successfully");
            } else {
                document.getElementById('cancelModal').style.display = 'none';
                toastr.error("Cancelled failed: " + result.message);
            }
        } else {
            const errorText = await response.text();
            toastr.error("Server error: " + errorText);
        }
    } catch (error) {
        console.error('Error cancelling request:', error);
    } finally {
        window.location.reload();
        hideLoader();
    }
}

// Payment Tab Handler
$('#paymentTab').on('click', function () {
    $('#paymentTabContainer').load('/YourPage?handler=PaymentInfo&serviceRequestNo=123');
});

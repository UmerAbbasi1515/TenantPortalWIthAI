// Service Request Detail - Photo Management

async function uploadPhoto(documentTypeID, attachementType) {
    debugger;
    showLoader();
    const fileInput = document.getElementById(`photoInput`);
    const expiryDate = "";
    debugger;
    if (!fileInput.files.length) {
        alert("Please select a photo.");
        hideLoader();
        return;
    }

    const file = fileInput.files[0];
    const formData = new FormData();
    formData.append("caseNo", caseNo1);
    formData.append("expiry", expiryDate);
    formData.append("attachementType", attachementType);
    formData.append("documentTypeId", documentTypeID);
    formData.append("file", file);

    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/ServiceRequestUploadReportDoc', {
            method: 'POST',
            body: formData
        });
        debugger;
        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                toastr.success("Photo Uploaded successfully");
            } else {
                toastr.error("Upload failed: " + result.message);
            }
        } else {
            const errorText = await response.text();
            toastr.error("Server error: " + errorText);
        }
    } catch (error) {
        toastr.error("Network error: " + error);
    } finally {
        window.location.reload();
        hideLoader();
    }
}

function openImageInNewTab(imageSrc) {
    window.open(imageSrc, '_blank');
}

window.showPopupDelete = function (photoId) {
    debugger;
    console.log("Photo ID:", photoId);
    document.getElementById('photoID').value = photoId;
    document.getElementById('deleteModal').style.display = 'block';
};

window.closePopupDelete = function () {
    document.getElementById('deleteModal').style.display = 'none';
};

// Delete photo with confirmation
async function deletePhoto() {
    debugger;
    showLoader();

    var photoID = document.getElementById('photoID').value;
    const formData = new FormData();
    formData.append("PhotoId", photoID);

    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/ServiceRequestDeleteReportDoc', {
            method: 'POST',
            body: formData
        });
        debugger;
        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                toastr.success("Photo deleted successful");
                closePopupDelete();
            } else {
                toastr.error("Photo deleted failed: " + result.message);
                closePopupDelete();
            }
        } else {
            const errorText = await response.text();
            toastr.error("Server error: " + errorText);
        }
    } catch (error) {
        toastr.error("Server error: " + error);
    } finally {
        window.location.reload();
        hideLoader();
    }
}

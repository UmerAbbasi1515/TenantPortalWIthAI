// UploadPhoto Page Scripts

function showLoader() {
    const loader = document.getElementById('fullPageLoader');
    if (loader) loader.style.display = 'flex';
}

function hideLoader() {
    const loader = document.getElementById('fullPageLoader');
    if (loader) loader.style.display = 'none';
}

async function uploadPhoto(documentTypeID, attachementType, caseNo) {
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
    formData.append("caseNo", caseNo);
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

async function downloadDocument(attachmentId, reqNo, extentsion, index) {
    debugger;
    showLoader();
    var id = "btnDownload_" + index;
    if (reqNo == "") {
        reqNo = caseNo1;
    }
    btnshowLoader(id);
    debugger;
    const data = {
        CaseNo: reqNo.toString(),
        RoleId: "1",
        PhotoId: attachmentId.toString()
    };
    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/ServiceRequestDownloadDocuments', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        debugger;
        if (response.ok) {
            btnhideLoader(id);
            const data1 = await response.json();
            if (data1.path) {
                if (extentsion == "png") {
                    downloadImage(data1.path);
                } else if (extentsion == "jpg") {
                    downloadImage(data1.path);
                } else if (extentsion == "jpeg") {
                    downloadImage(data1.path);
                } else {
                    downloadPdf(data1.path);
                }
            } else {
                toastr.error("Download failed: base64 path not found");
            }
        } else {
            btnhideLoader(id);
            toastr.error("Download failed: " + response.message);
        }
    } catch (error) {
        btnhideLoader(id);
        toastr.error("Download failed: " + error);
    } finally {
        hideLoader();
    }
}

function downloadPdf(base64StringFromAPI) {
    const base64String = base64StringFromAPI;
    const byteCharacters = atob(base64String);
    const byteNumbers = Array.from(byteCharacters, c => c.charCodeAt(0));
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: 'application/pdf' });
    
    const blobUrl = URL.createObjectURL(blob);
    const newTab = window.open(blobUrl, "_blank");
    
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = 'file.pdf';
    link.click();

    URL.revokeObjectURL(link.href);
}

function downloadImage(base64StringFromAPI) {
    debugger;
    const base64String = base64StringFromAPI;
    const byteCharacters = atob(base64String);
    const byteNumbers = Array.from(byteCharacters, c => c.charCodeAt(0));
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: 'image/jpeg' });

    const blobUrl = URL.createObjectURL(blob);
    const newTab = window.open(blobUrl, "_blank");

    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = 'file.jpg';
    link.click();

    URL.revokeObjectURL(link.href);

    setTimeout(() => {
        URL.revokeObjectURL(blobUrl);
    }, 3000);
}

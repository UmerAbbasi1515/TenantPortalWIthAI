// Service Request Detail - Document Management

// Open Upload Modal
function openUploadModal(index, reqNo, docTypeId, id, docName, callFrom, isRejected) {
    debugger;
    document.getElementById("modalTitle").innerText = callFrom + " — " + docName;
    document.getElementById("docNameLabel").innerText = docName;
    document.getElementById("uploadReqNo").value = reqNo;
    document.getElementById("uploadDocTypeId").value = docTypeId;
    document.getElementById("uploadEIDDocTypeId").value = id;
    document.getElementById("isRejected").value = isRejected;
    document.getElementById("IDD").value = id;

    document.getElementById("UploadFile").value = "";
    document.getElementById("expiryDateInput").value = "";
    document.getElementById('uploadBtn').innerText = callFrom;
    document.getElementById("uploadBtn").disabled = true;

    $('#uploadModel_').modal('show');
}

// Open Emirates ID Sample Modal
function openEMirateIDSample(index, reqNo, docTypeId, id, docName, callFrom, isRejected) {
    debugger;
    document.getElementById("modalEIDTitle").innerText = callFrom + " — " + docName;
    document.getElementById("docEIDNameLabel").innerText = docName;
    document.getElementById("docEIDNameLabel1").innerText = docName;
    document.getElementById("uploadEIDReqNo").value = reqNo;
    document.getElementById("uploadEIDDocTypeId").value = docTypeId;
    document.getElementById("IDD").value = id;
    document.getElementById("isRejectedEID").value = isRejected;

    document.getElementById("UploadEIDFrontFile").value = "";
    document.getElementById("UploadEIDBackFile").value = "";
    document.getElementById("expiryEIDDateInput").value = "";
    document.getElementById('uploadEIDBtn').innerText = callFrom;
    $('#emirateIDSampleModal').modal('show');
}

// Open Upload EID Modal
function openUploadEIDModal() {
    document.getElementById("nextEIDBtn").disabled = "";
    document.getElementById("expiryEIDDateInput").value = "";
    document.getElementById("IDNumber").value = "";
    document.getElementById("Nationality").innerText = "";
    document.getElementById("Name").value = "";
    document.getElementById("Name").innerText = "";
    document.getElementById("DOBDate").innerText = "";
    debugger;
    $('#emirateIDSampleModal').modal('hide');
    $('#uploadModelEID_').modal('show');
}

// Open Confirmation EID Modal
function openConfirmationEIDModal() {
    debugger;
    mergeFiles();
    $('#uploadModelEID_').modal('hide');
    $('#confirmationModelEID_').modal('show');
}

// Merge EID Front and Back Files
function mergeFiles() {
    const frontFile = document.getElementById("UploadEIDFrontFile").files[0];
    const backFile = document.getElementById("UploadEIDBackFile").files[0];

    if (!frontFile || !backFile) return;

    const frontImg = new Image();
    const backImg = new Image();

    frontImg.onload = () => {
        backImg.onload = () => {
            const canvas = document.getElementById("mergedCanvas");
            canvas.width = Math.max(frontImg.width, backImg.width);
            canvas.height = frontImg.height + backImg.height;

            const ctx = canvas.getContext("2d");
            ctx.drawImage(frontImg, 0, 0);
            ctx.drawImage(backImg, 0, frontImg.height);

            document.getElementById("mergedImage").src = canvas.toDataURL("image/png");
        };
        backImg.src = URL.createObjectURL(backFile);
    };
    frontImg.src = URL.createObjectURL(frontFile);
}

// Clear Expiry Date
function clearExpiryDate() {
    debugger;
    document.getElementById("expiryDateInput").value = "";
    document.getElementById("uploadBtn").disabled = true;
}

// Submit EID Upload
async function submitUploadEID() {
    btnshowLoader("uploadEIDBtn");
    debugger;
    const canvas = document.getElementById("mergedCanvas");
    let base64 = canvas.toDataURL("image/png");

    let file = base64ToFile(base64, "signature.png");

    if (file.size > 10 * 1024 * 1024) {
        toastr.error("File size must be 10 MB or less. Please upload again.");
        return;
    }

    let reqNo = document.getElementById("uploadEIDReqNo").value;
    let expiryDate = document.getElementById("ExpiryDateConfirmation").value;
    let issuingDate = document.getElementById("IssuingDate").value;
    let docName = document.getElementById("docEIDNameLabel").innerText;
    let docTypeId = document.getElementById("uploadEIDDocTypeId").value;
    let emirateIdNumber = document.getElementById("IDNumber").value;
    let nationality = document.getElementById("Nationality").value;
    let nameEng = document.getElementById("Name").value;
    let nameAr = document.getElementById("Name").value;
    let dOB = document.getElementById("DOBDate").value;
    let idd = document.getElementById("IDD").value;

    if (!expiryDate || expiryDate.trim() === "") {
        toastr.error("Expiry Date is required!");
        btnhideLoader("uploadEIDBtn");
        return;
    }

    if (!docName || docName.trim() === "") {
        toastr.error("Document Name is required!");
        btnhideLoader("uploadEIDBtn");
        return;
    }

    if (!emirateIdNumber || emirateIdNumber.trim() === "") {
        toastr.error("Emirates ID Number is required!");
        btnhideLoader("uploadEIDBtn");
        return;
    }

    if (!nationality || nationality.trim() === "") {
        toastr.error("Nationality is required!");
        btnhideLoader("uploadEIDBtn");
        return;
    }

    if (!nameEng || nameEng.trim() === "") {
        toastr.error("Name (English) is required!");
        btnhideLoader("uploadEIDBtn");
        return;
    }

    if (!nameAr || nameAr.trim() === "") {
        toastr.error("Name (Arabic) is required!");
        btnhideLoader("uploadEIDBtn");
        return;
    }

    if (!dOB || dOB.trim() === "") {
        toastr.error("Date of Birth is required!");
        btnhideLoader("uploadEIDBtn");
        return;
    }

    let callingFrom = document.getElementById("modalEIDTitle").innerText;
    var isRejected = document.getElementById("isRejectedEID").value;
    if (callingFrom.toLowerCase().includes('replace')) {
        await updateDocument(expiryDate, idd, file, reqNo);
    }
    else if (isRejected == "true") {
        await updateDocument(expiryDate, idd, file, reqNo);
    }
    else {
        await uploadEIDDocument(caseNo1, expiryDate, docName, docTypeId, file, emirateIdNumber, nationality, nameEng, nameAr, dOB);
    }

    btnhideLoader("uploadEIDBtn");
}

// Upload EID Document
async function uploadEIDDocument(reqNo, expiryDate, docName, docTypeId, file, emirateIdNumber, nationality, nameEng, nameAr, dOB) {
    debugger;
    showLoader();
    let formData = new FormData();
    formData.append("caseNo", reqNo);
    formData.append("expiry", expiryDate);
    formData.append("attachementType", docName);
    formData.append("documentTypeId", docTypeId);
    formData.append("file", file);
    formData.append("emirateIdNumber", emirateIdNumber);
    formData.append("nationality", nationality);
    formData.append("nameEng", docName);
    formData.append("nameAr", docName);
    formData.append("dOB", dOB);

    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/ServiceRequestUploadEIDDoc', {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                toastr.success("Document Uploaded successfully");
                await reloadTab2(reqNo);
                $('#emirateIDSampleModal').modal('hide');
                $('#uploadModelEID_').modal('hide');
                $('#confirmationModelEID_').modal('hide');
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
        btnhideLoader("uploadBtn");
        hideLoader();
    }
}

// Update Document
async function updateDocument(expiryDate, attachementId, file, reqNo) {
    debugger;
    showLoader();

    let formData = new FormData();
    formData.append("expiry", expiryDate);
    formData.append("attachementId", attachementId);
    formData.append("file", file);

    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/ServiceRequestUpdateDocument', {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                toastr.success("Document Updated successfully");
                await reloadTab2(reqNo);
                $('#uploadModel_').modal('hide');
                $('#confirmationModelEID_').modal('hide');
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
        hideLoader();
    }
}

// Convert Base64 to File
function base64ToFile(base64, fileName) {
    let arr = base64.split(',');
    let mime = arr[0].match(/:(.*?);/)[1];
    let bstr = atob(arr[1]);
    let n = bstr.length;
    let u8arr = new Uint8Array(n);

    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }

    return new File([u8arr], fileName, { type: mime });
}

// Submit Regular Document Upload
function submitUpload() {
    showLoader();
    debugger;
    let fileInput = document.getElementById("UploadFile");
    let file = fileInput.files[0];

    if (file.size > 10 * 1024 * 1024) {
        toastr.error("File size must be 10 MB or less. Please upload again.");
        fileInput.value = "";
        hideLoader();
        return;
    }

    let reqNo = document.getElementById("uploadReqNo").value;
    let docTypeId = document.getElementById("uploadDocTypeId").value;
    let docName = document.getElementById("docNameLabel").innerText;
    let expiryDate = document.getElementById("expiryDateInput").value;
    let callingFrom = document.getElementById("modalTitle").innerText;
    let idd = document.getElementById("IDD").value;

    var isRejected = document.getElementById("isRejected").value;
    if (callingFrom.toLowerCase().includes('replace')) {
        updateDocument(expiryDate, idd, file, reqNo);
    } else if (isRejected == "true") {
        updateDocument(expiryDate, idd, file, reqNo);
    } else {
        uploadDocument(caseNo1, expiryDate, docName, docTypeId, file);
    }
}

// Upload Regular Document
async function uploadDocument(reqNo, expiryDate, docName, docTypeId, file) {
    debugger;
    showLoader();
    let formData = new FormData();
    formData.append("caseNo", reqNo);
    formData.append("expiry", expiryDate);
    formData.append("attachementType", docName);
    formData.append("documentTypeId", docTypeId);
    formData.append("file", file);

    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/ServiceRequestUploadReportDoc', {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                toastr.success("Document Uploaded successfully");
                await reloadTab2(reqNo);
                $('#uploadModel_').modal('hide');
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
        btnhideLoader("uploadBtn");
        hideLoader();
    }
}

// Show Confirmation Model
function showConfirmationModel() {
    $('#stageConfirmationModal').modal('show');
}

// Cancel Confirmation Model
function cancelConfirmationModel() {
    $('#stageConfirmationModal').modal('hide');
}

// Close Confirmation Model
function closeConfirmationModel() {
    $('#stageConfirmationModal').modal('hide');
    $('#stageConfirmationModal').on('hidden.bs.modal', function () {
        window.location.href = '/RenewalFlow/Renewal/RenewalContracts';
    });
}

// Update Document Stage
async function UpdateDocumentStage(dueActionID) {
    btnshowLoader("btn-updateDocStage");
    debugger;
    const data = {
        DueActionId: dueActionID.toString()
    };
    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/ServiceRequestUpdateDocumentsStage', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        debugger;
        if (response.ok) {
            btnhideLoader("btn-updateDocStage");
            const data1 = await response.json();
            if (data1.message == "Stage Updated Successfully") {
                toastr.success("Stage Updated Successfully");
                await reloadTab2(caseNo1);
                showConfirmationModel();
            } else {
                toastr.error("Stage Updated failed: " + data1.message);
            }
        } else {
            btnhideLoader("btn-updateDocStage");
            toastr.error("Stage Updated failed: " + response.message);
        }
    } catch (error) {
        btnhideLoader("btn-updateDocStage");
        toastr.error("Stage Updated failed: " + error);
    }
}

// Delete Document
function DeleteDocument(attachmentId, docName) {
    debugger;
    document.getElementById("uploadAttachmentId").value = attachmentId;
    document.getElementById("uploadeddocName").innerHTML = "<strong>" + docName + "</strong>";
    $('#confirmationModal').modal('show');
}

// Remove Document
async function removeDocument() {
    showLoader();
    btnshowLoader("btn-delete");
    debugger;
    let attachmentId = document.getElementById("uploadAttachmentId").value;
    const data = {
        PhotoId: attachmentId.toString()
    };
    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/ServiceRequestRemoveDocuments', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        debugger;
        if (response.ok) {
            btnhideLoader("btn-delete");
            const data1 = await response.json();
            if (data1.status == "file Deleted Successfully") {
                toastr.success("Deleted successfully");
                await reloadTab2(caseNo1);
                $('#confirmationModal').modal('hide');
            } else {
                toastr.error("Deleted failed: " + data1.status);
            }
        } else {
            btnhideLoader("btn-delete");
            toastr.error("Deleted failed: " + response.message);
        }
    } catch (error) {
        btnhideLoader("btn-delete");
        toastr.error("Deleted failed: " + error);
    } finally {
        hideLoader();
    }
}

// Download Document
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
            extentsion = extentsion.toLowerCase();
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

// Download PDF
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

// Download Image
function downloadImage(base64StringFromAPI) {
    debugger;
    const base64String = base64StringFromAPI;
    const byteCharacters = atob(base64String);
    const byteNumbers = Array.from(byteCharacters, c => c.charCodeAt(0));
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: 'image/png' });

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

// Reset Progress
function resetProgress(progressContainer, progressBar) {
    progressBar.style.width = "0%";
    progressBar.innerText = "0%";
    progressContainer.style.display = "none";
}

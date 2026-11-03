// Service Request Detail - Emirates ID Scanning & Parsing

let frontSideData = null;
let backSideData = null;

// Attach Events to File Inputs
document.addEventListener('DOMContentLoaded', function() {
    var frontInput = document.getElementById("UploadEIDFrontFile");
    var backInput = document.getElementById("UploadEIDBackFile");
    
    if (frontInput) {
        frontInput.addEventListener("change", function () {
            scanIDCard(this, "front");
        });
    }
    
    if (backInput) {
        backInput.addEventListener("change", function () {
            scanIDCard(this, "back");
        });
    }
});

// Main Scan Function (Tesseract)
async function scanIDCard(fileInput, side) {
    showLoaderEID();
    debugger;
    const file = fileInput.files[0];
    if (!file) {
        alert("Please select a file.");
        hideLoaderEID();
        return null;
    }

    console.log(`📄 Scanning ${side.toUpperCase()} side...`);

    try {
        const result = await Tesseract.recognize(file, 'eng', {
            tessedit_char_whitelist: '0123456789/-',
            logger: console.log,
            tessedit_pageseg_mode: Tesseract.PSM.SINGLE_LINE
        });

        const text = result.data.text;
        console.log("🔍 Extracted Text:", text);
        let parsedData;
        if (side == "front") {
            parsedData = await emirateIDParsingFrontSide(text);
        }
        else {
            parsedData = await emirateIDParsingBackSide(text);
        }

        // Check if user uploaded the wrong side
        if (parsedData && parsedData.detectedSide) {
            if (side === "front" && parsedData.detectedSide === "back") {
                toastr.warning("⚠️ It looks like you uploaded the BACK side instead of the FRONT side. Please upload the correct side.");
                fileInput.value = "";
                hideLoaderEID();
                return;
            }

            if (side === "back" && parsedData.detectedSide === "front") {
                toastr.warning("⚠️ It looks like you uploaded the FRONT side instead of the BACK side. Please upload the correct side.");
                fileInput.value = "";
                hideLoaderEID();
                return;
            }

            if (parsedData.detectedSide === "unknown") {
                toastr.warning("⚠️ Unable to detect Emirates ID information. Please ensure the image is clear and try again. Please upload the correct side");
                fileInput.value = "";
                hideLoaderEID();
                return;
            }
        }

        return null;
    } catch (err) {
        console.error("❌ OCR ERROR:", err);
        alert("Error while scanning: " + err.message);
        return null;
    } finally {
        hideLoaderEID();
    }
}

async function emirateIDParsingFrontSide(text) {
    debugger;
    showLoaderEID();
    const formData = new FormData();
    formData.append("text", text);
    debugger;
    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/ParseFront', {
            method: 'POST',
            body: formData
        });
        debugger;
        if (response.ok) {
            const result = await response.json();
            populateFields(result);
            return result;
        } else {
            const errorText = await response.text();
            return null;
        }
    } catch (error) {
        console.error('Parsing error:', error);
        return null;
    } finally {
        hideLoaderEID();
    }
}

async function emirateIDParsingBackSide(text) {
    showLoaderEID();
    debugger;
    const formData = new FormData();
    formData.append("text", text);
    debugger;
    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/ParseBack', {
            method: 'POST',
            body: formData
        });
        debugger;
        if (response.ok) {
            const result = await response.json();
            populateFields(result);
            return result;
        } else {
            const errorText = await response.text();
            return null;
        }
    } catch (error) {
        console.error('Parsing error:', error);
        return null;
    } finally {
        hideLoaderEID();
    }
}

// Push Parsed Values Into Form Fields
async function populateFields(data) {
    debugger;
    const map = {
        fullName: "Name",
        idNumber: "IDNumber",
        dateOfBirth: "DOBDate",
        expiryDateConfirmation: "ExpiryDateConfirmation",
        IssuingDate: "IssuingDate",
        expiryDate: "expiryEIDDateInput",
        nationality: "Nationality",
        sex: "Sex",
        cardNumber: "CardNumber"
    };

    Object.keys(map).forEach(k => {
        if (data[k] && document.getElementById(map[k])) {
            document.getElementById(map[k]).value = data[k];
        }
    });

    debugger;
    var frontImg = document.getElementById("UploadEIDFrontFile").value;
    var backImg = document.getElementById("UploadEIDBackFile").value;
    var expiry = document.getElementById("expiryEIDDateInput").value;

    const isValid = await validateDate(expiry);

    if (frontImg != "" && backImg != "" && isValid) {
        document.getElementById("nextEIDBtn").disabled = false;
    } else {
        document.getElementById("expiryEIDDateInput").value = "";
    }
}

// Validation function
async function validateDate(expiry) {
    if (!expiry) return false;

    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/ValidateDate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: `dateString=${encodeURIComponent(expiry)}`
        });

        const result = await response.json();

        if (result.isValid) {
            return true;
        } else {
            toastr.error(
                `The scanned expiry date (${expiry}) is invalid. Please select the expiry date manually to continue.`
            );
            return false;
        }
    } catch (error) {
        console.error('Validation error:', error);
        toastr.error('Failed to validate date');
        return false;
    }
}

// User select manually date than enable button
document.addEventListener('DOMContentLoaded', function() {
    var expiryEIDInput = document.getElementById("expiryEIDDateInput");
    var expiryInput = document.getElementById("expiryDateInput");
    
    if (expiryEIDInput) {
        expiryEIDInput.addEventListener("change", function () {
            debugger;
            var frontImg = document.getElementById("UploadEIDFrontFile").value;
            var backImg = document.getElementById("UploadEIDBackFile").value;

            if (frontImg == "" || backImg == "") {
                toastr.error("Please select file first");
                document.getElementById("expiryEIDDateInput").value = "";
                return;
            }

            if (this.value && frontImg != "" && backImg != "") {
                document.getElementById("nextEIDBtn").disabled = false;
            }
        });
    }
    
    if (expiryInput) {
        expiryInput.addEventListener("change", function () {
            debugger;
            var file = document.getElementById("UploadFile").value;
            if (file == "") {
                toastr.error("Please select file first");
                document.getElementById("expiryDateInput").value = "";
                return;
            }
            if (this.value && file != "") {
                document.getElementById("uploadBtn").disabled = false;
            }
        });
    }
});

// ID Number Auto-format
document.addEventListener('DOMContentLoaded', function() {
    var idInput = document.getElementById('IDNumber');
    if (idInput) {
        idInput.addEventListener('input', function () {
            let value = $(this).val().replace(/[^0-9]/g, '');

            if (value.length > 0) {
                let formatted = '';
                if (value.length <= 3) {
                    formatted = value;
                } else if (value.length <= 7) {
                    formatted = value.slice(0, 3) + '-' + value.slice(3);
                } else if (value.length <= 14) {
                    formatted = value.slice(0, 3) + '-' + value.slice(3, 7) + '-' + value.slice(7);
                } else {
                    formatted = value.slice(0, 3) + '-' + value.slice(3, 7) + '-' + value.slice(7, 14) + '-' + value.slice(14, 15);
                    value = value.slice(0, 15);
                }
                $(this).val(formatted);
            }
        });
    }
});

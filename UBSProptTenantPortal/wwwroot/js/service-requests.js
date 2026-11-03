(function () {
    // =======================
    // AddNewRequest Functions
    // =======================
    function initCategoryDropdown() {
        var categorySelect = document.getElementById('SelectedCategory');
        if (!categorySelect) return;

        categorySelect.addEventListener('change', function () {
            var value = this.value;
            var label = document.getElementById("categoryLabel");
            var contactTimeDiv = document.getElementById("contactTimeDiv");
            var labelText = document.getElementById("categoryType");
            var subcategory = document.getElementById("subcategoryDiv");

            var selectedOption = this.options[this.selectedIndex];
            var type = selectedOption.getAttribute('data-type');

            if (label) label.style.display = "block";
            if (labelText) labelText.textContent = type;

            if (type == "FM" && contactTimeDiv) {
                contactTimeDiv.style.display = "block";
            } else if (contactTimeDiv) {
                contactTimeDiv.style.display = "none";
            }

            if (value) {
                fetch('/ServiceRequests/ServiceRequest/ServiceRequestCaseSubCategoryFunc?catId=' + value)
                    .then(response => response.json())
                    .then(data => {
                        var subCategorySelect = document.getElementById('SelectedSubCategory');
                        if (subCategorySelect) {
                            subCategorySelect.innerHTML = '<option value="">-- Select Subcategory --</option>';
                            data.caseSubCategories.forEach(function (item) {
                                var option = document.createElement('option');
                                option.value = item.id;
                                option.textContent = item.name;
                                subCategorySelect.appendChild(option);
                            });
                            if (subcategory) subcategory.style.display = "block";
                        }
                    })
                    .catch(error => {
                        console.error('Error fetching subcategories:', error);
                        alert('Error fetching subcategories');
                    });
            }
        });
    }

    function initOtherTenantToggle() {
        var isOtherCheckbox = document.getElementById('IsOtherThanTenant');
        if (!isOtherCheckbox) return;

        isOtherCheckbox.addEventListener('change', function () {
            var otherTenantFields = document.getElementById('otherTenantFields');
            var otherName = document.getElementById('OtherName');
            var otherMobile = document.getElementById('OtherMobile');

            if (otherTenantFields) {
                otherTenantFields.style.display = this.checked ? 'block' : 'none';
            }
            if (otherName) otherName.required = this.checked;
            if (otherMobile) otherMobile.required = this.checked;
        });
    }

    function initPhoneNumberInput() {
        var otherMobile = document.getElementById('OtherMobile');
        if (!otherMobile) return;

        otherMobile.addEventListener('input', function () {
            this.value = this.value.replace(/[^0-9+]/g, '').replace(/^([^+]*\+)(?=.)/g, '$1');
            if (this.value.indexOf('+') > 0) {
                this.value = this.value.replace(/\+/g, '');
            }
        });
    }

    
    // =======================
    // ServiceRequests Functions
    // =======================
    function initServiceRequestsFilters() {
        var fromDateInput = document.getElementById('FromDate');
        var toDateInput = document.getElementById('ToDate');

        if (fromDateInput) {
            fromDateInput.addEventListener('change', validateDates);
        }
        if (toDateInput) {
            toDateInput.addEventListener('change', validateDates);
        }
    }

    function validateDates() {
        var fromDate = document.getElementById('FromDate');
        var toDate = document.getElementById('ToDate');

        if (!fromDate || !toDate) return;

        var from = fromDate.value;
        var to = toDate.value;

        if (from && to && new Date(from) > new Date(to)) {
            alert('From Date must be before To Date');
            toDate.value = '';
        }
    }

    function formatToMMDDYYYY(dateStr) {
        var date = new Date(dateStr);
        var month = String(date.getMonth() + 1).padStart(2, '0');
        var day = String(date.getDate()).padStart(2, '0');
        var year = date.getFullYear();
        return month + '/' + day + '/' + year;
    }

    // =======================
    // Common File Operations
    // =======================
    function downloadPdf(base64StringFromAPI) {
        var base64String = base64StringFromAPI;
        var byteCharacters = atob(base64String);
        var byteNumbers = Array.from(byteCharacters, function (c) { return c.charCodeAt(0); });
        var byteArray = new Uint8Array(byteNumbers);
        var blob = new Blob([byteArray], { type: 'application/pdf' });

        var link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = 'file.pdf';
        link.click();
        URL.revokeObjectURL(link.href);
    }

    function downloadImage(base64StringFromAPI) {
        var base64String = base64StringFromAPI;
        var byteCharacters = atob(base64String);
        var byteNumbers = Array.from(byteCharacters, function (c) { return c.charCodeAt(0); });
        var byteArray = new Uint8Array(byteNumbers);
        var blob = new Blob([byteArray], { type: 'image/jpeg' });

        var link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = 'image.jpg';
        link.click();
        URL.revokeObjectURL(link.href);
    }

    async function downloadFileGetTicket(ticketReplyId, index) {
        if (!ticketReplyId) return;

        try {
            var response = await fetch('/ServiceRequests/ServiceRequest/DownloadFileGetTicket', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ ticketReplyId: ticketReplyId.toString() })
            });

            if (response.ok) {
                var data = await response.json();
                if (data.file) {
                    downloadPdf(data.file);
                }
            }
        } catch (error) {
            console.error('Download error:', error);
        }
    }

    async function downloadDocument(attachmentId, reqNo, extension, index) {
        if (!attachmentId || !reqNo) return;

        try {
            var response = await fetch('/ServiceRequests/ServiceRequest/DownloadDocumentAttachmentGetCRM', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ attachmentId: attachmentId.toString(), reqNo: reqNo })
            });

            if (response.ok) {
                var data = await response.json();
                if (data.file) {
                    if (extension === 'pdf' || extension === '.pdf') {
                        downloadPdf(data.file);
                    } else {
                        downloadImage(data.file);
                    }
                }
            }
        } catch (error) {
            console.error('Download error:', error);
        }
    }

    // =======================
    // Tab Operations
    // =======================
    function openTab(evt, tabName, propertyID) {
        var i, tabcontent, tablinks;
        tabcontent = document.getElementsByClassName("tabcontent");
        for (i = 0; i < tabcontent.length; i++) {
            tabcontent[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablinks");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }

        var tab = document.getElementById(tabName);
        if (tab) {
            tab.style.display = "block";
        }
        if (evt && evt.currentTarget) {
            evt.currentTarget.className += " active";
        }
    }

    // =======================
    // Survey Functions
    // =======================
    function selectSurveyAnswer(answerId, optionID, qID, isCompleted) {
        document.querySelectorAll('.answer-option').forEach(function (option) {
            option.classList.remove('selected');
        });

        var selected = document.querySelector('[data-answer-id="' + answerId + '"]');
        if (selected) {
            selected.classList.add('selected');
        }
    }

    function nextSurveyQuestion() {
        var form = document.getElementById('surveyForm');
        if (form) {
            form.submit();
        }
    }

    function hideSurvey() {
        var surveyContainer = document.getElementById('surveyContainer');
        if (surveyContainer) {
            surveyContainer.style.display = 'none';
        }
    }

    // =======================
    // File Management
    // =======================
    function removeFile() {
        var fileInput = document.getElementById('fileInput');
        if (fileInput) {
            fileInput.value = '';
        }
    }

    function onFileSelected(event) {
        var uploadBtn = document.getElementById('uploadBtn');
        if (uploadBtn) {
            uploadBtn.disabled = !event.target.files.length;
        }
    }

    // =======================
    // Loader Functions
    // =======================
    function showLoader() {
        var loader = document.getElementById('fullPageLoader');
        if (loader) loader.style.display = 'flex';
    }

    function hideLoader() {
        var loader = document.getElementById('fullPageLoader');
        if (loader) loader.style.display = 'none';
    }

    // =======================
    // Expose to Global Scope
    // =======================
    window.initCategoryDropdown = initCategoryDropdown;
    window.initOtherTenantToggle = initOtherTenantToggle;
    window.initPhoneNumberInput = initPhoneNumberInput;
    window.initServiceRequestsFilters = initServiceRequestsFilters;
    window.validateDates = validateDates;
    window.formatToMMDDYYYY = formatToMMDDYYYY;
    window.downloadPdf = downloadPdf;
    window.downloadImage = downloadImage;
    window.downloadFileGetTicket = downloadFileGetTicket;
    window.downloadDocument = downloadDocument;
    window.openTab = openTab;
    window.selectSurveyAnswer = selectSurveyAnswer;
    window.nextSurveyQuestion = nextSurveyQuestion;
    window.hideSurvey = hideSurvey;
    window.removeFile = removeFile;
    window.onFileSelected = onFileSelected;
    window.showLoader = showLoader;
    window.hideLoader = hideLoader;

    document.addEventListener('DOMContentLoaded', function () {
        initCategoryDropdown();
        initOtherTenantToggle();
        initPhoneNumberInput();
        initServiceRequestsFilters();
    });
})();
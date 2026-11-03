// Service Request Detail - Survey Functions

async function loadSurvey() {
    showLoader();
    document.getElementById('surveyContainer').style.display = 'block';
    const formData = new FormData();
    formData.append("caseNo", caseNo1);
    debugger;
    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/LoadSurveyPartial', {
            method: 'POST',
            body: formData
        });

        // Read HTML from the partial
        const html = await response.text();
        // Insert into the page
        document.getElementById("surveyContainer").innerHTML = html;
        // Hide the Survey button after loading partial
        const surveyButton = document.querySelector("button[onclick='loadSurvey()']");
        if (surveyButton) {
            surveyButton.style.display = "none";
        }
    } catch (error) {
        console.error('Error loading survey:', error);
    } finally {
        hideLoader();
    }
}

function hideSurvey() {
    debugger;
    hideLoader();
    document.getElementById('surveyContainer').style.display = 'none';
    document.getElementById('surveybuttonId').style.display = 'block';
}

function selectSurveyAnswer(answerId, optionId, questionID, isCompleted) {
    debugger;
    showLoader();
    // Fill the hidden fields
    document.getElementById("answerId").value = answerId;
    document.getElementById("customerChoice").value = optionId;
    document.getElementById("descriptionInput").value = "desc";
    document.getElementById("questionIDInut").value = questionID;
    document.getElementById("isCompletedIDInut").value = isCompleted;
    if (questionID != -1) {
        document.getElementById("isLastQuestion").value = "0";
    } else {
        document.getElementById("isLastQuestion").value = "1";
    }
    var isLast = document.getElementById("isLastQuestion").value;
    console.log("Selected values:", { answerId, optionId, questionID, isCompleted });
    hideLoader();
}

async function nextSurveyQuestion() {
    debugger;
    showLoader();
    var answerId = document.getElementById("answerId").value;
    var customerChoice = document.getElementById("customerChoice").value;
    var description = document.getElementById("descriptionInput").value;
    var IsCompleted = document.getElementById("isCompletedIDInut").value;
    var QuestionId = document.getElementById("questionIDInut").value;

    var payload;
    if (answerId != '' && customerChoice != '' && IsCompleted != '' && QuestionId != '') {
        payload = {
            answerId: document.getElementById("answerId").value,
            customerChoice: document.getElementById("customerChoice").value,
            description: document.getElementById("descriptionInput").value,
            IsCompleted: document.getElementById("isCompletedIDInut").value,
            QuestionId: document.getElementById("questionIDInut").value,
        };
    } else {
        toastr.warning("Please choose correct one");
        hideLoader();
        return;
    }
    debugger;
    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/NextQuestion', {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(payload)
        });

        // Check if this was the last question
        var isLast = document.getElementById("isLastQuestion").value;
        if (isLast === "1") {
            const surveyModal = new bootstrap.Modal(document.getElementById('surveySuccessModal'));
            surveyModal.show();
            const html = await response.text();
            document.getElementById("surveyContainer").innerHTML = html;
        } else {
            const html = await response.text();
            document.getElementById("surveyContainer").innerHTML = html;
        }
    } catch (error) {
        console.error("Network error:", error);
    } finally {
        hideLoader();
    }
}

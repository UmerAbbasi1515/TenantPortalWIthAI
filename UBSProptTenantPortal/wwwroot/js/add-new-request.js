// AddNewRequest Page Scripts

async function SubmitRequest() {
    debugger;

    const categoryType = document.querySelector("#categoryType").innerText;
    const selectedCategory = document.getElementById("SelectedCategory").value;
    const selectedSubCategory = document.getElementById("SelectedSubCategory").value;
    const selectedProperty = document.getElementById("SelectedProperty").value;
    const isOtherThanTenant = document.getElementById("IsOtherThanTenant").checked;
    const otherName = document.getElementById("OtherName").value;
    const otherMobile = document.getElementById("OtherMobile").value;
    const contactTime = document.getElementById("SelectedContactTime").value;
    const description = document.getElementById("Description").value;

    clearErrorMessages(categoryDiv);
    clearErrorMessages(subcategoryDiv);
    clearErrorMessages(propertyDiv);
    clearErrorMessages(nameDiv);
    clearErrorMessages(mobileDiv);
    clearErrorMessages(contactTimeDiv);
    clearErrorMessages(descriptionDiv);

    let hasError = false;

    if (selectedCategory === "") {
        showErrorMessage(categoryDiv, "Please select category");
        hasError = true;
    }
    if (selectedSubCategory === "") {
        showErrorMessage(subcategoryDiv, "Please select sub category");
        hasError = true;
    }

    if (isOtherThanTenant) {
        if (otherName === "") {
            showErrorMessage(nameDiv, "Please enter name");
            hasError = true;
        }
        if (otherMobile === "") {
            showErrorMessage(mobileDiv, "Please enter phone number");
            hasError = true;
        }
    }

    if (categoryType === "FM" && contactTime === "") {
        showErrorMessage(contactTimeDiv, "Please select contact time");
        hasError = true;
    }

    if (description === "") {
        showErrorMessage(descriptionDiv, "Please enter description");
        hasError = true;
    }

    if (hasError) {
        return;
    }
    debugger;

    try {
        debugger;
        var data = {
            "CaseCategoryId": selectedCategory,
            "CaseSubCategoryId": selectedSubCategory,
            "ContractUnitId": selectedProperty,
            "OtherContactPersonName": otherName,
            "OtherContactPersonMobile": otherMobile,
            "ContactTimeId": contactTime,
            "Description": description,
        };
        btnshowLoader("submitID");
        const response = await fetch('/ServiceRequests/ServiceRequest/SubmitServiceRequest', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });
        debugger;
        if (response.ok) {
            btnhideLoader("submitID");
            const data1 = await response.json(); // ✅ parse JSON
            if (data1.status == null) {
                toastr.error("Something went wrong");
            } else {
                var caseNO = data1.addServiceRequest.caseNo;
                window.location.href = '/ServiceRequests/ServiceRequest/UploadPhoto?caseNo=' + caseNO;
            }
        } else {
            btnhideLoader("submitID");
            toastr.error("Error", response.message);
        }
    } catch (error) {
        btnhideLoader("submitID");
        toastr.error("Error", error);
    }
}

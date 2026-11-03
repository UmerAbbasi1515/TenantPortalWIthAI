

// Register custom validation
$.validator.addMethod('graceperioddates', function (value, element, params) {
    var isGracePeriodApplicable = $("input[name='" + params.comparisonProperty + "']").prop('checked');

    if (isGracePeriodApplicable) {
        return value !== '';
    }
    return true;
}, 'Start Date is required.');

$.validator.addMethod('rentpaidbyentityrequired', function (value, element, params) {
    // Get the value of the RentPaidBy radio button or select field
    var rentPaidBy = $("input[name='" + params.comparisonProperty + "']:checked").val(); // For radio buttons

    // If RentPaidBy is 2 or 3, then RentPaidByEntity must not be empty
    if (rentPaidBy == "2" || rentPaidBy == "3") {
        // Check if the select2 value is not empty
        var rentPaidByEntityValue = $(element).val();
        return rentPaidByEntityValue !== '' && rentPaidByEntityValue !== null; // Ensure it's not empty
    }

    return true; // No validation needed if RentPaidBy is not 2 or 3
}, 'Rent Paid By Entity is required');

// Custom jQuery validation method
$.validator.addMethod('occupantsharerequired', function (value, element, params) {
    // Get the value of the RentPaidBy radio button or select field
    var rentPaidBy = $("input[name='" + params.comparisonProperty + "']:checked").val(); // For radio buttons

    // If RentPaidBy is 3, then OccupantShare must not be empty or 0
    if (rentPaidBy == "3") {
        return value !== "" && value !== null && value !== "0";  // Ensure it's not empty or 0
    }

    // No validation needed if RentPaidBy is not 3
    return true;
}, "Occupant Share is required."); 


// Custom jQuery Validator method for "greaterthan"
$.validator.addMethod("greaterthan", function (value, element, params) {
    // Get the comparison element value
    var comparisonValue = $(params).val();

    // Parse the values and compare
    return parseFloat(value) > parseFloat(comparisonValue);
});

// Unobtrusive validation adapter to map 'greaterthan' to our custom jQuery validator
$.validator.unobtrusive.adapters.add("greaterthan", ["comparisonproperty"], function (options) {
    var element = $(options.form).find("#" + options.params.comparisonproperty);
    options.rules["greaterthan"] = element;
    options.messages["greaterthan"] = options.message;
});



// Custom jQuery Validator method for "Field One Value Must Belongs to Field Two Value"
//  example.. tenant Name autocomplete value should have the tenantId value.
$.validator.addMethod("tenantnamemusthavetenantid", function (value, element, params) {
    // Get the comparison element value
    var comparisonValue = $(params).val();
    debugger;
    return value && parseInt(comparisonValue) > 0;
});

// Unobtrusive validation adapter to map 'greaterthan' to our custom jQuery validator
$.validator.unobtrusive.adapters.add("tenantnamemusthavetenantid", ["comparisonproperty"], function (options) {
    var element = $(options.form).find("#" + options.params.comparisonproperty);
    options.rules["tenantnamemusthavetenantid"] = element;
    options.messages["tenantnamemusthavetenantid"] = options.message;
});



// Add a custom validation method to check if the list is not null or empty
$.validator.addMethod("notnullorempty", function (value, element, params) {
    // Check if the value is an array (like a multi-select dropdown)

    debugger;
    if (Array.isArray(value)) {
        return value.length > 0; // Valid if array has at least one item
    }

    // If it's a comma-separated string (in case of multi-select)
    if (typeof value === "string") {
        return value.trim().length > 0; // Valid if it's not an empty string
    }

    // If it's anything else, ensure it's not null or empty
    return value != null && value.length > 0;
});

// Unobtrusive validation adapter to map 'notnullorempty' to our custom jQuery validator
$.validator.unobtrusive.adapters.add("notnullorempty", [], function (options) {
    // Add the rule to the current field
    options.rules["notnullorempty"] = true;
    options.messages["notnullorempty"] = options.message;
});



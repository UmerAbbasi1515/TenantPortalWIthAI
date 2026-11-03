
var cityID = 0;
$row = "";
function Search(pageNumber) {
    $('#txt-pgno').val(pageNumber);
    $("#search").click();
}

function Next(pageNumber) {

    var currentPage = parseInt($('#txt-pgno').val());
    if (currentPage == 0) {
        currentPage = 1;
    }
    $('#txt-pgno').val(parseInt(pageNumber + 1));
    $("#search").click();
}

$("#search").click(function () {

    debugger;

    $('#search').html('<i class="fa fa-spinner fa-spin" style="font-size:24px"></i>Loading...')
   
});


function MyYesFunction() {
    DeleteCity();
}

$(document).ready(function () {
    var IsDeualf = $('#txt-defualt').val();
    if (IsDeualf == "True") {
        $('#txt-defualt').val("false");
        gridLoad();
    }
});

function MyNoFunction() {
}

function DeleteCity() {
    var id = $row.find("input.cityid").val();
    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: '/City/Delete',
        data: { CityID: id },
        success: function (response) {
            if (response == "Success") {
                $('#txt-pgno').val(0);
                toastr.success("Success", "record deleted successfully");
                setInterval(function () { gridLoad(); }, 4000);
            }
            else {
                toastr.error("Failed", "Error while deleting data");
            }
        },
        error: function () {
            alert("");
            toastr.error("Eror", "Error while deleting data");
        }
    });
}

function gridLoad() {
    $("#search").click()
}

$('.fa-trash-o').click(function () {
    $row = $(this).closest("tr");
    AsyncConfirmYesNo(
        "Delete Confirmation",
        "Are you sure to delete ?",
        MyYesFunction,
        MyNoFunction
    );
});

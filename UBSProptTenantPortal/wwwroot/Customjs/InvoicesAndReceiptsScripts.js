// Script code here

$LegalInvoiceID = 0;

$deletePermission = false;
$editPermission = false;

//$(document).ready(function () {
//    $('.selct2Dropdown_InvoicesAndReceipts').select2();
//    GetData_InvoicesAndReceipts();
//});

function GetData_InvoicesAndReceipts() {
    if ($('#ViewMode').val() != 'view') {
    }
    if ($('#hdn_viewMode').val() == 'view') {
        $("#dt_InvoicesAndReceipts tr th:last-child.action").remove();
        $('#btnCreate_Invoice').addClass('hide');
    } else {
        $('#btnCreate_Invoice').removeClass('hide');
    }
    var valdata = $("#form-search-InvoicesAndReceipts").serialize();
    $.ajax({
        url: "/Setup/InvoicesAndReceipts/Manage",
        type: "POST",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: valdata,
        async: true,
        beforeSend: function () {
            btnshowLoader("btnsearch_InvoicesAndReceipts");
        },
        success: function (data_InvoicesAndReceipts) {
            var list = data_InvoicesAndReceipts.InvoicesAndReceiptsLists;
            var TotalRecord_InvoicesAndReceipts = data_InvoicesAndReceipts.TotalRecord;
            $('#hiddenTotalRecord_InvoicesAndReceipts').val(TotalRecord_InvoicesAndReceipts);

            var grid = '';
            if (TotalRecord_InvoicesAndReceipts == "0") {
                grid += '<tr><td colspan="9" style="text-align:center"><strong>No Records Found.</strong><td></tr>';
            }
            else {
                var i = 0;
                $.each(list, function (d, value) {
                    i++;
                    $actionbutton = "";
                    var cssClass = i % 2 == 0 ? "odd" : "even";
                    grid += '<tr class=' + cssClass + '><td>' + value.SrNo + '</td>'

                        + '<td>' + value.InvoiceNo + '</td>'
                        + '<td>' + value.CaseNo + '</td>'
                        + '<td>' + value.InvoiceDate + '</td>'
                        + '<td>' + value.InvoiceAmount + '</td>'
                        + '<td>' + value.VATAmount + '</td>'
                        + '<td>' + value.NetAmount + '</td>'
                        + '<td>' + value.InvoiceDetail + '</td>'


                    $actionbutton += '<button class="btn m-1 btn-sm btn-primary btn-outline-primary padding-5" type="button" data-bs-toggle="tooltip" data-bs-original-title="Click to edit record" onclick="openInvoicesAndReceiptsCreateOrEditModal(' + value.LegalInvoiceID + ',0)"><i class="fa fa-pencil fa-fw font-size-15"></i></button>'
                    $actionbutton += '<button class="btn m-1 btn-sm btn-success btn-outline-danger padding-5" type="button" data-bs-toggle="tooltip" data-bs-original-title="Click to delete" onclick="return deleteRow_InvoicesAndReceipts(this,' + value.LegalInvoiceID + ')"><i class="fa fa-trash fa-fw font-size-15"></i></button>'
                    // if ($editPermission == "True") {
                    // }

                    // if (value.IsDeleteable == true && $deletePermission == "True") {
                    // }

                    //if ($('#ViewMode').val() != 'view') {
                    //}
                    //grid += "<td class=''>" + $actionbutton + "</td>";
                    //

                    let linksTd = `
                                    <div class="btn-group">
                                    <button class="btn btn-primary btn-outline-primary padding-5 dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                    <i class="fa fa-ellipsis-v fa-fw font-size-15"></i>
                                    </button>
                                        <ul class="dropdown-menu">`;

                    linksTd += '<li><a href="javascript:void(0)" onclick="openInvoicesAndReceiptsCreateOrEditModal(' + value.LegalInvoiceID + ')"><i class="fa fa-pencil font-size-15"></i> Edit</a></li>'
                    //linksTd += '<li><a href="javascript:void(0)" onclick="return deleteRow_InvoicesAndReceipts(this,' + value.LegalInvoiceID + ')"><i class="fa fa-trash font-size-15"></i> Delete</a></li>'


                    linksTd += '</ul></div>';

                    if ($('#hdn_viewMode').val() != 'view') {
                        grid += "<td class='action'>" + linksTd + "</td>";
                    }
                    grid += "</tr>";
                });
            }

            if (TotalRecord_InvoicesAndReceipts == "0") {
                $("#tbody_InvoicesAndReceipts").html(grid);
                btnhideLoader("btnsearch_InvoicesAndReceipts");
                return;
            }
            Pagination($('#hiddenTotalRecord_InvoicesAndReceipts').val(), $("#hidcurrentindexemp_InvoicesAndReceipts").val(), $("#hidemppagesize_InvoicesAndReceipts").val(), "#qemp", "qemp", 'InvoicesAndReceipts', 'InvoicesAndReceipts');
            $("#tbody_InvoicesAndReceipts").html(grid);
            $('#tbody_InvoicesAndReceipts').find('[data-bs-toggle="tooltip"]').tooltip();
        },
        error: function (xhr, textStatus) {
            btnhideLoader("btnsearch_InvoicesAndReceipts");
            if (xhr.status === 401) { window.location.reload(); }
            else {
                $.notify('please try again!', "error");

            }
        }
    });
}


function Search_InvoicesAndReceipts(pagenumber) {
    $("#hidcurrentindexemp_InvoicesAndReceipts").val(pagenumber);
    GetData_InvoicesAndReceipts();
}


function deleteRow_InvoicesAndReceipts(e, id) {
    $LegalInvoiceID = id;
    var name = $(e).closest('tr').find('td:eq(1)').text();
    AsyncConfirmYesNo(
        "Delete Confirmation",
        "Are you sure to delete " + name + "?",
        MyYesFunction_InvoicesAndReceipts,
        MyNoFunction_InvoicesAndReceipts
    );
}

function MyNoFunction_InvoicesAndReceipts() {
}

function MyYesFunction_InvoicesAndReceipts() {
    Delete_InvoicesAndReceipts($LegalInvoiceID);
}

function Delete_InvoicesAndReceipts(id) {
    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: '/Setup/InvoicesAndReceipts/Delete',
        data: { id: id },
        success: function (response) {
            if (response == "Success") {
                //$('#txt-pgno').val(0);
                toastr.success("Success", "Record deleted successfully");
                GetData_InvoicesAndReceipts();
            }
            else {
                toastr.error("Failed", "Error while deleting data");
            }
        },
        error: function () {
            toastr.error("Eror", "Error while deleting data");
        }
    });
}


function OnSuccess_InvoicesAndReceipts(response) {
    // if (isSaveAndBack)
    //     btnhideLoader("btn-save-InsuranceNBack");
    // else
    let isUpdate = false;
    if (parseInt($('#InsuranceID').val()) > 0)
        isUpdate = true;
    else
        isUpdate = false;


    btnhideLoader("btn-save-InvoicesAndReceipts");
    if (response == "Success") {
        $('#btn-reset-InvoicesAndReceipts').addClass('hide');
        if (isUpdate)
            toastr.success("Success", "Hearing has been updated successfully");
        else
            toastr.success("Success", "Hearing has been created successfully");
        $('#modalInvoiceAndReceipts').modal('hide');

        GetData_InvoicesAndReceipts();
    }
    else if (response == "Failed") {
        if (isUpdate)
            toastr.error("Failed", "Failed to update hearing");
        else
            toastr.error("Failed", "Failed to create hearing");
    }
    // else if (response == "Exist") {
    //     toastr.info("Exist", "Insurance with same title is already exists");

    // }
}
function OnBegin_InvoicesAndReceipts() {
    btnshowLoader("btn-save-InvoicesAndReceipts");
}
function OnFailure_InvoicesAndReceipts(response) {
    // if (isSaveAndBack)
    //     btnhideLoader("btn-save-InsuranceNBack");
    // else
    btnhideLoader("btn-save-InvoicesAndReceipts");
    toastr.error("Error", "Error occured while creating record");
    return;
}
function OnComplete_InvoicesAndReceipts() {
    // if (isSaveAndBack)
    //     btnhideLoader("btn-save-InsuranceNBack");
    // else
    btnhideLoader("btn-save-InvoicesAndReceipts");
    return;
}

function openInvoicesAndReceiptsCreateOrEditModal(Id, LegalCaseID) {
    let Url = '/Setup/InvoicesAndReceipts/InvoicesAndReceiptsAddEdit';

    if (Url) {
        $.get(Url, { id: Id, legalCaseID: LegalCaseID }, function (data) {
            $('#modalContentInvoiceAndReceipts').html(data);
            $(".select2ModelDropdown").select2({
                dropdownParent: $('#modalInvoiceAndReceipts')
            });
            // Rebind unobtrusive validation
            $.validator.unobtrusive.parse('.ModalForm');
            // Bind change event to trigger validation when the Select2 value changes
            $('.select2ModelDropdown').on('change', function () {
                $(this).valid(); // Trigger validation for the dropdown
            });
            //btnhideLoader("btnCreate");
            $('#modalInvoiceAndReceipts').modal('show');
        });
    }
}

function printInvoicesAndReceipts() {

    // hide for print the options
    $('#btnCreate_Invoice').hide();
    $('.btnPrint_InvoicesAndReceipts').hide();
    $('#div-pageinfo_InvoicesAndReceipts').hide();
    $('#div-pages_InvoicesAndReceipts').hide();
    $("#dt_InvoicesAndReceipts tr th:last-child.action").hide();
    $("#dt_InvoicesAndReceipts tr td:last-child.action").hide();
    print();

    // show
    $('#btnCreate_Invoice').show();
    $('.btnPrint_InvoicesAndReceipts').show();
    $('#div-pageinfo_InvoicesAndReceipts').show();
    $('#div-pages_InvoicesAndReceipts').show();
    $("#dt_InvoicesAndReceipts tr th:last-child.action").show();
    $("#dt_InvoicesAndReceipts tr td:last-child.action").show();
    //let Url = '/Setup/InvoicesAndReceipts/PrintManage?legalCaseId=' + legalcaseId;
    //// Create a new window
    //var printWindow = window.open(Url, '', 'height=600,width=800');
}

(function () {


window.btnshowLoader = function (btnId) {
    $htmlOld = "";
    $htmlOld = $("#" + btnId).html();
    $("#" + btnId).prop('disabled', true);
    $("#" + btnId).html('<i class="fa fa-spinner fa-spin"" style="font-size:20px"></i>Loading...');

    $("button").prop('disabled', true);
    $("a").prop('disabled', true);
}

window.btnhideLoader = function (btnId) {
    $("#" + btnId).html($htmlOld);
    $("#" + btnId).prop('disabled', false);
    $("button").prop('disabled', false);
    $("a").prop('disabled', false);
}
    function formatToMMDDYYYY(dateStr) {
        if (!dateStr) return "";
        const parts = dateStr.split('/'); // dd-mm-yyyy
        if (parts.length !== 3) return "";
        return `${parts[1]}-${parts[0]}-${parts[2]}`; // mm-dd-yyyy
    }

    function validateDates() {
        const fromDate = $('#FromDate').val();
        const toDate = $('#ToDate').val();

        if (fromDate && toDate) {
            const from = new Date(fromDate.split('/').reverse().join('/'));
            const to = new Date(toDate.split('/').reverse().join('/'));
            if (to <= from) {
                toastr.error("Please select a valid date range");
                $('#ToDate').val('');
            }
        }
    }

    function validateInvoiceDates(fromDateSelector, toDateSelector, fromDateErrorID, toDateErrorID) {
        const fromDate = $(fromDateSelector).val();
        const toDate = $(toDateSelector).val();
        let isValid = true;

        $('#' + fromDateErrorID).hide();
        $('#' + toDateErrorID).hide();

        if (!fromDate) {
            $('#' + fromDateErrorID).show();
            isValid = false;
        }
        if (!toDate) {
            $('#' + toDateErrorID).show();
            isValid = false;
        }
        return isValid;
    }

    function initContractsPage() {
        if ($('#form-search').length === 0) {
            return;
        }

        $('#resetButton').on('click', function (e) {
            btnshowLoader('resetButton');
            e.preventDefault();
            $('#PropertyName').val('');
            $('.seelct2Dropdown').val('').trigger('change');
            $('.fromDate').val('');
            $('.toDate').val('');
            window.location.href = '/Contracts/Contract/ContractsN';
            btnHideLoader('resetButtonh');
        });

        $('#btn-Search').on('click', function (e) {
            btnshowLoader('btn-Search');
            e.preventDefault();

            let fromDate = $('#FromDate').val();
            let toDate = $('#ToDate').val();
            let propertyName = $('#PropertyName').val();
            let propertyType = $('.seelct2Dropdown[name="Property"]').val();
            let contractStatus = $('.seelct2Dropdown[name="ContractStatus"]').val();

            if (fromDate === '' && toDate === '' && propertyName === '' && propertyType == null && contractStatus == null) {
                toastr.error('Please fill some fields');
                btnhideLoader('btn-Search');
                return;
            }

            let fromDateSend = formatToMMDDYYYY(fromDate);
            let toDateSend = formatToMMDDYYYY(toDate);

            $.ajax({
                url: '/Contracts/Contract/ContractsNFilter',
                type: 'POST',
                data: {
                    fromDate: fromDateSend,
                    toDate: toDateSend,
                    propertyName: propertyName,
                    propertyType: propertyType,
                    contractStatus: contractStatus
                },
                success: function (result) {
                    $('#ContractTableID').html(result);
                },
                error: function (xhr, status, error) {
                    toastr.error('Something went wrong while fetching contracts');
                    console.error('AJAX Error:', error);
                },
                complete: function () {
                    btnhideLoader('btn-Search');
                }
            });
        });

        $('#FromDate, #ToDate').on('change', function () {
            $('#fromDateErrorID').hide();
            $('#toDateErrorID').hide();
            validateDates();
        });
    }

    window.openTab = function (evt, tabName, propertyID) {
        var tabContents = document.getElementsByClassName('tab-content');
        for (var i = 0; i < tabContents.length; i++) {
            tabContents[i].classList.remove('active');
        }

        var tabs = document.getElementsByClassName('tab');
        for (var j = 0; j < tabs.length; j++) {
            tabs[j].classList.remove('active');
        }

        if (document.getElementById(tabName)) {
            document.getElementById(tabName).classList.add('active');
        }

        if (evt && evt.currentTarget) {
            evt.currentTarget.classList.add('active');
        }
    };

    window.fetchUnitInfo = async function (contractID) {
        var loadingIndicator = document.getElementById('unitInfoloadingIndicator');
        try {
            if (loadingIndicator) {
                loadingIndicator.style.display = 'block';
            }
            var data = { ContractId: contractID };
            var response = await fetch('/Contracts/GetContractUnites', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });
            if (!response.ok) {
                console.error('Error in response:', response.status);
                alert('Error fetching data.');
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Failed to fetch data.');
        } finally {
            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }
        }
    };

    window.fetchPaymentInfo = async function (contractID) {
        var loadingIndicator = document.getElementById('paymentInfoloadingIndicator');
        try {
            if (loadingIndicator) {
                loadingIndicator.style.display = 'block';
            }
            var data = { ContractId: contractID };
            var response = await fetch('/Contracts/GetContractPayments', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });
            if (!response.ok) {
                console.error('Error in response:', response.status);
                alert('Error fetching data.');
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Failed to fetch data.');
        } finally {
            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }
        }
    };

    window.fetchPaymentChequesInfo = async function (transactionID) {
        var loadingIndicator = document.getElementById('loadingIndicator');
        try {
            if (loadingIndicator) {
                loadingIndicator.style.display = 'block';
            }
            var data = { TransactionID: transactionID };
            var response = await fetch('/Contracts/GetContractPaymentsCheques', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });
            if (!response.ok) {
                console.error('Error in response:', response.status);
                alert('Error fetching data.');
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Failed to fetch data.');
        } finally {
            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }
        }
    };

    window.fetchChargesInfo = async function (contractID) {
        var loadingIndicator = document.getElementById('chargesInfoloadingIndicator');
        try {
            if (loadingIndicator) {
                loadingIndicator.style.display = 'block';
            }
            var data = { ContractId: contractID };
            var response = await fetch('/Contracts/GetContractCharges', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });
            if (!response.ok) {
                console.error('Error in response:', response.status);
                alert('Error fetching data.');
            }
        } catch (error) {
            console.error('Error:', error);
            alert('Failed to fetch data.');
        } finally {
            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }
        }
    };

    window.unitDetail = function (unitID, unitNo) {
        var pageName = 'ContractDetail';
        var url = '/Properties/GetPropertyUnitDetailNew' +
            '?unitIDParam=' + encodeURIComponent(unitID) +
            '&pageName=' + pageName;
        window.location.href = url;
    };

    window.showPopup = function (index) {
        var modal = document.getElementById('myModal');
        if (modal) {
            modal.style.display = 'flex';
        }
    };

    window.closePopup = function () {
        var modal = document.getElementById('myModal');
        if (modal) {
            modal.style.display = 'none';
        }
    };

    window.addEventListener('click', function (event) {
        var modal = document.getElementById('myModal');
        if (modal && event.target === modal) {
            modal.style.display = 'none';
        }
    });

    function initContractDetailsPage() {
        var ctx = document.getElementById('myDonutChart');
        if (!ctx) {
            return;
        }

        if (typeof Chart === 'undefined' || typeof ChartDataLabels === 'undefined') {
            return;
        }

        new Chart(ctx.getContext('2d'), {
            type: 'pie',
            data: {
                labels: ['Paid Charges', 'Outstanding Charges'],
                datasets: [{
                    label: 'Charges',
                    data: [10.0, 20.0],
                    backgroundColor: ['rgba(0, 98, 255, 1)', 'rgba(255, 179, 0, 1)'],
                    borderColor: ['rgba(0, 98, 255, 1)', 'rgba(255, 179, 0, 1)'],
                    borderWidth: 2
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: { display: true },
                    datalabels: {
                        color: '#ffffff',
                        anchor: 'center',
                        align: 'center',
                        formatter: function (value) {
                            return value;
                        }
                    }
                }
            },
            plugins: [ChartDataLabels]
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        initContractsPage();
        initContractDetailsPage();
    });
}());

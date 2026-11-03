(function () {
    async function ChequesDownload(id) {
        showLoader();
        if (id == "" || id == null) {
            toastr.error("Error", "Transaction Id is empty");
            return;
        }
        try {
            const response = await fetch('/Payments/Payment/PaymentDetailDownloadReceipt', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ transactionid: id.toString() })
            });
            if (response.ok) {
                const data = await response.json();
                if (data.path) {
                    downloadPdf(data.path);
                } else {
                    toastr.error("Download failed: base64 path not found");
                }
            } else {
                toastr.error("Download failed:", response.message);
            }
        } catch (error) {
            toastr.error("Download failed:", error);
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

        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = 'file.pdf';
        link.click();

        URL.revokeObjectURL(link.href);
    }

    function showLoader() {
        const loader = document.getElementById('fullPageLoader');
        if (loader) loader.style.display = 'flex';
    }

    function hideLoader() {
        const loader = document.getElementById('fullPageLoader');
        if (loader) loader.style.display = 'none';
    }

    window.ChequesDownload = ChequesDownload;
    window.downloadPdf = downloadPdf;
    window.showLoader = showLoader;
    window.hideLoader = hideLoader;

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('[data-action="download-receipt"]').forEach(function (button) {
            button.addEventListener('click', function (event) {
                event.preventDefault();
                ChequesDownload(this.dataset.id);
            });
        });
    });





})();
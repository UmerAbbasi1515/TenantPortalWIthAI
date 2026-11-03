(function () {
    function initFaqsDetail() {
        const faqList = document.querySelector('.faq-list');
        if (!faqList) {
            return;
        }

        faqList.addEventListener('click', function (event) {
            const toggle = event.target.closest('.faq-header');
            if (!toggle) {
                return;
            }

            event.preventDefault();
            const item = toggle.closest('.faq-item');
            if (!item) {
                return;
            }

            const description = item.querySelector('.faq-description');
            if (!description) {
                return;
            }

            const isHidden = window.getComputedStyle(description).display === 'none';
            description.style.display = isHidden ? 'block' : 'none';
        });
    }

    document.addEventListener('DOMContentLoaded', initFaqsDetail);
})();

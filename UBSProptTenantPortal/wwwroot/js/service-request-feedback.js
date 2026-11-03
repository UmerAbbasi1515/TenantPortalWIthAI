// Service Request Detail - Feedback Functions

function loadFeedback() {
    debugger;
    document.getElementById('feedbackContainer').style.display = 'block';
    document.getElementById('feedbackButton').style.display = 'none';
}

function hideFeedback() {
    debugger;
    document.getElementById('feedbackContainer').style.display = 'none';
    document.getElementById('feedbackButton').style.display = 'block';
}

function feedbackDataValues() {
    debugger;
    const form = document.getElementById('feedbackForm');
    const formData = new FormData(form);

    // Get individual values
    const rating = formData.get('Rating');
    const description = formData.get('Description');

    console.log('Rating:', rating);
    console.log('Description:', description);

    // Validate
    if (!rating || rating === '0') {
        document.getElementById('ratingError3').style.display = 'block';
        return;
    }

    if (!description || description === '') {
        document.getElementById('descriptionError').style.display = 'block';
        return;
    }

    // Submit
    submitFeedbackValues(rating, description);
}

async function submitFeedbackValues(rating, description) {
    showLoader();
    debugger;
    var payload = {
        caseId: caseNo1,
        description: description,
        rating: rating,
    };
    debugger;
    try {
        const response = await fetch('/ServiceRequests/ServiceRequest/SubmitFeedBack', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        });
        debugger;
        if (response.ok) {
            const result = await response.json();
            if (result.success) {
                document.getElementById('feedbackContainer').style.display = 'none';
                document.getElementById('feedbackButton').style.display = 'none';
                toastr.success("Feedback submitted Successfully");
            } else {
                toastr.error("Feedback submitted failed: " + result.message);
            }
        } else {
            const errorText = await response.text();
            toastr.error("Server error: " + errorText);
        }
    } catch (error) {
        toastr.error("Server error: " + error);
    } finally {
        window.location.reload();
        hideLoader();
    }
}

// Initialize star rating
document.addEventListener('DOMContentLoaded', function () {
    const starsContainer = document.getElementById('starsRating');
    if (starsContainer) {
        const stars = starsContainer.querySelectorAll('i');
        const ratingInput = document.getElementById('ratingValue');
        let selectedRating = 0;

        stars.forEach(star => {
            star.addEventListener('click', function () {
                selectedRating = parseInt(this.getAttribute('data-rating'));
                ratingInput.value = selectedRating;
                updateStars(selectedRating);
                document.getElementById('ratingError').style.display = 'none';
            });

            star.addEventListener('mouseenter', function () {
                const rating = parseInt(this.getAttribute('data-rating'));
                updateStars(rating);
            });
        });

        starsContainer.addEventListener('mouseleave', function () {
            updateStars(selectedRating);
        });

        function updateStars(rating) {
            stars.forEach((star, index) => {
                if (index < rating) {
                    star.classList.remove('far');
                    star.classList.add('fas');
                } else {
                    star.classList.remove('fas');
                    star.classList.add('far');
                }
            });
        }

        // Form validation
        const form = document.getElementById('feedbackForm');
        if (form) {
            form.addEventListener('submit', function (e) {
                if (selectedRating === 0) {
                    e.preventDefault();
                    document.getElementById('ratingError').style.display = 'block';
                }
            });
        }
    }
});

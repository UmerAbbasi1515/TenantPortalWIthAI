// authFirebase.js

// Import Firebase modules
import { initializeApp } from "https://www.gstatic.com/firebasejs/9.0.0/firebase-app.js";
import { getAuth, RecaptchaVerifier, signInWithPhoneNumber } from "https://www.gstatic.com/firebasejs/9.0.0/firebase-auth.js";

// Firebase Configuration
const firebaseConfig = {
    apiKey: "AIzaSyD3kNXIpTAJnU8O7rahVDv_XZTKBvfyTqY",
    authDomain: "landlorddotnetcore.firebaseapp.com",
    projectId: "landlorddotnetcore",
    storageBucket: "landlorddotnetcore.appspot.com",
    messagingSenderId: "604232761436",
    appId: "1:604232761436:web:a5101607951001ecd22699",
    measurementId: "G-DVB5WX138B"
};

// Initialize Firebase and Auth
const app = initializeApp(firebaseConfig);
const auth = getAuth(app);

// Function to mask phone number
function maskPhoneNumber(phoneNumber) {
    const countryCode = phoneNumber.substring(0, 4);
    const firstDigit = phoneNumber[4];
    const last4Digits = phoneNumber.slice(-4);
    return countryCode + firstDigit + '****' + last4Digits;
}

// Initialize RecaptchaVerifier
const appVerifier = new RecaptchaVerifier("recaptcha-container", {
    size: 'invisible',
    callback: () => {
        // reCAPTCHA solved successfully
    },
    'expired-callback': () => {
        btnhideLoader("btngetOTPID");
        hideLoader();
    },
    'error-callback': () => {
        btnhideLoader("btngetOTPID");
        hideLoader();
    }
}, auth);

// Sign in with phone
function signInWithPhone(phoneNumber, isForgotMpin) {
    showLoader();
    btnshowLoader("btngetOTPID");

    // Safety timer
    const safetyTimer = setTimeout(() => {
        btnhideLoader("btngetOTPID");
        hideLoader();
        if (isForgotMpin == "True") {
            clearErrorMessages(divmpinOTP);
            btnhideLoader("btnforgotMpin");
            document.getElementById('VerifyOtpButtonByPhone').innerText = window.labels.verify;
        }
        const divPhoneNumber = document.getElementById("divPhoneNumber");
        showErrorMessage(divPhoneNumber, "reCAPTCHA timed out. Please try again.");
    }, 10000);

    signInWithPhoneNumber(auth, phoneNumber, appVerifier)
        .then((confirmationResult) => {
            clearTimeout(safetyTimer);
            btnhideLoader("btngetOTPID");
            hideLoader();
            if (isForgotMpin == "True") {
                clearErrorMessages(divmpinOTP);
                btnhideLoader("btnforgotMpin");
                document.getElementById('VerifyOtpButtonByPhone').innerText = window.labels.verify;
            }
            document.querySelectorAll("#pin-container .pin-input").forEach(input => {
                input.value = "";
            });
            const maskedPhoneNumber = maskPhoneNumber(phoneNumber);
            document.cookie = "maskedPhoneNumber=" + encodeURIComponent(maskedPhoneNumber) + "; path=/";
            window.confirmationResult = confirmationResult;
            if (isForgotMpin == "True") {
                document.getElementById('isForgotMpin').value = isForgotMpin;
                document.getElementById("divmpin").style.display = 'none';
            }
            clearPins("#divOTPValidation .pin-input");
            document.getElementById("divPhoneNumberValidation").style.display = 'none';
            clearErrorMessages(otpByPhone);
            document.getElementById('VerifyOtpButtonByPhone').innerText = window.labels.verify;
            document.getElementById("divOTPValidation").style.display = 'block';
        })
        .catch((error) => {
            clearTimeout(safetyTimer);
            btnhideLoader("btngetOTPID");
            hideLoader();
            if (isForgotMpin == "True") {
                clearErrorMessages(divmpinOTP);
                btnhideLoader("btnforgotMpin");
                document.getElementById('VerifyOtpButtonByPhone').innerText = window.labels.verify;
            }
            handleFirebaseError(error);
        });
}

// Handle Firebase errors
function handleFirebaseError(error) {
    btnhideLoader("btngetOTPID");
    hideLoader();
    let errorMessage = 'Error: ' + error.message;
    switch (error.code) {
        case 'auth/invalid-phone-number':
            errorMessage = window.labels.invalidPhoneNumber;
            break;
        case 'auth/missing-verify-code':
            errorMessage = 'Verification code is missing.';
            break;
        case 'auth/quota-exceeded':
            errorMessage = 'SMS quota exceeded. Please try again later.';
            break;
        case 'auth/too-many-requests':
            errorMessage = 'Too many requests. Please try again later.';
            break;
        case 'auth/network-request-failed':
            errorMessage = 'Network request failed. Please check your internet connection.';
            break;
    }
    const divPhoneNumber = document.getElementById("divPhoneNumber");
    showErrorMessage(divPhoneNumber, window.labels.pleaseEnterValidPhone);
}

// Handle Sign In Button Click
window.getPhoneNumberValidation = function (isForgotMpin) {
    btnshowLoader("btngetOTPID");
    if (isForgotMpin == "True") {
        clearErrorMessages(divmpinOTP);
        btnshowLoader("btnforgotMpin");
        document.getElementById('VerifyOtpButtonByPhone').innerText = window.labels.verify;
    }

    const countryCode = document.getElementById("country").value;
    const phoneInput = document.getElementById("phone").value;
    clearErrorMessages(divCountryCode);
    clearErrorMessages(divPhoneNumber);

    if (!countryCode || !phoneInput) {
        if (!countryCode) {
            btnhideLoader("btngetOTPID");
            if (isForgotMpin == "True") {
                clearErrorMessages(divmpinOTP);
                btnhideLoader("btnforgotMpin");
                document.getElementById('VerifyOtpButtonByPhone').innerText = window.labels.verify;
            }
            showErrorMessage(divCountryCode, window.labels.pleaseSelectCountryCode);
        }
        if (!phoneInput) {
            btnhideLoader("btngetOTPID");
            if (isForgotMpin == "True") {
                clearErrorMessages(divmpinOTP);
                btnhideLoader("btnforgotMpin");
                document.getElementById('VerifyOtpButtonByPhone').innerText = window.labels.verify;
            }
            showErrorMessage(divPhoneNumber, window.labels.pleaseEnterPhoneNoFirst);
        }
        return;
    }
    btnhideLoader("btngetOTPID");
    if (isForgotMpin == "True") {
        clearErrorMessages(divmpinOTP);
        btnhideLoader("btnforgotMpin");
        document.getElementById('VerifyOtpButtonByPhone').innerText = window.labels.verify;
    }
    const phoneNumber = countryCode + phoneInput;
    signInWithPhone(phoneNumber, isForgotMpin);
};
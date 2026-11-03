using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

public static class AuthLabelsPortal
{
    public static string language { get; set; }
    // Static constructor to initialize the static properties
    static AuthLabelsPortal()
    {
        language = "English"; // Default language
    }
    // Method to set the language based on input
    public static void SetLanguage(string languagePar)
    {
        language = languagePar;
    }
    public static bool isErrorFunction(dynamic response)
    {
        var result = response == AuthLabelsPortal.Unauthorized || response == AuthLabelsPortal.BadRequest || response == AuthLabelsPortal.NoDataFound || response == AuthLabelsPortal.AnyError || response == AuthLabelsPortal.ProcessError || response == AuthLabelsPortal.CouldNotConnectToServer;
        return result;
    }
    public static string selectaCountry => language == "English" ? "Select a country" : "اختر بلدا";
    public static string next => language == "English" ? "Next" : "التالي";
    public static string passwordMustbe8Char => language == "English" ? "Password must be at least 8 characters." : "يجب أن تتكون كلمة المرور من 8 أحرف على الأقل . ";
    public static string passwordMContainOneNumericVal => language == "English" ? "Password must contain at least one numeric value." : "يجب أن تحتوي كلمة المرور على قيمة رقمية واحدة على الأقل . ";
    public static string passwordMContainOneAlphaNumericVal => language == "English" ? "Password must contain at least onenon-alphanumeric character." : "يجب أن تحتوي كلمة المرور على حرف واحد غير أبجدي رقمي على الأقل . ";
    public static string passwordMContainOneCapSmaAlpha => language == "English" ? "Password must contain at least onesmall & one capital character" : "يجب أن تحتوي كلمة المرور على حرف صغير وحرف كبير واحد على الأقل . ";
    public static string unitedArabEmirate971 => language == "English" ? "United Arab Emirate (+971)" : "الإمارات العربية المتحدة (+971)";
    public static string pakistan92 => language == "English" ? "Pakistan (+92)" : "باكستان (+92)";
    public static string phoneNumberWithStaric = "";
    public static string fabPropertiesWeb => language == "English" ? "UBS Portals" : "تطبيق الويب الخاص ببنك أبوظبي الأول العقارية";
    public static bool isFirebaseOTPEnable = false;
    public static string jwtToken = "";
    public static string token = "";
    public static int landlordID = -1;
    public static string landlordName = "";
    // public static int landlordID = -1;
    public static string portalName = "LandlordPortal";
    public static int UserID = -1;
    public static int ReportPageSize = 20;
    public static string selectedLandlordReprtName = "";
    public static string userNameEng = "";
    public static string userNameAR = "";
    public static string email = "";
    public static string mobile = "";
    public static string Language => language == "English" ? "Language" : "اللغة";
    public static string SelectLanguage => language == "English"
        ? "Select your preferred language"
        : "حدد لغتك المفضلة";
    public static string select => language == "English" ? "Select" : "يختار";
    public static string english => language == "English" ? "English" : "English";
    public static string arabic => language == "English" ? "Arabic" : "العربية";
    public static string password => language == "English" ? "Password" : "كلمة المرور";
    public static string confirmPassword => language == "English" ? "Confirm Password" : "تأكيد كلمة المرور";
    public static string save => language == "English" ? "Save" : "يحفظ";
    public static string passwordStaric => language == "English" ? "*********" : "*********";
    public static string rememberPassword => language == "English" ? "Remember password" : "تذكر كلمة المرور";
    public static string forgotPassword => language == "English" ? "Forgot password?" : "هل نسيت كلمة السر؟";
    public static string forgotMpin => language == "English" ? "Forgot MPIN?" : "هل نسيت كلمة السر؟";
    public static string signin => language == "English" ? "Sign in" : "تسجيل الدخول";
    public static string pleaseEnterEmailAddressFirst => language == "English" ? "Please enter valid email address" : "الرجاء إدخال عنوان البريد **الإلكتروني أولا";
    public static string pleaseEnterValidEmailAddress => language == "English" ? "Please enter valid email address" : "الرجاء إدخال بريد إلكتروني صالح";
    public static string pleaseEnterValidEPhone => language == "English" ? "Please enter valid Mobile Number" : "الرجاء إدخال بريد إلكت**روني صالح";
    public static string userNotFound => language == "English" ? "User not found" : "لم يتم العثور على المستخدم";
    public static string userDoesNotExist => language == "English" ? "User does not exist" : "المستخدم غير موجود";
    public static string enterOtpSentOnEmail => language == "English" ? "Please enter otp that we sent on your email address" : "\nالرجاء إدخال كلمة مرور لمرة واحدة التي أرسلناها على عنوان بريدك الإلكتروني";
    public static string invalidOTP => language == "English" ? "Invalid OTP" : "كلمة المرور لمرة واحدة غير صالحة";
    public static string pleaseEnterPassword => language == "English" ? "Please enter password" : "الرجاء إدخال كلمة المرور";
    public static string pleaseEnterConfirmPassword => language == "English" ? "Please enter confirm password" : "الرجاء إدخال تأكيد كلمة المرور";
    public static string passwordDoesNotMatch => language == "English" ? "Password does not match" : "كلمة المرور غير متطابقة";
    public static string passwordMustbeStrong => language == "English" ? "Password must be strong" : "كلمة المرور يجب أن تكون قوية";
    public static string incorrectCredentials => language == "English" ? "Incorrect credentials" : "أوراق اعتماد غير صحيحة";
    public static string pleaseSelectCountryCode => language == "English" ? "Please select country code" : "الرجاء تحديد رمز البلد";
    public static string pleaseEnterPhoneNoFirst => language == "English" ? "Please enter a valid mobile number" : "الرجاء إدخال رقم الهاتف أولا";
    public static string pleaseEnterPasswordFirst => language == "English" ? "Please enter valid password" : "الرجاء إدخال كلمة المرور أولا";
    public static string continuebtn => language == "English" ? "Continue" : "متابعة";
    public static string signIntoAccount => language == "English" ? "Sign in to your Landlord account" : "تسجيل الدخول إلى الحساب";
    public static string enterEmailForLoginProcess => language == "English" ? "Enter your email to login" : "أدخل بريدك الإلكتروني لتسجيل الدخول";
    public static string enterEmailAndPass => language == "English" ? "Enter your password to login" : "أدخل كلمة المرور الخاصة بك لتسجيل الدخول";
    public static string emailAddress => language == "English" ? "Email Address" : "عنوان البريد الكتروني	";
    public static string enterEmailAddress => language == "English" ? "Enter Email Address" : "أدخل عنوان البريد الإلكتروني";
    public static string logout => language == "English" ? "Logout" : "تسجيل الخروج";
    public static string login => language == "English" ? "Sign in to your account" : "تسجيل الدخول";
    public static string cancel => language == "English" ? "Cancel" : "إلغاء";
    public static string personalDataDisclaimerShare => language == "English" ? "Disclaimer: Please do not enter any personal or sensitive data in any free text box which is not relevant to the purpose."
      : " اخلاء المسئولية :يرجى عدم مشاركة الرقم التعريفى الخاص بك";
    public static string mpinVerification => language == "English" ? "MPIN Verification" : "التحقق من رقم التعريف الشخصي";
    public static string setUp => language == "English" ? "SET UP" : "";
    public static string mpin => language == "English" ? "MPIN" : "";
    public static string setMpin => language == " English" ? "Set your MPIN" : "";
    public static string enterMpin => language == " English" ? "Enter a six digit MPIN" : "";
    public static string reEnterMpin => language == " English" ? "Re-enter the six digit MPIN" : "";
    
    public static string enterYourmPin => language == "English" ? "Enter your MPIN" : "أدخل رقم التعريف الشخصي الخاص بك";
    public static string reset => language == "English" ? "Reset " : "اعادة ضبط البرنامج ";
    public static string landlord => language == "English" ? "LandLord" : "المالك";
    public static string selectRole => language == "English" ? "Click on your role to continue"
      : "حدد دورك الرئيسي";
    public static string verify => language == "English" ? "Verify and Proceed"
      : "تحقق وتابع:";
    public static string mobileNumber => language == "English" ? "Mobile Number:"
      : "رقم الهاتف المحمول:";
    public static string getOtp => language == "English" ? "Get OTP" : "احصل على كلمة مرور لمرة واحدة";
    public static string countryCode => language == "English" ? "Country Code:" : "رمز البلد:";
    public static string enterTheCode => language == "English" ? "Enter the code that was sent to your Mobile Number "
      : " أدخل الرمز الذي تم إرساله إلى";
    public static string otpVerification => language == "English" ? "OTP Verification"
      : "التحقق من الرقم السري";
    public static string oneTimePassword => language == "English" ? "We will send you a One Time Password on this\nMobile Number."
      : "سنرسل لك كلمة مرور لمرة واحدة على هذا  الرقم للهاتف المحمول.";
    public static string ConnectionTimedOut => language == "English" ? "Connection timed out" : "انتهت مهلة الاتصال";
    public static string Unauthorized => language == "English" ? "Unauthorized" : "غير مصرح به";
    public static string NoDataFound => language == "English" ? "No data found" : "لاتوجد بيانات";
    public static string AnyError => language == "English" ? "Something went wrong" : "حدث خطأ ما";
    public static string ProcessError => language == "English" ? "Processing Error" : "خطأ في المعالجة";
    public static string internalServerError => language == "English" ? "Response status code does not indicate success: 500 (Internal Server Error)" : "";
    public static string BadRequest => language == "English" ? "Bad Request" : "طلب خاطئ";
    public static string CouldNotConnectToServer => language == "English" ? "Could not connect to server"
      : "لا يمكن الاتصال باالحاسوب الرئيسي";

}
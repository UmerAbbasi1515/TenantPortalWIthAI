//using System.Text.RegularExpressions;
//namespace UBSProptAuthPortal;

//public static class AuthLabelsRepository
//{
//    public static string _language { get; set; }
//    // Static constructor to initialize the static properties
//    static AuthLabelsRepository()
//    {
//        _language = "English"; // Default language
//    }
//    // Method to set the language based on input
//    public static void SetLanguage(string language)
//    {
//        _language = language;
//    }
//    public static bool isErrorFunction(dynamic response)
//    {
//        var result = response == AuthLabelsRepository.Unauthorized || response == AuthLabelsRepository.BadRequest || response == AuthLabelsRepository.NoDataFound || response == AuthLabelsRepository.AnyError || response == AuthLabelsRepository.ProcessError || AuthLabelsRepository.CouldNotConnectToServer;
//        return result;
//    }
//    public static string selectaCountry => _language == "English" ? "Select a country" : "اختر بلدا";
//    public static string next => _language == "English" ? "Next" : "التالي";
//    public static string passwordMustbe8Char => _language == "English" ? "Password must be at least 8 characters." : "يجب أن تتكون كلمة المرور من 8 أحرف على الأقل . ";
//    public static string passwordMContainOneNumericVal => _language == "English" ? "Password must contain at least one numeric value." : "يجب أن تحتوي كلمة المرور على قيمة رقمية واحدة على الأقل . ";
//    public static string passwordMContainOneAlphaNumericVal => _language == "English" ? "Password must contain at least onenon-alphanumeric character." : "يجب أن تحتوي كلمة المرور على حرف واحد غير أبجدي رقمي على الأقل . ";
//    public static string passwordMContainOneCapSmaAlpha => _language == "English" ? "Password must contain at least onesmall & one capital character" : "يجب أن تحتوي كلمة المرور على حرف صغير وحرف كبير واحد على الأقل . ";
//    public static string unitedArabEmirate971 => _language == "English" ? "United Arab Emirate (+971)" : "الإمارات العربية المتحدة (+971)";
//    public static string pakistan92 => _language == "English" ? "Pakistan (+92)" : "باكستان (+92)";
//    public static string phoneNumberWithStaric = "";
//    public static string fabPropertiesWeb => _language == "English" ? "UBS Portals" : "تطبيق الويب الخاص ببنك أبوظبي الأول العقارية";
//    public static bool isFirebaseOTPEnable = false;
//    public static string token = "";
//    public static int landlordID = 417;
//    // public static int landlordID = -1;
//    public static string portalName = "LandlordPortal";
//    public static int UserID = -1;
//    public static int ReportPageSize = 20;
//    public static string selectedLandlordReprtName = "";
//    public static string userNameEng = "";
//    public static string userNameAR = "";
//    public static string email = "";
//    public static string mobile = "";
//    public static string Language => _language == "English" ? "Language" : "اللغة";
//    public static string SelectLanguage => _language == "English"
//        ? "Select your preferred language"
//        : "حدد لغتك المفضلة";
//    public static string select => _language == "English" ? "Select" : "يختار";
//    public static string english => _language == "English" ? "English" : "English";
//    public static string arabic => _language == "English" ? "Arabic" : "العربية";
//    public static string password => _language == "English" ? "Password" : "كلمة المرور";
//    public static string confirmPassword => _language == "English" ? "Confirm Password" : "تأكيد كلمة المرور";
//    public static string save => _language == "English" ? "Save" : "يحفظ";
//    public static string passwordStaric => _language == "English" ? "*********" : "*********";
//    public static string rememberPassword => _language == "English" ? "Remember password" : "تذكر كلمة المرور";
//    public static string forgotPassword => _language == "English" ? "Forgot password?" : "هل نسيت كلمة السر؟";
//    public static string signin => _language == "English" ? "Sign in" : "تسجيل الدخول";
//    public static string pleaseEnterEmailAddressFirst => _language == "English" ? "Please enter email address first" : "الرجاء إدخال عنوان البريد الإلكتروني أولا";
//    public static string pleaseEnterValidEmailAddress => _language == "English" ? "Please enter valid email" : "الرجاء إدخال بريد إلكتروني صالح";
//    public static string userNotFound => _language == "English" ? "User not found" : "لم يتم العثور على المستخدم";
//    public static string userDoesNotExist => _language == "English" ? "User does not exist" : "المستخدم غير موجود";
//    public static string enterOtpSentOnEmail => _language == "English" ? "Please enter otp that we sent on your email address" : "\nالرجاء إدخال كلمة مرور لمرة واحدة التي أرسلناها على عنوان بريدك الإلكتروني";
//    public static string invalidOTP => _language == "English" ? "Invalid OTP" : "كلمة المرور لمرة واحدة غير صالحة";
//    public static string pleaseEnterPassword => _language == "English" ? "Please enter password" : "الرجاء إدخال كلمة المرور";
//    public static string pleaseEnterConfirmPassword => _language == "English" ? "Please enter confirm password" : "الرجاء إدخال تأكيد كلمة المرور";
//    public static string passwordDoesNotMatch => _language == "English" ? "Password does not match" : "كلمة المرور غير متطابقة";
//    public static string passwordMustbeStrong => _language == "English" ? "Password must be strong" : "كلمة المرور يجب أن تكون قوية";
//    public static string incorrectCredentials => _language == "English" ? "Incorrect credentials" : "أوراق اعتماد غير صحيحة";
//    public static string pleaseSelectCountryCode => _language == "English" ? "Please select country code" : "الرجاء تحديد رمز البلد";
//    public static string pleaseEnterPhoneNoFirst => _language == "English" ? "Please enter phone number first" : "الرجاء إدخال رقم الهاتف أولا";
//    public static string pleaseEnterPasswordFirst => _language == "English" ? "Please enter password first" : "الرجاء إدخال كلمة المرور أولا";
//    public static string continuebtn => _language == "English" ? "Continue" : "متابعة";
//    public static string signIntoAccount => _language == "English" ? "Sign in to account" : "تسجيل الدخول إلى الحساب";
//    public static string enterEmailForLoginProcess => _language == "English" ? "Enter your email to login" : "أدخل بريدك الإلكتروني لتسجيل الدخول";
//    public static string enterEmailAndPass => _language == "English" ? "Enter your password to login" : "أدخل كلمة المرور الخاصة بك لتسجيل الدخول";
//    public static string emailAddress => _language == "English" ? "Email Address" : "عنوان البريد الكتروني	";
//    public static string enterEmailAddress => _language == "English" ? "Enter Email Address" : "أدخل عنوان البريد الإلكتروني";
//    public static string logout => _language == "English" ? "Logout" : "تسجيل الخروج";
//    public static string login => _language == "English" ? "Login" : "تسجيل الدخول";
//    public static string cancel => _language == "English" ? "Cancel" : "إلغاء";
//    public static string personalDataDisclaimerShare => _language == "English" ? "Disclaimer: Please do not enter any personal or sensitive data in any free text box which is not relevant to the purpose."
//      : " اخلاء المسئولية :يرجى عدم مشاركة الرقم التعريفى الخاص بك";
//    public static string mpinVerification => _language == "English" ? "MPIN Verification" : "التحقق من رقم التعريف الشخصي";
//    public static string enterYourmPin => _language == "English" ? "Enter your MPIN" : "أدخل رقم التعريف الشخصي الخاص بك";
//    public static string reset => _language == "English" ? "Reset " : "اعادة ضبط البرنامج ";
//    public static string landlord => _language == "English" ? "LandLord" : "المالك";
//    public static string selectRole => _language == "English" ? "Select your role"
//      : "حدد دورك الرئيسي";
//    public static string verify => _language == "English" ? "Verify and Proceed"
//      : "تحقق وتابع:";
//    public static string mobileNumber => _language == "English" ? "Mobile Number:"
//      : "رقم الهاتف المحمول:";
//    public static string getOtp => _language == "English" ? "Get OTP" : "احصل على كلمة مرور لمرة واحدة";
//    public static string countryCode => _language == "English" ? "Country Code:" : "رمز البلد:";
//    public static string enterTheCode => _language == "English" ? "Enter the code that was sent to "
//      : " أدخل الرمز الذي تم إرساله إلى";
//    public static string otpVerification => _language == "English" ? "OTP Verification"
//      : "التحقق من الرقم السري";
//    public static string oneTimePassword => _language == "English" ? "We will send you a One Time Password on this\nMobile Number."
//      : "سنرسل لك كلمة مرور لمرة واحدة على هذا  الرقم للهاتف المحمول.";
//    public static string ConnectionTimedOut => _language == "English" ? "Connection timed out" : "انتهت مهلة الاتصال";
//    public static string Unauthorized => _language == "English" ? "Unauthorized" : "غير مصرح به";
//    public static string NoDataFound => _language == "English" ? "No data found" : "لاتوجد بيانات";
//    public static string AnyError => _language == "English" ? "Something went wrong" : "حدث خطأ ما";
//    public static string ProcessError => _language == "English" ? "Processing Error" : "خطأ في المعالجة";
//    public static string BadRequest => _language == "English" ? "Bad Request" : "طلب خاطئ";
//    public static string CouldNotConnectToServer => _language == "English" ? "Could not connect to server"
//      : "لا يمكن الاتصال باالحاسوب الرئيسي";

//}
using System.Text.RegularExpressions;
namespace UBSProptTenantPortal;

public static class LabelsTenantPortal
{
    public static string _language { get; set; }
    // Static constructor to initialize the static properties
    static LabelsTenantPortal()
    {
        _language = AuthLabelsPortal.language; // Default language

    }
    // Method to set the language based on input
    public static void SetLanguage(string language)
    {
        _language = language;
    }
    public static bool isErrorFunction(dynamic response)
    {
        var result = response == LabelsTenantPortal.Unauthorized || response == LabelsTenantPortal.BadRequest || response == LabelsTenantPortal.NoDataFound || response == LabelsTenantPortal.AnyError || response == LabelsTenantPortal.ProcessError || LabelsTenantPortal.CouldNotConnectToServer;
        return result;
    }
    public static string logout => _language == "English" ? "Logout" : "تسجيل الخروج";
    public static string selectaCountry => _language == "English" ? "Select a country" : "اختر بلدا";
    public static string next => _language == "English" ? "Next" : "التالي";
    public static string dashboard => _language == "English" ? "Dashboard" : "لوحة القيادة";
    public static string defaultDashboard => _language == "English" ? "Dashboard" : "لوحة القيادة الافتراضية";
    public static string getData => _language == "English" ? "Get Data" : "الحصول على البيانات";
    public static string getContracts => _language == "English" ? "Get Contracts" : "الحصول على العقود";
    public static string getInvoices => _language == "English" ? "Get Invoices" : "احصل على الفواتير";
    public static string supplierInvoices => _language == "English" ? "Supplier Invoices" : "فواتير الموردين";
    public static string getproperties => _language == "English" ? "Get Properties" : "احصل على خصائص";
    public static string tenancyContracts => _language == "English" ? "Tenancy Contracts" : "عقود الإيجار";
    public static string monthlyOverView => _language == "English" ? "Monthly Overview" : "نظرة عامة شهرية";
    public static string operaitonal => _language == "English" ? "Operational" : "التشغيلية";
    public static string financial => _language == "English" ? "Financial" : "مالي";
    public static string weekly => _language == "English" ? "Weekly" : "أسبوعي";
    public static string monthly => _language == "English" ? "Monthly" : "شهريا";
    public static string yearly => _language == "English" ? "Yearly" : "سنوي";
    public static string passwordMustbe8Char => _language == "English" ? "Password must be at least 8 characters." : "يجب أن تتكون كلمة المرور من 8 أحرف على الأقل . ";
    public static string passwordMContainOneNumericVal => _language == "English" ? "Password must contain at least one numeric value." : "يجب أن تحتوي كلمة المرور على قيمة رقمية واحدة على الأقل . ";
    public static string passwordMContainOneAlphaNumericVal => _language == "English" ? "Password must contain at least onenon-alphanumeric character." : "يجب أن تحتوي كلمة المرور على حرف واحد غير أبجدي رقمي على الأقل . ";
    public static string passwordMContainOneCapSmaAlpha => _language == "English" ? "Password must contain at least onesmall & one capital character" : "يجب أن تحتوي كلمة المرور على حرف صغير وحرف كبير واحد على الأقل . ";
    public static string unitedArabEmirate971 => _language == "English" ? "United Arab Emirate (+971)" : "الإمارات العربية المتحدة (+971)";
    public static string pakistan92 => _language == "English" ? "Pakistan (+92)" : "باكستان (+92)";
    public static string phoneNumberWithStaric = "";

    public static string portalName = "LandlordPortal";
    public static int UserID = -1;
    public static int ReportPageSize = 20;
    public static string selectedLandlordReprtName = "";
    public static string userNameEng = "";
    public static string userNameAR = "";
    public static string email = "";
    public static string mobile = "";
    public static string Language => _language == "English" ? "Language" : "اللغة";
    public static string SelectLanguage => _language == "English"
        ? "Select your preferred language"
        : "حدد لغتك المفضلة";
    public static string select => _language == "English" ? "Select" : "يختار";
    public static string english => _language == "English" ? "English" : "English";
    public static string arabic => _language == "English" ? "Arabic" : "العربية";
    public static string password => _language == "English" ? "Password" : "كلمة المرور";
    public static string confirmPassword => _language == "English" ? "Confirm Password" : "تأكيد كلمة المرور";
    public static string save => _language == "English" ? "Save" : "يحفظ";
    public static string passwordStaric => _language == "English" ? "*********" : "*********";
    public static string rememberPassword => _language == "English" ? "Remember password" : "تذكر كلمة المرور";
    public static string forgotPassword => _language == "English" ? "Forgot password?" : "هل نسيت كلمة السر؟";
    public static string signin => _language == "English" ? "Sign in" : "تسجيل الدخول";
    public static string pleaseEnterEmailAddressFirst => _language == "English" ? "Please enter email address first" : "الرجاء إدخال عنوان البريد الإلكتروني أولا";
    public static string pleaseEnterValidEmailAddress => _language == "English" ? "Please enter valid email" : "الرجاء إدخال بريد إلكتروني صالح";
    public static string userNotFound => _language == "English" ? "User not found" : "لم يتم العثور على المستخدم";
    public static string userDoesNotExist => _language == "English" ? "User does not exist" : "المستخدم غير موجود";
    public static string enterOtpSentOnEmail => _language == "English" ? "Please enter otp that we sent on your email address" : "\nالرجاء إدخال كلمة مرور لمرة واحدة التي أرسلناها على عنوان بريدك الإلكتروني";
    public static string invalidOTP => _language == "English" ? "Invalid OTP" : "كلمة المرور لمرة واحدة غير صالحة";
    public static string pleaseEnterPassword => _language == "English" ? "Please enter password" : "الرجاء إدخال كلمة المرور";
    public static string pleaseEnterConfirmPassword => _language == "English" ? "Please enter confirm password" : "الرجاء إدخال تأكيد كلمة المرور";
    public static string passwordDoesNotMatch => _language == "English" ? "Password does not match" : "كلمة المرور غير متطابقة";
    public static string passwordMustbeStrong => _language == "English" ? "Password must be strong" : "كلمة المرور يجب أن تكون قوية";
    public static string incorrectCredentials => _language == "English" ? "Incorrect credentials" : "أوراق اعتماد غير صحيحة";
    public static string pleaseSelectCountryCode => _language == "English" ? "Please select country code" : "الرجاء تحديد رمز البلد";
    public static string pleaseEnterPhoneNoFirst => _language == "English" ? "Please enter phone number first" : "الرجاء إدخال رقم الهاتف أولا";
    public static string pleaseEnterPasswordFirst => _language == "English" ? "Please enter password first" : "الرجاء إدخال كلمة المرور أولا";
    public static string continuebtn => _language == "English" ? "Continue" : "متابعة";
    public static string signIntoAccount => _language == "English" ? "Sign in to account" : "تسجيل الدخول إلى الحساب";
    public static string enterEmailForLoginProcess => _language == "English" ? "Enter your email to login" : "أدخل بريدك الإلكتروني لتسجيل الدخول";
    public static string enterEmailAndPass => _language == "English" ? "Enter your password to login" : "أدخل كلمة المرور الخاصة بك لتسجيل الدخول";
    public static string emailAddress => _language == "English" ? "Email Address" : "عنوان البريد الكتروني	";
    public static string enterEmailAddress => _language == "English" ? "Enter Email Address" : "أدخل عنوان البريد الإلكتروني";
    public static string searchAnything => _language == "English" ? "Search anything..." : "ابحث عن أي شيء...";

    public static string notification => _language == "English" ? "Notifications" : "إشعارات";
    public static string view => _language == "English" ? "View" : "منظر";
    public static string allNotification => _language == "English" ? "All Notification" : "كل الإخطار";
    public static string bookmark => _language == "English" ? "Bookmark" : "إشارة مرجعية";
    public static string searchBookmark => _language == "English" ? "Search Bookmark..." : "بحث في الإشارة المرجعية...";
    public static string message => _language == "English" ? "Message" : "رسالة";

    public static string property => _language == "English" ? "Property" : "العقار ";
    public static string propertyID => _language == "English" ? "Property Id" : "معرف العقار ";
    public static string propertyName => _language == "English" ? "Property Name" : " اسم العقار ";
    public static string properties => _language == "English" ? "Properties" : "ملكيات ";
    public static string emirate => _language == "English" ? "Emirate" : "الإمارة ";
    public static string type => _language == "English" ? "Type" : "يكتب ";
    public static string contractNo => _language == "English" ? "Contract No" : "رقم العقد ";
    public static string unitNo => _language == "English" ? "Unit No" : "رقم الوحدة ";
    public static string contractDate => _language == "English" ? "Contract Date" : "تاريخ العقد ";
    public static string status => _language == "English" ? "Status" : "حالة ";
    public static string category => _language == "English" ? "Category" : "فئة ";
    public static string viewAll => _language == "English" ? "View All" : "عرض الكل ";
    public static string invoiceNumber => _language == "English" ? "Invoice Number" : "رقم الفاتورة";
    public static string invoiceAmount => _language == "English" ? "Invoice Amount" : "مبلغ الفاتورة ";
    public static string invoiceDate => _language == "English" ? "Invoice Date" : "تاريخ الفاتورة ";
    public static string myProfile => _language == "English" ? "My Profile" : "الملف الشخصى";
    public static string profile => _language == "English" ? "Profile" : "الشخصى";
    public static string report => _language == "English" ? "Report" : "تقرير";
    public static string faqs => _language == "English" ? "FAQs" : "الأسئلة والأجوبة الشائعة";
    public static string help => _language == "English" ? "Help" : "طلب المساعدة";
    public static string ConnectionTimedOut => _language == "English" ? "Connection timed out" : "انتهت مهلة الاتصال";
    public static string Unauthorized => _language == "English" ? "Unauthorized" : "غير مصرح به";
    public static string NoDataFound => _language == "English" ? "No data found" : "لاتوجد بيانات";
    public static string AnyError => _language == "English" ? "Something went wrong" : "حدث خطأ ما";
    public static string ProcessError => _language == "English" ? "Processing Error" : "خطأ في المعالجة";
    public static string BadRequest => _language == "English" ? "Bad Request" : "طلب خاطئ";
    public static string CouldNotConnectToServer => _language == "English" ? "Could not connect to server"
      : "لا يمكن الاتصال باالحاسوب الرئيسي";


    public static string GetStatusCssClass(string? status)
    {
        return status?.Trim().ToLower() switch
        {
            "active" => "active",
            "expired" => "expired",
            "cancelled" or "rejected" => "rejected",
            "ended" => "ended",
            "terminated" => "terminated",
            "received" or "draft" => "received",
            "inprogress" => "inprogress",
            "underapproval" => "underapproval",
            "assigntocontractor" => "assigntocontractor",
            "posted" => "posted",
            _ => "received"
        };
    }


}
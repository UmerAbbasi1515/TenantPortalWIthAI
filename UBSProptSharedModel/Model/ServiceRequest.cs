using System.Text.Json.Serialization;
using UBSProptSharedModel.Model;

public class ServiceRequestViewModel
{
    public ServiceRequestsResponse? fmRequest { get; set; }
    public ServiceRequestsResponse? pmRequest { get; set; }
    public ServiceRequestFilter? serviceRequestFilter { get; set; }
    public ServiceRequestTabs? tabs { get; set; }
}
public class ServiceRequestTabs
{
    public List<ServiceRequest>? ServiceRequests { get; set; }
    public String? tabName { get; set; }
}
public class SrReqDetailMainDocumentCommunication
{
    public ServiceRequestDetailsTModel? mainInfo { get; set; }
    public UploadDocParam? uploadDocParam { get; set; }
    public SrReqDetailReport? report { get; set; }
    public UploadDocResponse? uploadDocResponse { get; set; }
    public CommunicationModel? CoomunicationResponse { get; set; }
}
public class SrReqDetailMainDocumentCommunicationVendor
{
    public ServiceRequestDetailsModel? mainInfo { get; set; }
    public UploadDocParam? uploadDocParam { get; set; }
    public SrReqDetailReport? report { get; set; }
    public UploadDocResponse? uploadDocResponse { get; set; }
    public CommunicationModel? CoomunicationResponse { get; set; }
}
public class SrReqDetailMainDocumentCommunicationPublic
{
    public ServiceRequestDetailsTModel? mainInfo { get; set; }
    public UploadDocParam? uploadDocParam { get; set; }
    public SrReqDetailReport? report { get; set; }
    public UploadDocResponse? uploadDocResponse { get; set; }
    public TicketReplyResponsePublic? CoomunicationResponse { get; set; }
}
public class SrReqDetailReport
{
    public List<PhotoFile?>? photos { get; set; }
    public List<DocFile?>? files { get; set; }
    public SRReportDetailModel? report { get; set; }
    public string? errorMessage { get; set; }
}
public class SrReqAddNewRequest
{
    public GetCaseCategoryModel? caseCategory { get; set; }
    public GetCaseSubCateoryModel? caseSubCategory { get; set; }
    public GetPropertiesModel? propeties { get; set; }
    public GetContactTimingModel? contactTiming { get; set; }
}

public class TicketReplyResponsePublic
{
    public string? Status { get; set; }

    public List<TicketReplyPublic>? TicketReply { get; set; }

    public string? Message { get; set; }
    public string? errorMessage { get; set; }
}

public class TicketReplyPublic
{
    public string? Reply { get; set; }

    public string? DateTime { get; set; }

    public string? FileName { get; set; }

    public int? PublicUserId { get; set; }

    public int UserId { get; set; }

    public int Id { get; set; }
}
#region  Service request



public class ServiceRequestsResponse
    {
        public string? Status { get; set; }
        public List<ServiceRequest>? ServiceRequests { get; set; }
        public string? selectedCat { get; set; }
        public string? Message { get; set; }
    //for Technician
    public int OpenCount { get; set; }
    public int CloseCount { get; set; }
    public int TotalCount { get; set; }
}

    public class ServiceRequest
    {
        public string? tabName { get; set; }
        public dynamic? RequestNo { get; set; }
        public string? Category { get; set; }
        public string? CategoryAR { get; set; }
        public string? Detail { get; set; }
        public string? DetailAR { get; set; }
        public string? Date { get; set; }
        public string? PropertyName { get; set; }
        public string? PropertyNameAr { get; set; }
        public string? Status { get; set; }
        public string? StatusAR { get; set; }
        public dynamic? Units { get; set; }
        public string? SubCategory { get; set; }
        public string? SubCategoryAR { get; set; }
        public string? UnitRefNo { get; set; }
        public int? Code { get; set; }
        public string? CaseType { get; set; }
        public int? DueActionId { get; set; }
        public int? StageId { get; set; }

   

}

#endregion

#region  MainInfo

public class ServiceRequestDetailsModel
{
    public string? Status { get; set; }
    public Detail? Detail { get; set; }
    public StatusInfo? StatusInfo { get; set; }
    public string? Message { get; set; }
    public ContractInfo? ContractInfo { get; set; }
    public StageInfo? StageInfo { get; set; }
    public CaseStatus? caseStatus { get; set; }
    public List<PhotoFile>? reqPhotos { get; set; }
    public SurveyQuestions? surveyQAs { get; set; }
    public TenantGetSrFeedback? feedback { get; set; }
    public List<DocFile>? reqReport { get; set; }
}
public class ServiceRequestDetailsTModel
{
    public string? Status { get; set; }
    public Detail? Detail { get; set; }
    public StatusInfo? StatusInfo { get; set; }
    public string? Message { get; set; }
    public ContractInfo? ContractInfo { get; set; }
    public StageInfo? StageInfo { get; set; }
    public CaseStatus? caseStatus { get; set; }
    public List<PhotoFileTen>? reqPhotos { get; set; }
    public SurveyQuestions? surveyQAs { get; set; }
    public TenantGetSrFeedback? feedback { get; set; }
    public List<DocFile>? reqReport { get; set; }
}
public class CaseStatus
{
    public bool CanCancel { get; set; }
    public bool CanReopen { get; set; }
    public bool CanAddFeedback { get; set; }
    public bool RequestClosed { get; set; }
    public bool CanTakeSurvey { get; set; }
}

public class Detail
{
    public object? CaseNo { get; set; }
    public string? CaseType { get; set; }
    public string? PropertyName { get; set; }
    public string? PropertyNameAr { get; set; }
    public string? contactPersonName { get; set; }
    public string? contactPersonMobile { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public string? ContactTiming { get; set; }
    public string? InitialAssesment { get; set; }
    public string? CaseServiceLevel { get; set; }
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public string? Status { get; set; }
    public string? StatusAR { get; set; }
    public string? Language { get; set; }
    public string? Units { get; set; }
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public string? UnitRefNo { get; set; }
    public string? unitRefNoOtherContactPersonName { get; set; } // From unitRefNoOtherContactPersonName
    public string? otherContactPersonName { get; set; } // From otherContactPersonMobile
    public string? otherContactPersonMobile { get; set; } // From otherContactPersonMobile
    public int? CaseFeedback { get; set; }
    public int? CaseCategouryId { get; set; }
    public int? CaseSubCatagouryId { get; set; }
    public string? VacatingDate { get; set; }
    public string? VacatingReason { get; set; }
    public string? CategoryAR { get; set; }
    public string? SubCategoryAR { get; set; }
    public string? RequestType { get; set; }
    public bool? contractorAcknowledged { get; set; }
    public bool? contractorRejected { get; set; }
}
public class StatusInfo
{
    public bool? CanCancel { get; set; }
    public bool? canClose { get; set; }
    public bool? CanReopen { get; set; }
    public bool? CanAddFeedback { get; set; }
    public bool? RequestClosed { get; set; }
    public bool? CanTakeSurvey { get; set; }
    public bool? CanUploadDocs { get; set; } // Mapped from canUploadDocuments
    public bool? ShowPhotos { get; set; }     // Mapped from canUploadPhotos
    public bool? CanChat { get; set; }
}
public class ContractInfo
{
    public string? Contractno { get; set; }
    public int? ContractId { get; set; }
    public string? PropertyName { get; set; }
    public string? PropertyNameAr { get; set; }
}
public class StageInfo
{
    public int? DueActionid { get; set; }
    public int? StageId { get; set; }
}


public class SurveyQuestions
{
    public List<FaqQuestion?>? FaqQuestion { get; set; }
    public int currentQuestion { get; set; } = 0;
    public int? selectedAnswer { get; set; } = 0;
}

public class FaqQuestion
{
    public int? QuestionId { get; set; }
    public int? AnswerId { get; set; }
    public string? Title { get; set; }
    public string? TitleAr { get; set; }
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public SurveyQuestionAnswers? Answers { get; set; }

    // Dart GetX reactive fields converted to plain .NET properties
    public bool LoadingAnswers { get; set; } = false;
    public string? ErrorLoadingAnswers { get; set; } = "";
    public int SelectedAnswer { get; set; } = -1;
}

public class SurveyQuestionAnswers
{
    public List<FaqOption>? FaqOptions { get; set; }
    public double Progress { get; set; } = 0;
}

public class FaqOption
{
    public int? OptionId { get; set; }
    public int? QuestionId { get; set; }
    public string? OptionTitle { get; set; }
    public string? OptionTitleAr { get; set; }
    public bool? DescriptionRequired { get; set; }
}

public class SurveyQASaveParam
{
    public string? answerId { get; set; }
    public string? customerChoice { get; set; }
    public string? description { get; set; }
    public string? IsCompleted { get; set; }
    public int? QuestionId { get; set; }
}

public class TenantGetSrFeedback
{
    public string? Status { get; set; }
    public Feedback? Feedback { get; set; }
    public string? Message { get; set; }
}
public class Feedback
{
    public int? CaseNo { get; set; }
    public string? Description { get; set; }
    public double? Rating { get; set; }
}
public class SubmitFeedback
{
    public string? caseId { get; set; }
    public string? description { get; set; }
    public string? rating { get; set; }
}

#endregion

#region  Documents
public class UploadDocResponse
    {
        public List<DocFile>? Docs { get; set; }
        public CaseStageInfo? CaseStageInfo { get; set; }
        public string? ResponseMessage { get; set; }
        public string? ResponseMessageAR { get; set; }
    }

    public class CaseStageInfo
    {
        public string? DueActionId { get; set; }
        public string? StageId { get; set; }
    }

    public class DocFile
    {
        public string? Id { get; set; }

        // Use byte[] for binary data like Uint8List
        public byte[]? File { get; set; }

        public string? Path { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? NameAr { get; set; }
        public string? Expiry { get; set; }
        public string? Size { get; set; }
        public string? AttachmentDate { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? AttachmentId { get; set; }
        public string? Extentsion { get; set; }
    
        public int? DocumentTypeId { get; set; }
        public bool? IsRejected { get; set; }

        // These fields are UI-related in Flutter. You can skip or keep them if needed for internal logic.
        public bool Loading { get; set; } = false;
        public bool ErrorLoading { get; set; } = false;
        public bool Removing { get; set; } = false;
        public bool ErrorRemoving { get; set; } = false;
        public bool Update { get; set; } = false;
}

public class InvoiceDocumentTypeResponse
{
    public string? status { get; set; }
    public List<InvoiceDocumentRecord> record { get; set; } = new();
}

public class InvoiceDocumentRecord
{
    public int id { get; set; }
    public int documentTypeId { get; set; }
    public string? name { get; set; }
    public string? attachmentUrl { get; set; }
    public string? extentsion { get; set; }
    public string? attachmentDate { get; set; }
    public string? expireDate { get; set; }
    public string? attachmentId { get; set; }
    public bool isRejected { get; set; }
}


public class OfferDocsResponse
{
    public string? status { get; set; }
    public string? errorMessage { get; set; }          // set by your controller when the API call fails
    public List<OfferDocRecord>? record { get; set; }

    // Number of real documents (empty upload slot not counted)
    public int DocCount() => record?.Count ?? 0;

    // Index of the upload slot: 0 docs -> 1, 1 doc -> 2, 2 docs -> 3, 3 docs -> 4
    public int UploadSlotIndex() => DocCount() + 1;

    // record + ONE extra item at the end for uploading a new doc
    public List<OfferDocRecord> GetDocsWithUploadSlot()
    {
        var list = new List<OfferDocRecord>(record ?? new List<OfferDocRecord>());
        list.Add(new OfferDocRecord { isUploadSlot = true });
        return list;
    }
}

public class OfferDocRecord
{
    public int documentID { get; set; }
    public string? title { get; set; }
    public string? originalFiles { get; set; }
    public string? physicalFolder { get; set; }
    public DateTime? addedDate { get; set; }
    public string? documentType { get; set; }

    // Not in the API: true only for the extra "Click to Upload" item
    public bool isUploadSlot { get; set; }

    public string GetExtension() =>
        System.IO.Path.GetExtension(originalFiles ?? "").TrimStart('.').ToLower();
}

#endregion

#region Communication
public class CommunicationModel
    {
        public string? Status { get; set; }
        public List<TicketReply>? TicketReply { get; set; }
        public string? Message { get; set; }
        public string? errorMessage { get; set; } // Add this
}

    public class TicketReply
    {
        public int? TicketReplyId { get; set; }
        public string? Reply { get; set; }
        public string? DateTime { get; set; }
        public int? UserId { get; set; }
        public int? UserId2 { get; set; }
           public int? PublicUserId { get; set; }
    public string? FileName { get; set; }
        public string? Path { get; set; }
        public bool DownloadingFile { get; set; } = false;
    }
#endregion

#region Parameters
public class UploadDocParam
    {
        public string? caseNo { get; set; }
        public string? path { get; set; }
        public string? name { get; set; }
        public string? expiry { get; set; }
        public string? documentTypeId { get; set; }
    }
public class ServiceRequestsCaseSubCategoryParam
{
    public string? CategoryId { get; set; }
}
public class ServiceRequestsCasePropertiesParam
{
    public string? Tenantid { get; set; }
}
public class ServiceRequestsCatUnitTypeParam
{
    public string? Category { get; set; }
}
#endregion

#region photos
public class PhotoFile
{
    public int? Id { get; set; }
    public byte[]? File { get; set; }      // Uint8List → byte[]
    public string? Path { get; set; }
    public string? Type { get; set; }
    public string? Source { get; set; }

    public bool Uploading { get; set; } = false;
    public bool Downloading { get; set; } = false;
    public bool ErrorUploading { get; set; } = false;
    public bool ErrorDownloading { get; set; } = false;
    public string? ErrorText { get; set; } = string.Empty;
    public bool Removing { get; set; } = false;
    public bool ErrorRemoving { get; set; } = false;
}
public class PhotoFileTen
{
    public int? photoId { get; set; }  // Uint8List → byte[]
    public string? photoSource { get; set; }
    public string? attachmentType { get; set; }
}

#endregion

#region caseCategory
public class GetCaseCategoryModel
{
    public string? Status { get; set; }
    public int? SelectedCategoryId { get; set; }
    public List<CaseCategory>? CaseCategories { get; set; }
    public string? Message { get; set; }
}
public class CaseCategory
{
    public dynamic? Id { get; set; }
    public string? Name { get; set; }
    public string? NameAR { get; set; }
    public string? Type { get; set; }
}
#endregion

#region caseSubCategory
public class GetCaseSubCateoryModel
{
    public string? Status { get; set; }
    public List<CaseSubCategory>? CaseSubCategories { get; set; }
    public string? Message { get; set; }
}

public class CaseSubCategory
{
    public dynamic? Id { get; set; } 
    public string? Name { get; set; }
    public string? NameAR { get; set; }
}
#endregion

#region Property
public class GetPropertiesModel
{
    public string? Status { get; set; }
    public List<Property>? Properties { get; set; }
    public string? Message { get; set; }
}

public class Property
{
    public dynamic? ContractUnitID { get; set; }  // `object` to represent Dart's dynamic
    public string? UnitName { get; set; }
    public string? UnitNameAr { get; set; }
    public string? PropertyName { get; set; }
    public string? PropertyNameAR { get; set; }
    public string? Status { get; set; }
    public List<Property>? Properties { get; set; }  // Recursive list
    public string? Message { get; set; }
}
#endregion

#region ContactTiming
public class GetContactTimingModel
{
    public string? Status { get; set; }
    public List<ContactTiming>? ContactTiming { get; set; }
    public string? Message { get; set; }
}

public class ContactTiming
{
    public dynamic? Id { get; set; }  // Use int or Guid if known
    public string? Name { get; set; }
}
#endregion

#region Add Request
public class SubmitServiceRequestParam
{
    public string? CaseCategoryId { get; set; }
    public string? CaseSubCategoryId { get; set; }
    public string? ContractUnitId { get; set; }
    public string? Description { get; set; }
    public string? OtherContactPersonName { get; set; }
    public string? OtherContactPersonMobile { get; set; }
    public string? ContactTimeId { get; set; } // Optional: rename to match C# naming convention
}
public class SaveServiceRequestModel
{
    public string? Status { get; set; }
    public AddServiceRequestNReq? AddServiceRequest { get; set; }
    public string? Message { get; set; }
}

public class AddServiceRequestNReq
{
    public int? CaseNo { get; set; }
}
#endregion

#region Report
public class SRReportDetailModel
{
    public string? Status { get; set; }
    public string? fgpCorrectionId { get; set; }
    public string? proposedRemedy { get; set; }
    public string? description { get; set; }
    public Data? Data { get; set; }
}

public class Data
{
    public int? FgpCorrectionId { get; set; }
    public string? ProposedRemedy { get; set; }
    public string? Description { get; set; }
}

public class CloseReqData
{
    public string? CaseNo { get; set; }
    public string? FGPCorrectionId { get; set; }
    public string? ProposedRemedy { get; set; }
    public string? Description { get; set; }

}
#endregion



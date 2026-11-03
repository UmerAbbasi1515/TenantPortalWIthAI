public class GetNotificationParameter
{
    public string? pageNo { get; set; }
    public string? pageSize { get; set; }
    public string? status { get; set; }
}
public class GetNotificationDetailParameter
{
    public string? NotificationId { get; set; }
}

public class NotificationViewModel
{
    public NotificationModel? allNotification { get; set; }
    public NotificationModel? unreadNotifications { get; set; }
    public NotificationReadModel? readNotifications { get; set; }
    public NotificationArchivedModel? archiveNotifications { get; set; }
}
public class NotificationDetailsModel
{
    public string? StatusCode { get; set; }
    public string? Status { get; set; }
    public Notification? Notification { get; set; }
    public string? Message { get; set; }
}
public class NotificationArchivedModel
{
    public string? Status { get; set; }
    public string? StatusCode { get; set; }
    public string? Message { get; set; }
}
public class NotificationReadModel
{
    public string? Status { get; set; }
    public string? StatusCode { get; set; }
    public string? Message { get; set; }
}
public class GetDataRepose
{
    public string? Status { get; set; }
    //public Dashboard? Dashboard { get; set; }
    public string? StatusCode { get; set; }
    public string? Message { get; set; }
    public int? UnreadNotifications { get; set; }
    public int? ContractRenewalAction { get; set; }

    public dynamic? contractExpiringIn30Days { get; set; }
    public dynamic? toBePaidIn30DaysLength { get; set; }
    public dynamic? paymentCurrency { get; set; }
    public dynamic? paidCurrency { get; set; }
    public dynamic? toBePaidCurrency { get; set; }
    public dynamic? balanceCurrency { get; set; }
    public dynamic? rentalVal { get; set; }
    public dynamic? rentOutstanding { get; set; }
}


public class NotificationModel
{
    public string? StatusCode { get; set; }
    public string? Status { get; set; }
    public List<Notification>? Notifications { get; set; }
    public string? Message { get; set; }
}

public class Notification
{
    public int? NotificationId { get; set; }
    public dynamic? CreatedOn { get; set; }
    public string? CurrentStatus { get; set; }
    public int? NotificationTypeId { get; set; }
    public bool? IsRead { get; set; }
    public dynamic? ReadOn { get; set; }
    public bool? Sent { get; set; }
    public dynamic? SentOn { get; set; }
    public string? Batch { get; set; }
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Mobile { get; set; }
    public string? Title { get; set; }
    public string? DescriptionAr { get; set; }
    public string? TitleAr { get; set; }
    public string? Description { get; set; }
    public string? NotificationType { get; set; }
    public int? TotalRecords { get; set; }
}

public class NotificationPopUpModel
{
    public string? StatusCode { get; set; }
    public string? Status { get; set; }
    public List<NotificationPopUp>? Notifications { get; set; }
    public string? Message { get; set; }
}

public class NotificationPopUp
{
    public string? ContractNo { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public int? StageId { get; set; }
    public int? NotificationId { get; set; }
    public int? RecordId { get; set; }
    public string? Title { get; set; }
    public string? TitleAr { get; set; }
    public string? RecordType { get; set; }
    public string? Description { get; set; }
    public string? DescriptionAr { get; set; }
    public string? CreatedOn { get; set; }
    public int? DueActionId { get; set; }
    public int? CaseId { get; set; }
    public bool? ShowExtend { get; set; }
}



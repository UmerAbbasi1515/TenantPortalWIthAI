using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using UBSProptTenantPortal.Models;
using Newtonsoft.Json;
using UBSProptRepository.Repository;

namespace UBSProptPortal.Areas.Notifications.Controllers
{
    [Area("Notifications")]
    public class NotificationController : BaseController
    {
        private readonly DashboardRepo dahboardRepo;
        private readonly NotificationRepo notificationRepo;
        private readonly UBSTenantApiConfig apiConfig;
        new string? token = "";

        public NotificationController(DashboardRepo _dahboardRepo, NotificationRepo _notificationRepo, IOptions<UBSTenantApiConfig> _apiConfig)
        {
            dahboardRepo = _dahboardRepo;
            notificationRepo = _notificationRepo;
            apiConfig = _apiConfig.Value;
            token = AuthLabelsPortal.jwtToken ?? "";
        }
      

        // Contracts
        public async Task<IActionResult> NotificationsN()
        {
            NotificationViewModel model = new NotificationViewModel();
            model.allNotification = await GetAllNoti1fications("");
            model.unreadNotifications = await GetAllNoti1fications("Unread");
            return View(model);
        }

        public async Task<NotificationModel> GetAllNoti1fications(string status)
        {
            try
            {
                ViewBag.errorGetAll = "";
                var data = new GetNotificationParameter
                {
                    status = status,
                    pageNo = 1.ToString(),
                    pageSize = 200.ToString(),
                };
                var postUrl = apiConfig.getNotifications;
                var response = await notificationRepo.GetNotifications(data, postUrl, token??"");
                if (response is string) {
                    ViewBag.errorGetAll = response;
                    NotificationModel model1 = new NotificationModel
                    {
                        Status = "",
                        Notifications = null,
                        Message = ""
                    };
                    return model1;
                }
                    Console.WriteLine(response);
                // Convert JArray to List
                List<Notification> dataList = response.notifications.ToObject<List<Notification>>();
                if (dataList.Count == 0) { ViewBag.errorGetAll = "No Data Found"; }
                NotificationModel model = new NotificationModel
                {
                        Status = response.status,
                        Notifications = dataList,
                        Message = response.message
                    };
                    Console.WriteLine(model.Status);
                    Console.WriteLine(model.Message);
                    Console.WriteLine(model.Notifications);
                    return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetAll = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                NotificationModel model1 = new NotificationModel
                {
                    Status = "",
                    Notifications = null,
                    Message = ""
                };
                return model1;
            }
        }
        public async Task<NotificationViewModel> ArchiveNoti1fications([FromBody] GetNotificationDetailParameter param)
        {
            try
            {
                ViewBag.errorGetAll = "";
                var data = new GetNotificationDetailParameter
                {
                    NotificationId = param.NotificationId,
                };
                var postUrl = apiConfig.tenantArchiveNotifications;
                var response = await notificationRepo.ArchiveNotification(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorGetAll = response;
                    NotificationViewModel model1 = new NotificationViewModel();
                    return model1;
                }
                Console.WriteLine(response);
                NotificationArchivedModel resp = new NotificationArchivedModel
                {
                    Status = response.status,
                    Message = response.message
                };
                NotificationViewModel model = new NotificationViewModel();
                model.archiveNotifications = resp;
                Console.WriteLine(model.archiveNotifications.Status);
                Console.WriteLine(model.archiveNotifications.Message);
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetAll = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                NotificationViewModel model1 = new NotificationViewModel();
                return model1;
            }
        }

        public async Task<IActionResult> NotificationsDetails(string id)
        {
            try
            {
                ViewData["NotificationID"] = id;
                ViewBag.errorGetALlDetail = "";
                var data = new GetNotificationDetailParameter
                {
                    NotificationId = id,
                };
                var postUrl = apiConfig.tenantNotificationsDetails;
                var response = await notificationRepo.GetNotificationsDetail(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorGetALlDetail = response;
                    NotificationDetailsModel model1 = new NotificationDetailsModel
                    {
                        Status = "",
                        Notification = null,
                        Message = ""
                    };
                    return View(model1);
                }
                Console.WriteLine(response);
                if (response.status == "Ok") {
                    var para = new GetNotificationDetailParameter
                    {
                        NotificationId = id,
                    };
                    dynamic isRead = await ReadNoti1fications(para);

                }
                NotificationDetailsModel model = new NotificationDetailsModel
                {
                    Status = response.status,
                    Notification = response.notification.ToObject<Notification>(),
                    Message = response.message
                };
                Console.WriteLine(model.Status);
                Console.WriteLine(model.Message);
                Console.WriteLine(model.Notification);
                return View(model);
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetALlDetail = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                NotificationDetailsModel model1 = new NotificationDetailsModel
                {
                    Status = "",
                    Notification = null,
                    Message = ""
                };
                return View(model1);
            }
        }
        public async Task<dynamic> ReadNoti1fications([FromBody] GetNotificationDetailParameter param)
        {
            try
            {
                ViewBag.errorGetAll = "";
                var data = param;
                var postUrl = apiConfig.tenantReadNotifications;
                var response = await notificationRepo.ReadNotification(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorGetAll = response;
                    NotificationViewModel model1 = new NotificationViewModel();
                    return false;
                }
                Console.WriteLine(response);
                var dashboard = await GetDashboardData();
                NotificationReadModel resp = new NotificationReadModel
                {
                    Status = response.status,
                    Message = response.message
                };
                NotificationViewModel model = new NotificationViewModel();
                model.readNotifications = resp;
                Console.WriteLine(model.readNotifications.Status);
                Console.WriteLine(model.readNotifications.Message);
                //return true;
                return Json(new
                {
                    success = true,
                    unreadCount = dashboard?.UnreadNotifications?.ToString() ?? "0"
                });
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetAll = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return false;
            }
        }
        public async Task<GetDataRepose> GetDashboardData()
        {

            try
            {
                ViewBag.errorGetData = "";
                var data = new { };
                var postUrl = apiConfig.tenantDashboardGetData;
                var response = await dahboardRepo.GetData(data, postUrl, token ?? "");
                if (response is string)
                {
                    ViewBag.errorGetData = response;
                    GetDataRepose model1 = new GetDataRepose
                    {
                        Status = "",
                        Dashboard = null,
                        StatusCode = "",
                        Message = "",
                    };
                    return model1;
                }
                Console.WriteLine(response);

                GetDataRepose apiResponse = JsonConvert.DeserializeObject<GetDataRepose>(response.ToString());
                GetDataRepose model = new GetDataRepose
                {
                    Status = apiResponse.Status,
                    Dashboard = apiResponse.Dashboard,
                    StatusCode = apiResponse.StatusCode,
                    Message = apiResponse.Status,
                    UnreadNotifications = apiResponse.UnreadNotifications
                };
                HttpContext.Session.SetString("unReadNotification", $"{model.UnreadNotifications.ToString()}");
                var unReadNotification = HttpContext?.Session.GetString("unReadNotification");
                return model;
            }
            catch (HttpRequestException e)
            {
                ViewBag.errorGetData = e.Message;
                Console.WriteLine($"Request error: {e.Message}");
                return new GetDataRepose();
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}


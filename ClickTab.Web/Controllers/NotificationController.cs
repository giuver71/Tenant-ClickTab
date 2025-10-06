using ClickTab.Core.DAL.Models.NotificationCenter;
using ClickTab.Core.EntityService.NotificationCenter;
using ClickTab.Core.HelperService;
using ClickTab.Web.Mappings;
using ClickTab.Web.Mappings.ModelsDTO.NotificationCenter;
using ClickTab.Web.NotificationCenter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickTab.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private AutoMappingService _mappingService;
        private SessionService _sessionService;
        private NotificationService _notificationService;
        private NotificationDetailService _notificationDetailService;
        public NotificationController(AutoMappingService autoMappingService, SessionService sessionService, NotificationService notificationService, NotificationDetailService notificationDetailService)
        {
            _mappingService = autoMappingService;
            _sessionService = sessionService;
            _notificationService = notificationService;
            _notificationDetailService = notificationDetailService;
        }

        [HttpGet, Route("/api/[controller]/GetNotifications/{onlyUnread:bool}")]
        public async Task<IActionResult> GetNotifications(bool onlyUnread)
        {
            int currentUserID = _sessionService.SessionAvailable() ? _sessionService.GetCurrentSession().User.ID : 0;
            List<NotificationDetail> notificationDetails = _notificationDetailService.GetBy(n => n.FK_Receiver == currentUserID && (onlyUnread ? !n.ReadDate.HasValue : true), n => n.Notification);
            List<NotificationDetailDTO> notificationDTOs = _mappingService.CurrentMapper.Map<List<NotificationDetail>, List<NotificationDetailDTO>>(notificationDetails);
            return Ok(notificationDTOs.OrderByDescending(n => n.SendDate));
        }


        [HttpGet, Route("/api/[controller]/MarkAsRead/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            _notificationDetailService.MarkAsRead(id);
            return Ok(DateTime.Now);
        }

        [HttpPost, Route("/api/[controller]/MarkListAsRead")]
        public async Task<IActionResult> MarkListAsRead([FromBody] List<int> notificationIDs)
        {
            List<NotificationDetail> notificationDetails = _notificationDetailService.GetBy(n => notificationIDs.Contains(n.ID));
            notificationDetails.ForEach(notification =>
            {
                notification.ReadDate = DateTime.Now;
                _notificationDetailService.Save(notification);
            });
            return Ok(DateTime.Now);
        }

        [AllowAnonymous]
        [HttpGet, Route("/api/[controller]/CreateAndSendTestNotification")]
        public async Task<IActionResult> CreateAndSendTestNotification()
        {
            _notificationService.CreateAndSendTestNotification();
            return Ok();
        }
    }
}

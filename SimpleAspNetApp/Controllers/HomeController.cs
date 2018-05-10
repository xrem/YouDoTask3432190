using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SimpleAspNetApp.Models;
using SimpleAspNetApp.Services;

namespace SimpleAspNetApp.Controllers
{
    public class HomeController : Controller {
        private const string AuthCookieKey = "CD6BCB86-335E-42FD-A435-F2FBA6F213BE";
        private const int HttpStatusCodeNoContent = 204;
        
        [HttpGet]
        public ActionResult Index() {
            if (!HttpContext.Request.Cookies.AllKeys.Contains(AuthCookieKey)) {
                HttpContext.Response.Cookies.Add(new HttpCookie(AuthCookieKey, Guid.NewGuid().ToString("N")));
            }
            return View();
        }

        [HttpPost]
        public ActionResult PostMessage(string message) {
            var userCookie = HttpContext.Request.Cookies[AuthCookieKey];
            if (userCookie == null || string.IsNullOrWhiteSpace(userCookie.Value)) {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            if (string.IsNullOrWhiteSpace(message)) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessagesStorage.PutNewMessage(new UserMessage() {
                UserId = userCookie.Value,
                Message = message
            });
            return new HttpStatusCodeResult(HttpStatusCodeNoContent);
        }

        [HttpGet]
        public ActionResult GetMessage() {
            var userCookie = HttpContext.Request.Cookies[AuthCookieKey];
            if (userCookie == null || string.IsNullOrWhiteSpace(userCookie.Value)) {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return new JsonResult() {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new {
                    UserMessages = MessagesStorage.GetLastTenMessagesForUser(userCookie.Value),
                    AllMessages = MessagesStorage.GetLast20Messages()
                },
                ContentEncoding = Encoding.UTF8
            };
        }
    }
}
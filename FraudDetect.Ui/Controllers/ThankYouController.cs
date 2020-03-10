using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace FraudDetect.Ui.Controllers
{
    public class ThankYouController : Controller
    {
        IActionContextAccessor accessor;

        public ThankYouController(IActionContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public IActionResult Index(string payload)
        {
            var ip = accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            return View();
        }

        //public string Index()
        //{
        //    return "Thank you!";
        //}
    }
}
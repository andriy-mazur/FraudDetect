using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FraudDetect.Ui
{
    public class thankyouModel : PageModel
    {
        IActionContextAccessor accessor;

        public thankyouModel(IActionContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public void OnGet()
        {
            var ip = accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
        }

    }
}
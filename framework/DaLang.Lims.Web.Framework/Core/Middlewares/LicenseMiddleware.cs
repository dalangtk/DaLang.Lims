using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.Framework.Core.Exceptions;
using DaLang.Lims.Web.Framework.Core.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Core.Middlewares
{
    public class LicenseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LicenseValidator _validator;

        public LicenseMiddleware(RequestDelegate next, LicenseValidator validator)
        {
            _next = next;
            _validator = validator;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 放行激活接口
            //if (context.Request.Path.StartsWithSegments("/api/license/activate"))
            //{
            //    await _next(context);
            //    return;
            //}

            // 检查注册状态
            if (!AppInfo.IsRegistrationCodeValid)
            {
                //if (!_validator.IsValid)
                //{
                var machineCode = MachineHelper.GetMachineCode();
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync($"Service not registered, please register first!{machineCode}", Encoding.UTF8);
                return;
                //}
            }


            await _next(context);
        }
    }
}

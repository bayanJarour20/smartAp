using Elkood.Web.Helper.ExtensionMethods.ClaimsPrincipal;
using Elkood.Web.MVC.Security.Enum;
using Elkood.Web.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using SmartStart.SharedKernel.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartStart.Security
{
    public class ElAuthorizeDistributedAttribute : Elkood.Web.MVC.Security.Attribute.ElAuthorizeAttribute,  IAsyncAuthorizationFilter
    {
        public ElAuthorizeDistributedAttribute() : base() { }
        public ElAuthorizeDistributedAttribute(params string[] roles) : base(roles) { }
        public ElAuthorizeDistributedAttribute(params SmartStartRoles[] roles) : this(roles.Select(x => x.ToString()).ToArray()) { }
        public ElAuthorizeDistributedAttribute(params ElRoles[] roles) : base(roles) { }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
            Guid key = context.HttpContext.User.CurrentUserId<Guid>();

            var user_id_expier = await cache.GetStringAsync("smart_start_user_blacklist:" + key.ToString());

            if (user_id_expier != null)
            {
                if (user_id_expier == "-")
                {
                    context.Result = new UnauthorizedResult();
                }
                else
                {
                    if (DateTime.TryParse(user_id_expier, out DateTime date))
                    {
                        var generateDate = context.HttpContext.User.Claims.First(x => x.Type == ElClaimTypes.GenerateDate);
                        if (date > DateTime.Parse(generateDate.Value))
                            context.Result = new UnauthorizedResult();
                    }
                    else
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }
            }

            var temp_generation_stamp = await cache.GetStringAsync("smart_start_user_generation_stamp:" + key.ToString());
            if (temp_generation_stamp != null)
            {
                var generationStamp = context.HttpContext.User.Claims.First(x => x.Type == ElClaimTypes.GenerationStamp);
                if (temp_generation_stamp != generationStamp.Value)
                {
                    context.Result = new UnauthorizedResult();
                }
            }

            return;
        }
    }
}

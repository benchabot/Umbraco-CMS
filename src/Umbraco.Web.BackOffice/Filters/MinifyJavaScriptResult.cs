﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Umbraco.Core.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Core.Assets;
using Umbraco.Core.Runtime;
using Umbraco.Web.BackOffice.Controllers;

namespace Umbraco.Web.BackOffice.Filters
{
    public class MinifyJavaScriptResult : ActionFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // logic before action goes here
            var hostingEnvironment = context.HttpContext.RequestServices.GetService<IHostingEnvironment>();
            if (!hostingEnvironment.IsDebugMode)
            {
                var runtimeMinifier = context.HttpContext.RequestServices.GetService<IRuntimeMinifier>();

                if (context.Result is JavaScriptResult jsResult)
                {


                    var result = jsResult.Content;
                    var minified = await runtimeMinifier.MinifyAsync(result, AssetType.Javascript);
                    jsResult.Content = minified;
                }
            }





            await next(); // the actual action

            // logic after the action goes here
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);





            //minify the result

        }
    }
}

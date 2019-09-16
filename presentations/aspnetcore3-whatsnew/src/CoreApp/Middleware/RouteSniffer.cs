﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Middleware
{
    public class RouteSniffer
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RouteSniffer> logger;

        public RouteSniffer(RequestDelegate next, ILogger<RouteSniffer> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var feature = context.Features.Get<IEndpointFeature>();
            
            logger.LogInformation($"{feature.Endpoint.DisplayName}");

            var logMessage = feature.Endpoint.Metadata
                                    .Select(m => $"{m.GetType()}{Environment.NewLine}")
                                    .Aggregate(new StringBuilder(), (sb, s) => sb.Append(s), sb => sb.ToString());            
            logger.LogInformation(logMessage);            

            await next(context);
        }
    }
}

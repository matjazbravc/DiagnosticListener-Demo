using System;
using System.Diagnostics;
using DiagnosticListenerDemo.Services.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace DiagnosticListenerDemo.Services
{
    public class MyDiagnosticListenerHandler : DiagnosticListenerHandler
    {
        public MyDiagnosticListenerHandler(DiagnosticListener diagnosticListener) : base(diagnosticListener)
        {
        }

        protected override void OnHttpRequestStart(HttpContext httpContext)
        {
            Debug.Print($"Request started, path: {httpContext.Request.Path}{Environment.NewLine}");
        }

        protected override void OnHttpRequestStop(HttpContext httpContext, TimeSpan duration)
        {
            Debug.Print($"Request ended for {httpContext.Request.Path} in {duration.TotalSeconds} secs{Environment.NewLine}");
        }

        protected override void OnHttpRequestException(HttpContext httpContext, Exception exception)
        {
            Debug.Print($"Request exception {exception.Message}{Environment.NewLine}");
        }
    }
}

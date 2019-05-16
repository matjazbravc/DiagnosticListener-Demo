using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DiagnosticListenerDemo.Services.Diagnostics
{
    public abstract class DiagnosticListenerHandler : IHostedService, IDisposable, IObserver<KeyValuePair<string, object>>
    {
        private const string StartTimestampKey = "DiagnosticListener_StartTime";
        private readonly DiagnosticListener _diagnosticListener;
        private IDisposable _diagnosticubscription;

        protected DiagnosticListenerHandler(DiagnosticListener diagnosticListener)
        {
            _diagnosticListener = diagnosticListener;
        }

        public void Dispose()
        {
            _diagnosticubscription?.Dispose();
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            switch (value.Key)
            {
                case "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start":
                    {
                        var httpContext = value.Value.GetType().GetProperty("HttpContext")?.GetValue(value.Value) as HttpContext;
                        httpContext.Items[StartTimestampKey] = DateTime.Now.Ticks;
                        OnHttpRequestStart(httpContext);
                    }
                    break;
                case "Microsoft.AspNetCore.Hosting.HttpRequestIn.Stop":
                    {
                        var httpContext = value.Value.GetType().GetProperty("HttpContext")?.GetValue(value.Value) as HttpContext;
                        var startTimestamp = (long)httpContext.Items[StartTimestampKey];
                        var endTimestamp = DateTime.Now.Ticks;
                        var duration = new TimeSpan((long)((endTimestamp - startTimestamp) * TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency));
                        OnHttpRequestStop(httpContext, duration);
                    }
                    break;
                case "Microsoft.AspNetCore.Hosting.UnhandledException":
                case "Microsoft.AspNetCore.Diagnostics.UnhandledException":
                case "Microsoft.AspNetCore.Diagnostics.HandledException":
                    {
                        var httpContext = value.Value.GetType().GetProperty("HttpContext")?.GetValue(value.Value) as HttpContext;
                        var exception = value.Value.GetType().GetProperty("exception")?.GetValue(value.Value) as Exception;
                        OnHttpRequestException(httpContext, exception);
                    }
                    break;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _diagnosticubscription = _diagnosticListener.Subscribe(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual void OnHttpRequestException(HttpContext httpContext, Exception exception)
        {
        }

        protected virtual void OnHttpRequestStart(HttpContext httpContext)
        {
        }

        protected virtual void OnHttpRequestStop(HttpContext httpContext, TimeSpan duration)
        {
        }
    }
}
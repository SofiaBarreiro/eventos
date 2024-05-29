using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosCeremonial
{
    //public class WebServiceLogger : ILogger
    //{
    //    protected private int myVar;

    //    public readonly WebServiceLoggerProvider ServiceProvider;

    //    public WebServiceLogger (WebServiceLoggerProvider service) {

    //        ServiceProvider = service;
        
    //    }

    //    public IDisposable BeginScope<TState>(TState state)
    //    {
    //        return null;
    //    }

    //    public bool IsEnabled(LogLevel logLevel)
    //    {
    //        return logLevel != LogLevel.None;

    //    }

    //    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    //    {

    //        string Logdata = DateTime.Now.ToString();
    //        string dataException = exception != null ? exception.Message : "";


    //        EventosCeremonial.Shared. login = new EventosCeremonial.Shared.LoginDisplay
    //        {

    //            Data = $"{Logdata} {dataException}",
    //            Level = logLevel.ToString()
    //        };
            


    //        }


        
    //}
}

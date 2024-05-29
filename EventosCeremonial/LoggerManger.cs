using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net.Core;
using log4net.Repository.Hierarchy;
using log4net.Layout;
using log4net.Appender;
using System.Text;

namespace EventosCeremonial
{
    public class LoggerManger : IloggerManager
    {

        private readonly ILog _logger = LogManager.GetLogger(typeof(LoggerManager));
        public LoggerManger()
        {
            try
            {
                    
                    XmlDocument log4netConfig = new XmlDocument();
                    using (var fs = File.OpenRead("log4net.config"))
                    {
                        log4netConfig.Load(fs);
                        var repo = LogManager.CreateRepository(
                                Assembly.GetEntryAssembly(),
                                typeof(log4net.Repository.Hierarchy.Hierarchy));
                        string ruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                        GlobalContext.Properties["FilePath"] = ruta.Substring(6, ruta.Length - 6);
                        XmlConfigurator.Configure(repo, log4netConfig["log4net"]);


                    }
                
             

                //Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository("");
                //hierarchy.Root.RemoveAllAppenders();                            // Clear all previously added repositories.
                //hierarchy.Root.Level = Level.Info;                             // Set Log level
                //PatternLayout layout = new PatternLayout() { ConversionPattern = "%d{yyyy-MM-dd HH:mm:ss.fff} %4t %-5p %m%n" }; // Log line format: Include millisecond precision, thread ID, Log type,
                //layout.ActivateOptions();                                       // Apply Configuration 

                //RollingFileAppender RFA = new RollingFileAppender();
                //RFA.Name = "RollingLogFileAppender";                                       // Set name of appender
                //RFA.File = @"Logs\logs" + "yyyy-MM-dd.'txt'";                 // Set file name prefix
                //RFA.LockingModel = new FileAppender.MinimalLock();              // Minimum lock time required, makes file available for reading
                //RFA.AppendToFile = true;                                        // Do not overwrite existing logs, append to them.
                //RFA.DatePattern = ".yyyy.MM.dd'.log'";                          // Add file extension here, to preserve the file extension
                //RFA.Encoding = Encoding.UTF8;                                   // Set format of file to UTF8 for international characters.
                //RFA.CountDirection = 1;                                         // Increment file name in bigger number is newest, instead of default backward.
                //RFA.MaximumFileSize = "100MB";                                  // Maximum size of file that I could open with common notepad applications
                //RFA.RollingStyle = RollingFileAppender.RollingMode.Composite;   // Increment file names by both size and date.
                //RFA.StaticLogFileName = false;
                //RFA.MaxSizeRollBackups = -1;                                    // Keep all log files, do not automatically delete any
                //RFA.PreserveLogFileNameExtension = true;                        // This plus extension added to DatePattern, causes to rolling size also work correctly

                //RFA.Layout = layout;
                //RFA.ActivateOptions();                                          // Apply Configuration 

                //hierarchy.Root.AddAppender(RFA);
                //BasicConfigurator.Configure(hierarchy);
            }
            catch (Exception ex)
            {
                _logger.Error("Error", ex);
            }
        }


       

        public void LogInformation(string message)
        {
            _logger.Info(message);
        }
        public void LogInformation(string message, Exception ex)
        {
            _logger.Info(message, ex);
        }
        public void LogAdvertencia(string message)
        {
            _logger.Warn(message);
        }
        public void LogAdvertencia(string message, Exception ex)
        {
            _logger.Warn(message, ex);
        }
        public void LogError(string message)
        {
            _logger.Error(message);
        }
        public void LogError(string message, Exception ex)
        {
            _logger.Error(message, ex);
        }
    }
}

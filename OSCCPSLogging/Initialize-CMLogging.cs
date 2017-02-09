using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Log4Net_CMTrace;
using System.IO;
using System.Reflection;


//Todo override console level
namespace OSCCPSLogging
{
//Todo add properties
//Todo Paramater validation
    [Cmdlet(VerbsData.Initialize,"CMLogging")]
    public class OSCCPSLogging : PSCmdlet
     {

        private string _fileLogLevel="Info";
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 2,
        HelpMessage = "Specify a loglevel, supported values are off, info, warn, error, debug.")]

        public string fileLogLevel
        {
            get { return _fileLogLevel; }
            set { _fileLogLevel = value; }
        }

        private string _logFileName;
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 0,
        HelpMessage = "Specify the full path including filename to a cmtrace formatted logging file.")]

        public string LogFileName
        {
            get { return _logFileName; }
            set { _logFileName = value; }
        }


        private string _logConfigFile = string.Concat(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"\logging.config");
        [Parameter(
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
            HelpMessage = "Specify the full path to a Log4Net formatted logging configuration file.")]
        
        public string LogConfigFile
        {
            get { return _logConfigFile; }
            set { _logConfigFile = value;}
        }

       //protected override void BeginProcessing()
       // {
       //     base.BeginProcessing();
       // }

        protected override void ProcessRecord()
        {
            //Console.WriteLine(_logConfigFile);
            log4net.ILog logger = log4net.LogManager.GetLogger("Powershell");
            log4net.Appender.IAppender[] appenders = log4net.LogManager.GetRepository().GetAppenders();

            System.IO.FileInfo logFileInfo = new System.IO.FileInfo(_logConfigFile);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logFileInfo);
            WriteObject(logger);

            foreach (log4net.Appender.IAppender logAppender in appenders)
            {
                if (logAppender.GetType() == typeof(log4net.Appender.RollingFileAppender))
                {
                    log4net.Appender.FileAppender fileAppender = (log4net.Appender.RollingFileAppender)logAppender;
                    //switch 
                    
                if (MyInvocation.BoundParameters.ContainsKey("LogFileName"))
                {
                    fileAppender.File = LogFileName;
                    
                }
                if (MyInvocation.BoundParameters.ContainsKey("FileLogLevel"))
                {
                    switch (fileLogLevel.ToLower())
                    {
                            case "off":     fileAppender.Threshold = log4net.Core.Level.Off; break;
                            case "info":    fileAppender.Threshold = log4net.Core.Level.Info; break;
                            case "warn":    fileAppender.Threshold = log4net.Core.Level.Warn; break;
                            case "error":   fileAppender.Threshold = log4net.Core.Level.Error; break;
                            case "debug":   fileAppender.Threshold = log4net.Core.Level.Debug; break;
                            default:        fileAppender.Threshold = log4net.Core.Level.Info; break;
                    }
                }
                fileAppender.ActivateOptions();
                }
            }
        }
    }
}

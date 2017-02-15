using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Log4Net_CMTrace;
using System.IO;
using System.Reflection;
using System.Diagnostics;



namespace OSCCPSLogging
{

    [Cmdlet(VerbsData.Out, "LogInfo")]
    public class OSCCPSLogInfo : PSCmdlet
    {
        private log4net.ILog _logObject;
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 0,
        HelpMessage = @"Specify a logger object as created by the Initialize-CMLogging Cmdlet")]

        public log4net.ILog LogObject
        {
            get { return _logObject; }
            set { _logObject = value; }
        }
        private string _logMessage;
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 1,
        HelpMessage = @"Specify the message to log")]

        public string LogMessage
        {
            get { return _logMessage; }
            set { _logMessage = value; }
        }
        protected override void ProcessRecord()
        {
            _logObject.Logger.Repository.
            _logObject.Info(_logMessage);
        }
    }

    //Todo add properties
    //Todo Paramater validation

    [Cmdlet(VerbsData.Out, "LogWarn")]
    public class OSCCPSLogWarn : PSCmdlet
    {
        private log4net.ILog _logObject;
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 0,
        HelpMessage = @"Specify a logger object as created by the Initialize-CMLogging Cmdlet")]

        public log4net.ILog LogObject
        {
            get { return _logObject; }
            set { _logObject = value; }
        }
        private string _logMessage;
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 1,
        HelpMessage = @"Specify the message to log")]

        public string LogMessage
        {
            get { return _logMessage; }
            set { _logMessage = value; }
        }
        protected override void ProcessRecord()
        {
            _logObject.Warn(_logMessage);
        }
    }

    [Cmdlet(VerbsData.Out, "LogError")]
    public class OSCCPSLogError : PSCmdlet
    {
        private log4net.ILog _logObject;
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 0,
        HelpMessage = @"Specify a logger object as created by the Initialize-CMLogging Cmdlet")]

        public log4net.ILog LogObject
        {
            get { return _logObject; }
            set { _logObject = value; }
        }
        private string _logMessage;
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 1,
        HelpMessage = @"Specify the message to log")]

        public string LogMessage
        {
            get { return _logMessage; }
            set { _logMessage = value; }
        }
        protected override void ProcessRecord()
        {
            _logObject.Error(_logMessage);
        }
    }

    [Cmdlet(VerbsData.Out, "LogDebug")]
    public class OSCCPSLogDebug : PSCmdlet
    {
        private log4net.ILog _logObject;
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 0,
        HelpMessage = @"Specify a logger object as created by the Initialize-CMLogging Cmdlet")]

        public log4net.ILog LogObject
        {
            get { return _logObject; }
            set { _logObject = value; }
        }
        private string _logMessage;
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 1,
        HelpMessage = @"Specify the message to log")]

        public string LogMessage
        {
            get { return _logMessage; }
            set { _logMessage = value; }
        }
        protected override void ProcessRecord()
        {
            _logObject.Debug(_logMessage);
        }
    }
    [Cmdlet(VerbsData.Initialize,"CMLogging")]
    public class OSCCPSLogging : PSCmdlet
     {
        private string _logFileName;
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 0,
        HelpMessage = @"Specify the full path including filename to a cmtrace formatted log file to be created or appended to. Default is $env:temp\OSCCPSLogging.log")]

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
            HelpMessage = "Specify the full path to a Log4Net formatted logging configuration file. Default file is logging.config in the OSCCPSLogging Powershell module directory")]
        
        public string LogConfigFile
        {
            get { return _logConfigFile; }
            set { _logConfigFile = value;}
        }
        private string _fileLogLevel = "Info";
        [ValidateSet("off", "info", "warn","error","debug", IgnoreCase = true)]
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 2,
        HelpMessage = "Specify a loglevel for the logfile, supported values are off, info, warn, error, debug.")]

        public string FileLogLevel
        {
            get { return _fileLogLevel; }
            set { _fileLogLevel = value; }
        }

        private string _consoleLogLevel="Info";
        [ValidateSet("off", "info", "warn", "error", "debug", IgnoreCase = true)]
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 3,
        HelpMessage = "Specify a loglevel for the console, supported values are off, info, warn, error, debug.")]
        public string ConsoleLogLevel
        {
            get { return _consoleLogLevel; }
            set { _consoleLogLevel = value; }
        }
        private bool _multiColor = true;
        [Parameter(
        Mandatory = false,
        ValueFromPipeline = true,
        ValueFromPipelineByPropertyName = true,
        Position = 4,
        HelpMessage = "Specify whether you want the multi-colored or single color logging style, Default value is multi-colored. Note: Windows PowerShell ISE Host does not support multi-colors")]
        public bool MultiColor
        {
            get { return _multiColor; }
            set { _multiColor = value; }
        }

        protected override void ProcessRecord()
        {
            //todo set default to scriptname.log
            string test = MyInvocation.ScriptName;

            log4net.ILog logger = log4net.LogManager.GetLogger("Powershell");
            System.IO.FileInfo logFileInfo = new System.IO.FileInfo(_logConfigFile);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logFileInfo);
            WriteObject(logger);

            log4net.Appender.IAppender[] appenders = log4net.LogManager.GetRepository().GetAppenders();
            foreach (log4net.Appender.IAppender logAppender in appenders)
            {
                #region RollingFilleAppender
                if (logAppender.GetType() == typeof(log4net.Appender.RollingFileAppender))
                {
                    log4net.Appender.FileAppender fileAppender = (log4net.Appender.RollingFileAppender)logAppender;
                    if (MyInvocation.BoundParameters.ContainsKey("LogFileName"))
                    {

                        log4net.Util.PatternString dynamicFileName = new log4net.Util.PatternString(LogFileName);
                        log4net.Util.ConverterInfo adminuiLogConverterInfo = new log4net.Util.ConverterInfo();
                        adminuiLogConverterInfo.Name = "adminuilog";
                        adminuiLogConverterInfo.Type = typeof(Log4Net_CMTrace.CMAdminUILogFolderPatternConverter);
                        log4net.Util.ConverterInfo ccmLogConverterInfo = new log4net.Util.ConverterInfo();
                        ccmLogConverterInfo.Name = "ccmlog";
                        ccmLogConverterInfo.Type = typeof(Log4Net_CMTrace.CMClientLogFolderPatternConverter);
                        log4net.Layout.PatternLayout newLayout = new log4net.Layout.PatternLayout();
                        dynamicFileName.AddConverter(adminuiLogConverterInfo);
                        dynamicFileName.AddConverter(ccmLogConverterInfo);
                        dynamicFileName.ActivateOptions();
                        //fileAppender.File = LogFileName;
                        fileAppender.File = dynamicFileName.Format();
                    }
                    if (MyInvocation.BoundParameters.ContainsKey("FileLogLevel"))
                    {
                        switch (FileLogLevel.ToLower())
                        {
                            case "off": fileAppender.Threshold = log4net.Core.Level.Off; break;
                            case "info": fileAppender.Threshold = log4net.Core.Level.Info; break;
                            case "warn": fileAppender.Threshold = log4net.Core.Level.Warn; break;
                            case "error": fileAppender.Threshold = log4net.Core.Level.Error; break;
                            case "debug": fileAppender.Threshold = log4net.Core.Level.Debug; break;
                            default: fileAppender.Threshold = log4net.Core.Level.Info; break;
                        }
                    }
                    else
                    {
                        fileAppender.Threshold = log4net.Core.Level.Info;
                    }
                    fileAppender.ActivateOptions();
                }
                #endregion
                #region ConsoleAppender
                if (logAppender.GetType() == typeof(log4net.Appender.ConsoleAppender))
                {
                    log4net.Appender.ConsoleAppender consoleAppender = (log4net.Appender.ConsoleAppender)logAppender;
                    if (this.Host.Name.ToLower() == "consolehost")
                    {
                        if (MultiColor == false)
                        {
                            {
                                if (MyInvocation.BoundParameters.ContainsKey("ConsoleLogLevel"))
                                {
                                    switch (ConsoleLogLevel.ToLower())
                                    {
                                        case "off": consoleAppender.Threshold = log4net.Core.Level.Off; break;
                                        case "info": consoleAppender.Threshold = log4net.Core.Level.Info; break;
                                        case "warn": consoleAppender.Threshold = log4net.Core.Level.Warn; break;
                                        case "error": consoleAppender.Threshold = log4net.Core.Level.Error; break;
                                        case "debug": consoleAppender.Threshold = log4net.Core.Level.Debug; break;
                                        default: consoleAppender.Threshold = log4net.Core.Level.Info; break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            consoleAppender.Threshold = log4net.Core.Level.Off;
                        }
                    }
                    else
                    {
                        if (this.Host.Name.ToLower() == "windows powershell ise host")
                        {
                            if (MyInvocation.BoundParameters.ContainsKey("ConsoleLogLevel"))
                            {
                                switch (ConsoleLogLevel.ToLower())
                                {
                                    case "off": consoleAppender.Threshold = log4net.Core.Level.Off; break;
                                    case "info": consoleAppender.Threshold = log4net.Core.Level.Info; break;
                                    case "warn": consoleAppender.Threshold = log4net.Core.Level.Warn; break;
                                    case "error": consoleAppender.Threshold = log4net.Core.Level.Error; break;
                                    case "debug": consoleAppender.Threshold = log4net.Core.Level.Debug; break;
                                    default: consoleAppender.Threshold = log4net.Core.Level.Info; break;
                                }
                            }
                            if ((MyInvocation.BoundParameters.ContainsKey("MultiColor"))  && (MultiColor == true)) 
                            {
                                Console.WriteLine("Powershell ISe does not support multi colored displays.");
                            }
                        }
                    }
                    consoleAppender.ActivateOptions();
                }
                #endregion
                #region coloredconsoleappender
                if (logAppender.GetType() == typeof(log4net.Appender.ColoredConsoleAppender))
                {
                    log4net.Appender.ColoredConsoleAppender coloredConsoleAppender = (log4net.Appender.ColoredConsoleAppender)logAppender;
                    if (this.Host.Name.ToLower() == "consolehost")
                    {
                        if (MultiColor == true)
                        {
                            if (MyInvocation.BoundParameters.ContainsKey("ConsoleLogLevel"))
                            {
                                switch (ConsoleLogLevel.ToLower())
                                {
                                    case "off": coloredConsoleAppender.Threshold = log4net.Core.Level.Off; break;
                                    case "info": coloredConsoleAppender.Threshold = log4net.Core.Level.Info; break;
                                    case "warn": coloredConsoleAppender.Threshold = log4net.Core.Level.Warn; break;
                                    case "error": coloredConsoleAppender.Threshold = log4net.Core.Level.Error; break;
                                    case "debug": coloredConsoleAppender.Threshold = log4net.Core.Level.Debug; break;
                                    default: coloredConsoleAppender.Threshold = log4net.Core.Level.Info; break;
                                }
                            }
                        }
                        else
                        {
                            coloredConsoleAppender.Threshold = log4net.Core.Level.Off;
                        }
                    }
                    if (this.Host.Name.ToLower() == "windows powershell ise host")
                    {
                        coloredConsoleAppender.Threshold = log4net.Core.Level.Off;
                    }
                    coloredConsoleAppender.ActivateOptions();
                }
                #endregion
            }
        }
    }
}

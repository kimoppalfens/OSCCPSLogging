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

//TODO  if host is ISE disable Colored Console Appender & enable Console appender for Info by default
//todo if host is PSHost default colored console appender, set variable to disable colored console

//console appender config file = x
//colored console appender config file = x

//ConsoleAppender
//done
//if host is ise
// if consoleloglevel is set set to value
// if consoleloglevel not set keep value => do nothing

//done
// if host is console host
// if multicolored is false
//    if consoleloglevel is set set to value
//    if consoleloglevel is not set => do nothing
// if multicolored is true
//  set threshold to off

//coloredconsole appender
// if  host is ise
//   if multicolored specified
//     writeline multicolored not supported
//   if multicolored not specified
//     set threshold to off

//done
// if host is console
//  if multicolored = false
//     set threshold to off
// if multicolored = true
//   if consoleloglevel is set set to value
//  if consoleloglevel not set => do nothing

namespace OSCCPSLogging
{
//Todo add properties
//Todo Paramater validation
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

        //protected override void BeginProcessing()
        //{
        //    base.BeginProcessing();
        //    Log4Net_CMTrace.NumericLevelPatternConverter s = new Log4Net_CMTrace.NumericLevelPatternConverter();
        //}

        protected override void ProcessRecord()
        {
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
                        fileAppender.File = LogFileName;
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

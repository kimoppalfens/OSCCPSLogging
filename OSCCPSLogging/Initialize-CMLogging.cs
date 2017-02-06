using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Log4Net_CMTrace;


namespace OSCCPSLogging
{
//Todo add properties
    [Cmdlet(VerbsData.Initialize,"CMLogging")]
    public class OSCCPSLogging : PSCmdlet
     {
        
        log4net.ILog logger;
        private string _logConfigFile;

        [Parameter(
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 0,
            HelpMessage = "Specify the full path to a Log4Net formatted logging configuration file.")]
        public string LogConfigFile
        {
            get { return _logConfigFile; }
            set { _logConfigFile = value; }
        }


       protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Log4Net_CMTrace.UTCOffsetPatternConverter utc = new Log4Net_CMTrace.UTCOffsetPatternConverter();
            
        }

        protected override void ProcessRecord()
        {
            //base.ProcessRecord();
            //log4net.LogManager logManager;

            logger = log4net.LogManager.GetLogger("Powershell");
            System.IO.FileInfo logFileInfo = new System.IO.FileInfo(_logConfigFile);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logFileInfo);
            WriteObject(logger);

            log4net.Appender.RollingFileAppender  fileAppender = (log4net.Appender.RollingFileAppender)log4net.LogManager.GetRepository().GetAppenders()[1];
            fileAppender.ActivateOptions();
            //string logFileName = 
        }

        //protected override void EndProcessing()
        //{
        //    base.EndProcessing();
        //}

    }
}

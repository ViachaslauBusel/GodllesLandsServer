using Cmd.Terminal.Debugger.Monitoring;
using Cmd.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;

namespace NetworkGameEngine.Debugger
{
    public static class Debug
    {
        public static NLog.Logger Log = LogManager.GetCurrentClassLogger();
        public static Telemetry Telemetry { get; } = new Telemetry();

        static Debug()
        {
            LogManager.Configuration = new XmlLoggingConfiguration("NLog.config");
            //  XmlConfigurator.Configure(new System.IO.FileInfo(@"log4net.config"));

            //Log = new LoggerConfiguration()
            //   .MinimumLevel.Information()
            //   .WriteTo.File("Logs/FULL.log", rollingInterval: RollingInterval.Day)
            //   .MinimumLevel.Debug()
            //   .WriteTo.TerminalAppender()
            //   .CreateLogger();

            TelemetryCommand command = new TelemetryCommand(Telemetry);
            command.FileName = $"Telemetry-VER-{1}";
            Terminal.AddCommand(command);
        }
    }
}

using Cmd.Terminal.Debugger.Logger;
using Cmd.Terminal;
using NLog.Targets;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGameEngine.Debugger
{
    [Target("TerminalAppender")]
    public class TerminalAppender : TargetWithLayout
    {
        //  private readonly IFormatProvider m_formatProvider;
        private TerminalLogger m_terminalLogger = new TerminalLogger();
        public TerminalAppender()
        {
            //    m_formatProvider = formatProvider;
            m_terminalLogger.Format = "%message";
            Terminal.AddCommand(new TerminalLoggerCommand(m_terminalLogger));
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var message = base.RenderLogEvent(base.Layout, logEvent);

            if (LogLevel.Debug == logEvent.Level) { m_terminalLogger.Debug(message); }
            else if (LogLevel.Info == logEvent.Level) { m_terminalLogger.Info(message); }
            else if (LogLevel.Warn == logEvent.Level) { m_terminalLogger.Warn(message); }
            else if (LogLevel.Error == logEvent.Level) { m_terminalLogger.Error(message); }
            else if (LogLevel.Fatal == logEvent.Level) { m_terminalLogger.Fatal(message); }
            else { m_terminalLogger.Info(message); }
        }



        //protected override void Append(LoggingEvent loggingEvent)
        //{

        //    if (Level.Debug == loggingEvent.Level) { m_terminalLogger.Debug(base.RenderLoggingEvent(loggingEvent)); }
        //    else if (Level.Info == loggingEvent.Level) { m_terminalLogger.Info(base.RenderLoggingEvent(loggingEvent)); }
        //    else if (Level.Warn == loggingEvent.Level) { m_terminalLogger.Warn(base.RenderLoggingEvent(loggingEvent)); }
        //    else if (Level.Error == loggingEvent.Level) { m_terminalLogger.Error(base.RenderLoggingEvent(loggingEvent)); }
        //    else if (Level.Fatal == loggingEvent.Level) { m_terminalLogger.Fatal(base.RenderLoggingEvent(loggingEvent)); }
        //    else  { m_terminalLogger.Info(base.RenderLoggingEvent(loggingEvent)); }
        //}
    }
}

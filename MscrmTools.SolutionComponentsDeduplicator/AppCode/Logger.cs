using System;

namespace MscrmTools.SolutionComponentsDeduplicator.AppCode
{
    public class Logger
    {
        public event EventHandler<LogEventArgs> OnLog;

        public void Log(string message)
        {
            OnLog?.Invoke(this, new LogEventArgs(message));
        }
    }
}
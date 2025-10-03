using System;

namespace MscrmTools.SolutionComponentsDeduplicator.AppCode
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
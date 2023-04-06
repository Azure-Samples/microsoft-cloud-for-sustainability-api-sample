using System;

namespace Azure.CfS.Library.Interfaces
{
    internal interface ILoggerAdapter<TType>
    {
        void LogError(Exception exception, string message);
        void LogError(string message);
    }
}

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Boxsie.Wrapplication.Logging
{
    public interface IBxLogger
    {
        void WriteLine(string message, LogLevel lvl = LogLevel.Debug);
    }
}
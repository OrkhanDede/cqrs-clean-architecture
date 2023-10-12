using System;
using Serilog;

namespace Core.Extensions
{
    public static class LogExtension
    {
        public static void LogException(this ILogger logger, Exception ex ,string title="ERROR!")
        {
            var errorId = Guid.NewGuid();
            //if error then continue(fetching from db)

            logger.ForContext("Type", "Error")
                .ForContext("ErrorTitle", title)
                .ForContext("StackTrace", ex.StackTrace)
                .ForContext("Exception", ex, true)
                .Error(ex, ex.Message + ". {@errorId}", errorId);

        }

    }
}

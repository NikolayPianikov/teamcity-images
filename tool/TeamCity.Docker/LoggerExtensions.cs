using System;
using System.Text;
using IoC;

namespace TeamCity.Docker
{
    internal static class LoggerExtensions
    {
        public static void Log([NotNull] this ILogger logger, [NotNull] Exception error, [NotNull] string message = "")
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            var sb = new StringBuilder();
            var tab = 0;
            if (!string.IsNullOrWhiteSpace(message))
            {
                sb.AppendLine(message);
                tab = 2;
            }

            do
            {
                sb.AppendLine($"{new string(' ', tab)}{error.Message} from {error.Source}");
                error = error.InnerException;
                tab += 2;
            } while (error != null);

            logger.Log(sb.ToString(), Result.Error);
        }
    }
}

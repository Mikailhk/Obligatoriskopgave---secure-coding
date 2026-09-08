namespace SecureCodingWebshop.Services
{
    public class LogService
    {
        private readonly string _logFile = "logs.txt";

        public void Log(string message)
        {
            string logMessage =
                $"{DateTime.Now}: {message}{Environment.NewLine}";

            File.AppendAllText(_logFile, logMessage);
        }
    }
}
namespace AlgebraImageApp.Patterns;
public sealed class CustomLogger
{
    private static readonly CustomLogger instance = new CustomLogger();
    private static readonly object lockObject = new object();
    private string logFilePath;


    private CustomLogger()
    {
        logFilePath = "log.txt"; // Specify the desired log file path here

    }

    public static CustomLogger Instance
    {
        get { return instance; }
    }

    public void Log(string message)
    {
        lock (lockObject) //ensure thread safety when multiple threads attempt to log concurrently
        {
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"[{currentTime}] {message}";
            Console.WriteLine(logEntry);
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(logEntry);
            }
        }
    }
}

namespace AlgebraImageApp.Logger;

using System;
using System.IO;

public class LoggerSingleton
{
    private static LoggerSingleton instance;
    private readonly string logFilePath;
    private readonly object lockObj = new object();
    
    private LoggerSingleton()
    {
        // Set the log file path
        string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
        logFilePath = Path.Combine(logDirectory, "app.log");
    }
    
    public static LoggerSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoggerSingleton();
            }
            
            return instance;
        }
    }
    
    public void Log(string message)
    {
        string formattedMessage = $"[{DateTime.Now}] {message}";

        // Log to console
        Console.WriteLine(formattedMessage);
        Console.WriteLine(logFilePath);
        
        
        // Log to file
        lock (lockObj)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(formattedMessage);
            }
        }
    }
}
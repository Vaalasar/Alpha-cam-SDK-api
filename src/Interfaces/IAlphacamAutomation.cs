using System;
using System.Collections.Generic;

namespace AlphacamSDK.Interfaces
{
    public interface IAlphacamAutomation
    {
        // Управление макросами
        bool ExecuteMacro(string macroPath, Dictionary<string, object> parameters = null);
        bool RecordMacro(string savePath);
        bool StopRecording();
        bool EditMacro(string macroPath);
        
        // Пакетная обработка
        bool StartBatch(BatchParameters parameters);
        bool StopBatch();
        BatchStatus GetBatchStatus();
        List<BatchResult> GetBatchResults();
        
        // Управление очередью
        int AddToQueue(QueueItem item);
        bool RemoveFromQueue(int itemId);
        bool ModifyQueueItem(int itemId, QueueItem newParameters);
        List<QueueItem> GetQueue();
        bool ClearQueue();
        
        // Автоматизация процессов
        bool StartProcess(ProcessParameters parameters);
        bool StopProcess();
        bool PauseProcess();
        bool ResumeProcess();
        ProcessStatus GetProcessStatus();
        
        // Настройка автоматизации
        bool ConfigureAutomation(AutomationConfig config);
        AutomationConfig GetCurrentConfig();
        bool SaveConfig(string path);
        bool LoadConfig(string path);
        
        // События и уведомления
        event EventHandler<AutomationEventArgs> OnProcessStarted;
        event EventHandler<AutomationEventArgs> OnProcessCompleted;
        event EventHandler<AutomationEventArgs> OnError;
        event EventHandler<AutomationEventArgs> OnStatusChanged;
        
        // Логирование и отчеты
        bool EnableLogging(string logPath);
        bool DisableLogging();
        List<LogEntry> GetLogs(DateTime startDate, DateTime endDate);
        bool GenerateReport(ReportParameters parameters);
    }

    public class BatchParameters
    {
        public List<string> FilePaths { get; set; }
        public string OutputDirectory { get; set; }
        public Dictionary<string, object> ProcessingParameters { get; set; }
        public bool ContinueOnError { get; set; }
        public int MaxParallelProcesses { get; set; }
        public bool CreateBackups { get; set; }
        public string LogPath { get; set; }
    }

    public class BatchStatus
    {
        public bool IsRunning { get; set; }
        public int TotalItems { get; set; }
        public int ProcessedItems { get; set; }
        public int FailedItems { get; set; }
        public string CurrentItem { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public Dictionary<string, string> ErrorLog { get; set; }
    }

    public class BatchResult
    {
        public string FilePath { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public TimeSpan ProcessingTime { get; set; }
        public Dictionary<string, object> Results { get; set; }
    }

    public class QueueItem
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public QueueItemStatus Status { get; set; }
        public DateTime ScheduledTime { get; set; }
        public int Priority { get; set; }
        public int RetryCount { get; set; }
        public TimeSpan Timeout { get; set; }
    }

    public enum QueueItemStatus
    {
        Pending,
        Scheduled,
        Processing,
        Completed,
        Failed,
        Cancelled,
        Paused
    }

    public class ProcessParameters
    {
        public string ProcessType { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public bool IsAsync { get; set; }
        public TimeSpan Timeout { get; set; }
        public int RetryAttempts { get; set; }
    }

    public class ProcessStatus
    {
        public bool IsRunning { get; set; }
        public int Progress { get; set; }
        public string CurrentOperation { get; set; }
        public string StatusMessage { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan EstimatedTimeRemaining { get; set; }
    }

    public class AutomationConfig
    {
        public bool EnableLogging { get; set; }
        public string LogDirectory { get; set; }
        public int MaxConcurrentProcesses { get; set; }
        public int RetryAttempts { get; set; }
        public TimeSpan RetryDelay { get; set; }
        public bool CreateBackups { get; set; }
        public string BackupDirectory { get; set; }
        public Dictionary<string, object> CustomSettings { get; set; }
    }

    public class AutomationEventArgs : EventArgs
    {
        public string ProcessId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }
        public Exception Error { get; set; }
    }

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string ProcessId { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }

    public class ReportParameters
    {
        public string ReportType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string OutputFormat { get; set; }
        public string OutputPath { get; set; }
        public Dictionary<string, object> FilterParameters { get; set; }
    }
}

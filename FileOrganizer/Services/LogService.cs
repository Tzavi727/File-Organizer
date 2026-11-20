using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FileOrganizer.Configuration;
using FileOrganizer.Interfaces;

namespace FileOrganizer.Services;

internal class LogService : ILogService
{
    private readonly string _logFilePath = AppPaths.LogFilePath;

    public record FileRecord
    {
        public required string FileName { get; set; }
        public required string OldPath { get; set; }
        public required string NewPath { get; set; }
    }

    public record SessionLog
    {
        public required string SessionId { get; set; }
        public required string Timestamp { get; set; }
        public required List<FileRecord> FilesMoved { get; set; }
    }

    private List<FileRecord> _sessionBuffer = new();
    private List<SessionLog> _fullLog = new();

    public void RecordMove(string fileName, string oldPath, string newPath)
    {
        _sessionBuffer.Add(new FileRecord
        {
            FileName = fileName,
            OldPath = oldPath,
            NewPath = newPath
        });
    }

    public void CommitSession()
    {
        if (_sessionBuffer.Count == 0) return;

        _fullLog.Add(new SessionLog
        {
            SessionId = Guid.NewGuid().ToString().Substring(0, 8),
            Timestamp = DateTime.Now.ToString("g"),
            FilesMoved = new List<FileRecord>(_sessionBuffer)
        });

        _sessionBuffer.Clear();
    }


    public void SaveLog()
    {
        List<SessionLog> fullLog = new();

        if (File.Exists(_logFilePath))
        {
            try
            {
                string jsonText = File.ReadAllText(_logFilePath);
                fullLog = JsonSerializer.Deserialize<List<SessionLog>>(jsonText) ?? new List<SessionLog>();
            }
            catch (Exception)
            {
                fullLog = new List<SessionLog>();
            }
        }

        fullLog.AddRange(_fullLog);

        var options = new JsonSerializerOptions { WriteIndented = true };
        string newJson = JsonSerializer.Serialize(fullLog, options);
        AppPaths.EnsureParentDirectory(_logFilePath);
        File.WriteAllText(_logFilePath, newJson);

        _fullLog.Clear();
    }
}
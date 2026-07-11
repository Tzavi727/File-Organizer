using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FileOrganizer.Services
{
    internal class LogService
    {
        public class FileRecord
        {
            public required string FileName { get; set; }
            public required string OldPath { get; set; }
            public required string NewPath { get; set; }
        }

        public class SessionLog
        {
            public required string SessionId { get; set; }
            public required string Timestamp { get; set; }
            public required List<FileRecord> FilesMoved { get; set; }
        }

        private static List<FileRecord> _sessionBuffer = new();
        private static List<SessionLog> _fullLog = new();

        public static void RecordMove(string fileName, string oldPath, string newPath)
        {
            _sessionBuffer.Add((new FileRecord
            {
                FileName = fileName,
                OldPath = oldPath,
                NewPath = newPath
            }));
        }

        public static void CommitSession()
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


        public static void SaveLog()
        {
            List<SessionLog> fullLog = new();
            if (File.Exists("FileOrganizerLog.json"))
            {
                try
                {
                    string jsonText = File.ReadAllText("FileOrganizerLog.json");
                    fullLog = JsonSerializer.Deserialize<List<SessionLog>>(jsonText) ?? new();
                }
                catch (Exception)
                {
                    fullLog = new List<SessionLog>();
                }
            }
            fullLog.AddRange(_fullLog);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string newJson = JsonSerializer.Serialize(fullLog, options);
            File.WriteAllText("FileOrganizerLog.json", newJson);

            _fullLog.Clear();
        }
    }
}

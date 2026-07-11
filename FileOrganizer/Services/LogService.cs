using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FileOrganizer.src.Services
{
    // [Yellow] Where is the interface ILogService? Violation of dependency inversion. Dont talk to concrete impls, talk to interfaces.
    // This service like the rest of the services in NOT unit Testable. 

    internal class LogService
    {
        // [Yellow] Why not record, why class? 
        public class FileRecord
        {
            public required string FileName { get; set; }
            public required string OldPath { get; set; }
            public required string NewPath { get; set; }
        }

        // [Yellow] Why not record, why class? 
        public class SessionLog
        {
            public required string SessionId { get; set; }
            public required string Timestamp { get; set; }
            public required List<FileRecord> FilesMoved { get; set; }
        }

        // [Red] static? I see, so just a fancy glorified globals
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

            // [Yellow] Code is not Unit Testable. Giving it a yellow (and not a red) because I dont know whether SessionId is just ephemeral or domain significant.
            // 
            
            _fullLog.Add(new SessionLog
            {
                SessionId = Guid.NewGuid().ToString().Substring(0, 8),
                Timestamp = DateTime.Now.ToString("g"),
                FilesMoved = new List<FileRecord>(_sessionBuffer)
            });

            _sessionBuffer.Clear();
        }


        // [Red] static? I see, so just a fancy glorified global (not replaceable)
        // [Red] Method not UT able. See comment about IFileSystem elsewhere.
        
        public static void SaveLog()
        {
            List<SessionLog> fullLog = new();

            // [Yellow] Magic strings hardcoded in methods. Create a constant from it.
            // [Yellow] This should even be a config param to this service. Really limits the usability. If you had proper DI you could have read from config or even just supplied from top-level
            // and still would've kept the ability to have multiple loggers with different file output.

            // [Yellow] SaveLog overloaded with resp. Add a LoadExistingLog() so you can do
            // List<SessionLog> fullLog = LoadExistingLog()
            
            if (File.Exists("FileOrganizerLog.json"))
            {
                try
                {
                    // [Red] Not unit testable. There is not way to control FileSystem operations. 
                    // Either use MS fs abstractions packages, or roll out your own IFileSystem->MyFileSystem (Light years better that this horrible code)

                    // [Yellow] Magic strings duplicated.
                    
                    string jsonText = File.ReadAllText("FileOrganizerLog.json");
                    fullLog = JsonSerializer.Deserialize<List<SessionLog>>(jsonText) ?? new();
                }
                catch (Exception)
                {
                    // [Yellow] You think an exception would be thrown AFTER fullLog was overwritten? Explain why this is needed? [See line 65]
                    fullLog = new List<SessionLog>();
                }
            }
            fullLog.AddRange(_fullLog);

            // [Yellow] Consider outsoursing Persistence to a separate Service ILogFileWriter, so you can easily swap later with differnt implementations.
            // In tests you would need a Mock for it
            
            var options = new JsonSerializerOptions { WriteIndented = true };
            string newJson = JsonSerializer.Serialize(fullLog, options);

            // [Yellow] Magic strings duplicated.
            File.WriteAllText("FileOrganizerLog.json", newJson);

            _fullLog.Clear();
        }
    }
}

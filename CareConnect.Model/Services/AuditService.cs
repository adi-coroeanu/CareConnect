using System;
using System.Collections.Generic;
using System.Text;

namespace CareConnect.Model.Services
{
    public class AuditService
    {
        public string AuditFolderPath { get; }

        public AuditService()
        {
            AuditFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audit");
            Directory.CreateDirectory(AuditFolderPath);
        }

        public void Log(string message, string userId)
        {
            var fileName = DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            var filePath = Path.Combine(AuditFolderPath, fileName);

            var logLine = $"[{DateTime.Now.ToString("HH:mm:ss")}] [UserID: {userId}] {message}";

            File.AppendAllText(filePath, logLine + Environment.NewLine);
        }
    }
}

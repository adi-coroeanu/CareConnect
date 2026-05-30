using System;
using System.Collections.Generic;
using System.Text;

namespace CareConnect.Model.Services
{
    public class AuditService
    {
        private readonly string _auditFolderPath;

        public AuditService()
        {
            _auditFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audit");
            Directory.CreateDirectory(_auditFolderPath);
        }

        public void Log(string message, string userId)
        {
            var fileName = DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            var filePath = Path.Combine(_auditFolderPath, fileName);

            var logLine = $"[{DateTime.Now.ToString("HH:mm:ss")}] [UserID: {userId}] {message}";

            File.AppendAllText(filePath, logLine + Environment.NewLine);
        }
    }
}

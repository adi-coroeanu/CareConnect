using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CareConnect.Model.Services
{
    public class AuditAdminService
    {
        private readonly AuditService _auditService;

        public AuditAdminService(AuditService auditService) 
        {
            _auditService = auditService;
        }

        public List<string> GetAuditList()
        {
            var AuditList = new List<string>(Directory.GetFiles(_auditService.AuditFolderPath).Select(Path.GetFileNameWithoutExtension)!);

            return AuditList;
        }

        public string GetAuditPath
        {
            get => _auditService.AuditFolderPath;
        }
    }
}

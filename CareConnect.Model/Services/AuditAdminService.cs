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
            try
            {
                var auditList = Directory.EnumerateFiles(_auditService.AuditFolderPath).Select(p => Path.GetFileNameWithoutExtension(p)).Where(a => DateTime.TryParseExact(a, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,out _))
                    .OrderByDescending(a => DateTime.ParseExact(a, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture)).ToList();

                return auditList;
            }

            catch(DirectoryNotFoundException)
            {
                return new();
            }
        }

        public string GetAuditPath
        {
            get => _auditService.AuditFolderPath;
        }
    }
}

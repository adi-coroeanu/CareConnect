using CareConnect.Model.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CareConnect.Model.Services
{
    public class CodeAdminService
    {
        private readonly ModelContext _modelContext;
        private readonly AuditService _auditService;

        public CodeAdminService(ModelContext modelContext, AuditService auditService)
        {
            _modelContext = modelContext;
            _auditService = auditService;
        }

        public string GenerateCode(string adminId, int length = 8)
        {
            var enabledCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var code = new char[length];

            for (int i = 0; i < length; i++)
            {
                var randomIdx = RandomNumberGenerator.GetInt32(enabledCharacters.Length);
                code[i] = enabledCharacters[randomIdx];
            }

            var codeString = new string(code);

            var newCode = new Code
            {
                Id = codeString,
                IdUser = adminId,
                DateCreated = DateTime.Now
            };

            _modelContext.Codes.Add(newCode);

            _modelContext.SaveChanges();

            _auditService.Log("Generated a code", adminId);

            return codeString;
        }

        public bool ExistingAdminCode(string adminId)
        {
            var existingCode = _modelContext.Codes.Where(c => c.IdUser == adminId).FirstOrDefault();

            if (existingCode != null)
                return true;

            return false;
        }
    }
}

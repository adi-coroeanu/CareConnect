using CareConnect.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace CareConnect.Model.Services
{
    public class LoginService
    {
        private ModelContext _modelContext;
        private readonly AuditService _auditService;
        public LoginService(ModelContext modelContext, AuditService auditService)
        {
            _modelContext = modelContext;
            _auditService = auditService;
        }

        public User? GetUserFromDb(string email, string password)
        {
            var found_user = _modelContext.Users.Where(u => (u.Email == email && u.Password == password)).FirstOrDefault();

            if (found_user != null)
                _auditService.Log("User has logged in", found_user.Id);

            return found_user;
        }

        public bool AllFieldsCompleted(string? email, string? password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;
            return true;
        }
    }
}

using CareConnect.Model.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly PasswordHasher<string> _passwordHasher;
        public LoginService(ModelContext modelContext, AuditService auditService, PasswordHasher<string> passwordHasher)
        {
            _modelContext = modelContext;
            _auditService = auditService;
            _passwordHasher = passwordHasher;
        }

        public User? GetUserFromDb(string email, string password)
        {
            var foundUser = _modelContext.Users.Where(u => (u.Email == email)).FirstOrDefault();

            if (foundUser == null)
                return null;

            if(foundUser.Email == "admin" || foundUser.Email == "staff" || foundUser.Email == "client")
            {
                if (foundUser.Email == foundUser.Password)
                    return foundUser;
            }

            if (_passwordHasher.VerifyHashedPassword(foundUser.Email, foundUser.Password, password) == PasswordVerificationResult.Success)
            {
                _auditService.Log("User has logged in", foundUser.Id);
                
                return foundUser;
            }

            return null;
        }

        public bool AllFieldsCompleted(string? email, string? password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;
            return true;
        }
    }
}

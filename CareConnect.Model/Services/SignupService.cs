using CareConnect.Model.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CareConnect.Model.Services
{
    public class SignupService
    {
        private readonly ModelContext _modelContext;
        private readonly AuditService _auditService;
        private readonly PasswordHasher<string> _passwordHasher;

        public SignupService(ModelContext modelContext, AuditService auditService, PasswordHasher<string> passwordHasher) 
        {
            _modelContext = modelContext;
            _auditService = auditService;
            _passwordHasher = passwordHasher;
        }

        public bool AllFieldsCompleted(string? email, string? firstName, string? lastName, string? password, string? repassword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(repassword))
                return false;
            return true;
        }

        public bool ExistingEmail(string? email)
        {
            if (email == null)
                return false;

            var user = _modelContext.Users.Where(u => u.Email == email).FirstOrDefault();

            if (user != null)
                return true;
            return false;
        }

        public bool MatchingPasswords(string? password, string? repassword)
        {
            if (password == null || repassword == null)
                return false;

            if (password.Equals(repassword))
                return true;
            return false;
        }

        public bool CorrectPasswordFormat(string? password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};:'""\\|,.<>\/?]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        public bool ExistingCode(string? code)
        {
            if (code == null)
                return false;

            var existingCode = _modelContext.Codes.Where(c => c.Id == code).FirstOrDefault();

            if(existingCode != null)
                return true;
            return false;
        }

        public User? AddUser(string? email, string? firstName, string? lastName, string? password, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(password))
                return null;

            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserRole = role,
                Password = _passwordHasher.HashPassword(email, password),
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            _modelContext.Users.Add(newUser);
            _modelContext.SaveChanges();

            _auditService.Log($"New account created [UserRole: {newUser.UserRole}]", newUser.Id);

            return newUser;
        }
    }
}

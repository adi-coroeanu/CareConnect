using CareConnect.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CareConnect.Model.Services
{
    public class AccountSettingsService
    {
        private readonly ModelContext _modelContext;

        public AccountSettingsService(ModelContext modelContext) 
        {
            _modelContext = modelContext;
        }

        public bool ExistingEmail(string? email, string activeUserId)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;

            var existingUser = _modelContext.Users.Where(u => u.Email == email).FirstOrDefault();

            if (existingUser == null)
                return false;

            if (existingUser.Id == activeUserId)
                return false;

            return true;
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
            if(string.IsNullOrWhiteSpace(password))
                return true;

            if (password.Length < 8)
                return false;

            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};:'""\\|,.<>\/?]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        public bool SaveAccountSettings(User? activeUser, string? email, string? firstname, string? lastname, string? password)
        {
            bool changed = false;

            if (activeUser.Email != email && !string.IsNullOrEmpty(email))
            {
                activeUser.Email = email;
                changed = true;
            }
            if (activeUser.FirstName != firstname && !string.IsNullOrEmpty(firstname))
            {
                activeUser.FirstName = firstname;
                changed = true;
            }
            if (activeUser.LastName != lastname && !string.IsNullOrEmpty(lastname))
            {
                activeUser.LastName = lastname;
                changed = true;
            }
            if (activeUser.Password != password && !string.IsNullOrEmpty(password))
            {
                activeUser.Password = password;
                changed = true;
            }

            if(changed)
            {
                _modelContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}

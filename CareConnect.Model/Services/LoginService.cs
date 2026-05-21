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
        public LoginService(ModelContext modelContext)
        {
            _modelContext = modelContext;
        }

        public User? GetUserFromDb(string email, string password)
        {
            var found_user = _modelContext.Users.Where(u => (u.Email == email && u.Password == password)).FirstOrDefault();

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

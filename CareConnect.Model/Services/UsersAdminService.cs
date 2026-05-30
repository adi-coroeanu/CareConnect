using CareConnect.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareConnect.Model.Services
{
    public class UsersAdminService
    {
        private readonly ModelContext _modelContext;
        private readonly AuditService _auditService;

        public UsersAdminService(ModelContext modelContext, AuditService auditService) 
        {
            _modelContext = modelContext;
            _auditService = auditService;
        }

        public List<User> GetUsers(string? typeUsers = null)
        {
            var allUsers = _modelContext.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();

            if (typeUsers == "All users")
                return allUsers;
            else if (typeUsers == "Staff")
                return allUsers.Where(u => u.UserRole == "STAFF").ToList();
            else if (typeUsers == "Clients")
                return allUsers.Where(u => u.UserRole == "CLIENT").ToList();
            else if (typeUsers == "Admins")
                return allUsers.Where(u => u.UserRole == "ADMIN").ToList();

            return allUsers;
        }

        public void DeleteUser(string userId, string adminId)
        {
            var deletingUser = _modelContext.Users.Where(u => u.Id == userId).First();

            _modelContext.Users.Remove(deletingUser);

            _modelContext.SaveChanges();

            _auditService.Log($"Deleted account [UserId: {userId}]", adminId);
        }
    }
}

using CareConnect.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CareConnect.Model.Services
{
    public class AppointmentsClientService
    {
        private readonly ModelContext _modelContext;
        public AppointmentsClientService(ModelContext modelContext) 
        {
            _modelContext = modelContext;
        }

        public List<Booking> GetBookings(string userId, string? periodBookings = null)
        {
            var allBookings = GetAllNecesaryInfoBookings(userId);

            if (periodBookings == "All appointmets")
                return allBookings;
            else if(periodBookings == "Future appointments")
                return allBookings.Where(b => b.BookingDate > DateTime.Now).ToList();
            else if(periodBookings == "Past appointments")
                return allBookings.Where(b => b.BookingDate < DateTime.Now).ToList();

            return allBookings;
        }

        private List<Booking> GetAllNecesaryInfoBookings(string userId)
        {
            return _modelContext.Bookings.Where(b => b.IdUser == userId).Include(b => b.IdServiceNavigation).ThenInclude(s => s!.IdDoctorNavigation).ToList();
        }
    }
}

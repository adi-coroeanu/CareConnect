using CareConnect.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareConnect.Model.Services
{
    public class AppointmentsStaffService
    {
        private readonly ModelContext _modelContext;
        public AppointmentsStaffService(ModelContext modelContext)
        {
            _modelContext = modelContext;
        }

        public List<Booking> GetBookings(string staffId, string? periodBookings = null)
        {
            var allBookings = GetAllNecesaryInfoBookings(staffId);

            if (periodBookings == "All appointmets")
                return allBookings;
            else if (periodBookings == "Future appointments")
                return allBookings.Where(b => b.BookingDate > DateTime.Now).ToList();
            else if (periodBookings == "Past appointments")
                return allBookings.Where(b => b.BookingDate < DateTime.Now).ToList();

            return allBookings;
        }

        private List<Booking> GetAllNecesaryInfoBookings(string staffId)
        {
            return _modelContext.Bookings.Include(b => b.IdServiceNavigation).Include(b => b.IdUserNavigation).Where(b => b.IdServiceNavigation!.IdDoctor == staffId).OrderByDescending(b =>  b.BookingDate).ToList();
        }
    }
}

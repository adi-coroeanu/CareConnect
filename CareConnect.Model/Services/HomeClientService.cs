using CareConnect.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CareConnect.Model.Services
{
    public class HomeClientService
    {
        private readonly ModelContext _modelContext;
        private readonly AuditService _auditService;
        public HomeClientService(ModelContext modelContext, AuditService auditService) 
        {
            _modelContext = modelContext;
            _auditService = auditService;
        }

        public List<Service> GetServices()
        {
            var services = _modelContext.Services.Include(s => s.IdDoctorNavigation).ToList();

            return services;
        }

        public List<string> GetFreePeriods(DateTime? date, Service? selectedService)
        {
            if (date == null || selectedService == null)
                return new List<string>();

            if (date.Value.DayOfWeek == DayOfWeek.Saturday || date.Value.DayOfWeek == DayOfWeek.Sunday)
                return new List<string>();

            if(date < DateTime.Now)
                return new List<string>();

            int duration = (int)selectedService.EstTimeMinutes;

            TimeSpan start = TimeSpan.Parse(selectedService.TimeStart);
            TimeSpan stop = TimeSpan.Parse(selectedService.TimeEnd);

            // Fetch existing appointments for that date
            List<Booking> existingAppointments = _modelContext.Bookings
                .Where(a => a.BookingDate.Date == date.Value.Date)
                .OrderBy(a => a.BookingDate.TimeOfDay)
                .ToList();

            List<string> freePeriods = new List<string>();
            TimeSpan current = start;

            while (current + TimeSpan.FromMinutes(duration) <= stop)
            {
                TimeSpan slotEnd = current + TimeSpan.FromMinutes(duration);

                bool isOverlapping = existingAppointments.Any(a =>
                    a.BookingDate.TimeOfDay < slotEnd &&
                    a.BookingDate.TimeOfDay + TimeSpan.FromMinutes(duration) > current
                );

                if (!isOverlapping)
                {
                    freePeriods.Add($"{current:hh\\:mm}-{slotEnd:hh\\:mm}");
                }

                current += TimeSpan.FromMinutes(duration);
            }

            return freePeriods;
        }

        public void MakeNewAppointment(string idService, string idClient, DateTime appointmentDate, string appointmentPeriod)
        {
            var appointmentTimeOnly = TimeOnly.Parse(appointmentPeriod.Split('-').First());
            var appointmentDateOnly = DateOnly.FromDateTime(appointmentDate);

            var appointmentDateTime = appointmentDateOnly.ToDateTime(appointmentTimeOnly);

            var newBooking = new Booking
            {
                Id = Guid.NewGuid().ToString(),
                IdUser = idClient,
                IdService = idService,
                BookingDate = appointmentDateTime,
                TotalAmmount = _modelContext.Services.Where(s => s.Id == idService).FirstOrDefault()!.Price
            };

            _modelContext.Bookings.Add(newBooking);
            _modelContext.SaveChanges();
            
            _auditService.Log($"Made an appointment [AppointmentId: {newBooking.Id}]", idClient);
        }
    }
}

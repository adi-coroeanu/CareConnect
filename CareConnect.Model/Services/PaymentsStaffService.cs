using CareConnect.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareConnect.Model.Services
{
    public class PaymentsStaffService
    {
        private readonly ModelContext _modelContext;
        private readonly AuditService _auditService;

        public PaymentsStaffService(ModelContext modelContext, AuditService auditService)
        {
            _modelContext = modelContext;
            _auditService = auditService;
        }

        public List<Booking> GetPendingPayments(string staffId)
        {
            var userPendingPayments = _modelContext.Bookings.Include(b => b.IdServiceNavigation).Where(b => b.IdServiceNavigation!.IdDoctor == staffId && b.BookingDate >= DateTime.Today && b.TotalAmmount > 0).OrderBy(b => b.BookingDate).ToList();

            return userPendingPayments;
        }

        public void GeneratePayment(string bookingId, decimal paymentValue, string paymentType, string staffId)
        {
            var currentBooking = _modelContext.Bookings.Where(b => b.Id == bookingId).First();

            var newPayment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                IdBooking = bookingId,
                PaymentValue = paymentValue,
                PaymentDate = DateTime.Now,
                PaymentType = paymentType
            };

            currentBooking.TotalAmmount = currentBooking.TotalAmmount - newPayment.PaymentValue;

            _modelContext.Payments.Add(newPayment);

            _modelContext.SaveChanges();

            _auditService.Log($"(Staff transaction) paid {paymentValue} for [BookingId: {bookingId}]", staffId);
        }
    }
}

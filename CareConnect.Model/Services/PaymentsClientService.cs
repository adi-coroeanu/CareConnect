using CareConnect.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareConnect.Model.Services
{
    public class PaymentsClientService
    {
        private readonly ModelContext _modelContext;
        public PaymentsClientService(ModelContext modelContext)
        {
            _modelContext = modelContext;
        }

        public List<Payment> GetPaymentsHistory(string userId)
        {
            var userPayments = _modelContext.Payments.Include(p => p.IdBookingNavigation).Where(p => p.IdBookingNavigation!.IdUser == userId).OrderByDescending(p => p.PaymentDate).ToList();

            return userPayments;
        }

        public List<Booking> GetPendingPayments(string userId)
        {
            var userPendingPayments = _modelContext.Bookings.Where(b => b.IdUser == userId && b.BookingDate > DateTime.Now && b.TotalAmmount > 0).OrderByDescending(b => b.BookingDate).ToList();

            return userPendingPayments;
        }

        public void GeneratePayment(string bookingId, decimal paymentValue, string paymentType)
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
        }
    }
}

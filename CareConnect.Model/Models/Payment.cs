using System;
using System.Collections.Generic;

namespace CareConnect.Model.Models;

public partial class Payment
{
    public string Id { get; set; } = null!;

    public string? IdBooking { get; set; }

    public decimal PaymentValue { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentType { get; set; } = null!;

    public virtual Booking? IdBookingNavigation { get; set; }
}

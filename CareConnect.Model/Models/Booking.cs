using System;
using System.Collections.Generic;

namespace CareConnect.Model.Models;

public partial class Booking
{
    public string Id { get; set; } = null!;

    public string? IdUser { get; set; }

    public string? IdService { get; set; }

    public DateTime BookingDate { get; set; }

    public decimal TotalAmmount { get; set; }

    public virtual Service? IdServiceNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

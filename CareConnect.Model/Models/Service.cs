using System;
using System.Collections.Generic;

namespace CareConnect.Model.Models;

public partial class Service
{
    public string Id { get; set; } = null!;

    public string? IdDoctor { get; set; }

    public string Name { get; set; } = null!;

    public decimal EstTimeMinutes { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual User? IdDoctorNavigation { get; set; }
}

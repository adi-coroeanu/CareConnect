using System;
using System.Collections.Generic;

namespace CareConnect.Model.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Code> Codes { get; set; } = new List<Code>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}

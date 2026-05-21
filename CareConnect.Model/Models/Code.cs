using System;
using System.Collections.Generic;

namespace CareConnect.Model.Models;

public partial class Code
{
    public string Id { get; set; } = null!;

    public string? IdUser { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}

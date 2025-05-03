using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class Donate
{
    public Guid Id { get; set; }

    public string TransId { get; set; } = null!;

    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Message { get; set; } = null!;

    public decimal Amount { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual UserAccount User { get; set; } = null!;
}

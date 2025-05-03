using System;
using System.Collections.Generic;

namespace GauCorner.Data.Entities;

public partial class UserAccount
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Name { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public string? Attachment { get; set; }

    public string? Path { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Donate> Donates { get; set; } = new List<Donate>();

    public virtual ICollection<StreamConfig> StreamConfigs { get; set; } = new List<StreamConfig>();

    public virtual ICollection<Uiconfig> Uiconfigs { get; set; } = new List<Uiconfig>();

    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
}

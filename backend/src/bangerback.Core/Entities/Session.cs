using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.bangerback.Core.Entities;

[Index("Token", Name = "UQ__Sessions__CA90DA7A01109920", IsUnique = true)]
public partial class Session
{
    [Key]
    [Column("session_id")]
    public int SessionId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("token")]
    [StringLength(255)]
    [Unicode(false)]
    public string Token { get; set; } = null!;

    [Column("expires_at", TypeName = "datetime")]
    public DateTime ExpiresAt { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Sessions")]
    public virtual User? User { get; set; }
}

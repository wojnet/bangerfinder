using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalAppTemplate.Models;

[PrimaryKey("UserId", "SongId")]
public partial class Recommendation
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Key]
    [Column("song_id")]
    public int SongId { get; set; }

    [Column("score", TypeName = "decimal(5, 2)")]
    public decimal? Score { get; set; }

    [Column("generated_at", TypeName = "datetime")]
    public DateTime? GeneratedAt { get; set; }

    [ForeignKey("SongId")]
    [InverseProperty("Recommendations")]
    public virtual Song Song { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Recommendations")]
    public virtual User User { get; set; } = null!;
}

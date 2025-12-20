using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.bangerback.Core.Entities;

[PrimaryKey("UserId", "SongId")]
public partial class UserSongFavorite
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Key]
    [Column("song_id")]
    public int SongId { get; set; }

    [Column("added_at", TypeName = "datetime")]
    public DateTime? AddedAt { get; set; }

    [ForeignKey("SongId")]
    [InverseProperty("UserSongFavorites")]
    public virtual Song Song { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserSongFavorites")]
    public virtual User User { get; set; } = null!;
}

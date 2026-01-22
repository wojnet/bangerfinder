using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.bangerback.Core.Entities;

[Index("ExternalId", Name = "UQ__Songs__ExternalId", IsUnique = true)]
[Index("Mbid", Name = "UQ__Songs__Mbid", IsUnique = true)]
public partial class Song
{
    [Key]
    [Column("song_id")]
    public int SongId { get; set; }

    [Column("external_id")]
    [StringLength(255)]
    [Unicode(false)]
    public string ExternalId { get; set; } = null!;

    [Column("mbid")]
    [StringLength(100)]
    [Unicode(false)]
    public string? Mbid { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("artist")]
    [StringLength(255)]
    public string Artist { get; set; } = null!;

    [Column("album")]
    [StringLength(255)]
    public string? Album { get; set; }

    // ADDED: Property to store the Last.fm image URL
    [Column("cover")]
    [StringLength(2048)] // URLs can be very long
    public string? Cover { get; set; }

    [InverseProperty("Song")]
    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    [InverseProperty("Song")]
    public virtual ICollection<UserSongFavorite> UserSongFavorites { get; set; } = new List<UserSongFavorite>();
}
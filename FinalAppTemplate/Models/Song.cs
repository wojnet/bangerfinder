using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalAppTemplate.Models;

[Index("SpotifyId", Name = "UQ__Songs__C253CFF10D62E3EF", IsUnique = true)]
public partial class Song
{
    [Key]
    [Column("song_id")]
    public int SongId { get; set; }

    [Column("spotify_id")]
    [StringLength(50)]
    [Unicode(false)]
    public string SpotifyId { get; set; } = null!;

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("artist")]
    [StringLength(255)]
    public string Artist { get; set; } = null!;

    [Column("album")]
    [StringLength(255)]
    public string? Album { get; set; }

    [InverseProperty("Song")]
    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    [InverseProperty("Song")]
    public virtual ICollection<UserSongFavorite> UserSongFavorites { get; set; } = new List<UserSongFavorite>();
}

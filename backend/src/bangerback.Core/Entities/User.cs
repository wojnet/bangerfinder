using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.bangerback.Core.Entities;

[Index("Email", Name = "UQ__Users__AB6E616455510AF1", IsUnique = true)]
[Index("Username", Name = "UQ__Users__F3DBC57274E78E26", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = null!;

    [Column("username")]
    [StringLength(50, MinimumLength = 4)]
    [Required]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    [Required]
    [EmailAddress]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("password_hash")]
    [StringLength(255)]
    [Required]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!;

    [Column("is_verified")]
    public bool IsVerified { get; set; }

    [Column("verification_token")]
    [StringLength(100)]
    [Unicode(false)]
    public string? VerificationToken { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    // --- LAST.FM UPDATED COLUMNS ---

    [Column("lastfm_username")] // Renamed from spotify_id
    [StringLength(100)]
    [Unicode(false)]
    public string? LastFmUsername { get; set; }

    [Column("lastfm_session_key")] // Renamed from spotify_access_token
    public string? LastFmSessionKey { get; set; }

    [Column("lastfm_token")] // Renamed from spotify_refresh_token
    public string? LastFmToken { get; set; }

    [Column("last_updated_at")] // Renamed from token_expires_at (Last.fm keys don't expire)
    public DateTime? LastUpdatedAt { get; set; }

    // --- NAVIGATION PROPERTIES ---
    [InverseProperty("User")]
    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    [InverseProperty("User")]
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    [InverseProperty("User")]
    public virtual ICollection<UserSongFavorite> UserSongFavorites { get; set; } = new List<UserSongFavorite>();
}
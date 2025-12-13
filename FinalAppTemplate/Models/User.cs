using FinalAppTemplate.Models.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalAppTemplate.Models;

[Index("Email", Name = "UQ__Users__AB6E616455510AF1", IsUnique = true)]
[Index("Username", Name = "UQ__Users__F3DBC57274E78E26", IsUnique = true)]
public partial class User
{
    [Column("is_verified")]
    public bool IsVerified { get; set; } // Maps to BIT (0 or 1)

    [Column("verification_token")]
    [StringLength(100)]
    [Unicode(false)] // VARCHAR in SQL Server is non-unicode. Use [Unicode(true)] if you used NVARCHAR.
    public string? VerificationToken { get; set; } // Question mark (?) allows nulls

    [Key]
    [Column("user_id")]
    public int UserId { get; set; } 

    [Column("name")]
    [StringLength(50)]
    [Required(ErrorMessage = "Name is required.")] 
    public string Name { get; set; } = null!;

    [Column("username")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be at least 4 characters.")] // Rule: Min 4 chars
    [Required(ErrorMessage = "Username is required.")] // Rule: Not empty
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    [Unicode(false)]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format (missing @ or domain).")]
    public string Email { get; set; } = null!;

    [Column("password_hash")]
    [StringLength(255)] // Just ensure it fits in the DB column
    [Required]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; } 

    [InverseProperty("User")]
    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    [InverseProperty("User")]
    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    [InverseProperty("User")]
    public virtual ICollection<UserSongFavorite> UserSongFavorites { get; set; } = new List<UserSongFavorite>();
}

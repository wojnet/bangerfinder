namespace Backend.bangerback.Core.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        // NOTICE: No PasswordHash, No VerificationToken. Safe for React.
    }
}

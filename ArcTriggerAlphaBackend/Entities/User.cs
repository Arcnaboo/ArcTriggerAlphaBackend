namespace ArcTriggerAlphaBackend.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = default!;
        public string Password { get; private set; } = default!;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        // EF Core private ctor
        private User() { }

        // Public constructor
        public User(string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Email = email;
            Password = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }
    }
}

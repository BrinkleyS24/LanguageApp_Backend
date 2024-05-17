using Microsoft.EntityFrameworkCore;

namespace MyProject.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        // Define your DbSets here
        public DbSet<Language> Languages { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity properties and relationships here if necessary
            modelBuilder.Entity<Language>()
                .HasIndex(l => l.LanguageCode)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            // Add more configurations as needed
        }
    }

    public class Language
    {
        public int Id { get; set; }
        public string? LanguageCode { get; set; }
        public string? LanguageName { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        // Add more properties as needed
    }
}

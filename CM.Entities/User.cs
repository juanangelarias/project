using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class User: BaseEntity, IBaseEntity
{
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool? IsEnabled { get; set; }
    public string? Email { get; set; }
    public bool? EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Token { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";

    public IEnumerable<UserRole>? UserRoles { get; set; }
    public IEnumerable<UserRefreshToken>? UserRefreshToken { get; set; }
    public IEnumerable<UserRequestToken>? UserRequestTokens { get; set; }
    public IEnumerable<UserPassword>? UserPasswords { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        Console.WriteLine("User OnModelCreating");
        m.Entity<User>(e =>
        {
            MapBaseEntityProperties(e);

            e
                .Property(p => p.UserName)
                .HasMaxLength(250)
                .IsRequired();

            e
                .Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired();
            
            e
                .Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired();
            
            e
                .Property(p => p.Email)
                .HasMaxLength(250)
                .IsRequired();

            e
                .Property(p => p.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            e.Property(x => x.IsEnabled)
                .HasDefaultValue(true);

            e.Property(x => x.EmailConfirmed)
                .HasDefaultValue(false);

            e
                .HasMany(p => p.UserRequestTokens)
                .WithOne(o => o.User)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e
                .HasMany(p => p.UserRefreshToken)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e
                .HasMany(x => x.UserPasswords)
                .WithOne(o => o.User)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            e
                .HasMany(x => x.UserRoles)
                .WithOne(o => o.User)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
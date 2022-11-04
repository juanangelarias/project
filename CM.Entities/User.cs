using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class User: BaseEntity, IBaseEntity
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsEnabled { get; set; }
    public long PersonId { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PhoneNumber { get; set; }
    public string Token { get; set; }

    public List<UserRole> Roles { get; set; }
    public List<UserRefreshToken> UserRefreshToken { get; set; }
    public List<UserPassword> UserPasswords { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<User>(entity =>
        {
            MapBaseEntityProperties(entity);

            entity
                .Property(p => p.UserName)
                .HasMaxLength(250)
                .IsRequired();

            entity
                .Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired();
            
            entity
                .Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired();
            
            entity
                .Property(p => p.Email)
                .HasMaxLength(250)
                .IsRequired();

            entity
                .Property(p => p.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.IsEnabled)
                .HasDefaultValue(true);

            entity
                .HasMany(p => p.UserRefreshToken)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasMany(x => x.UserPasswords)
                .WithOne(o => o.User)
                .HasForeignKey(k => k.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
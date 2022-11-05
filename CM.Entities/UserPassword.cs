using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class UserPassword: BaseEntity, IBaseEntity
{
    public long UserId { get; set; }
    public string SecurityStamp  { get; set; }
    public string PasswordHash {get;set;}

    public User User { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<UserPassword>(e =>
        {
            MapBaseEntityProperties(e);

            e
                .Property(p => p.SecurityStamp)
                .HasMaxLength(500)
                .IsRequired();

            e
                .Property(p => p.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();
        });
    }
}
using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class UserRequestToken: BaseEntity, IBaseEntity
{
    public long? UserId { get; set; }
    public string? Token { get; set; }
    public DateTime? Emitted { get; set; }
    public DateTime? Expires { get; set; }

    public User? User { get; set; }

    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<UserRequestToken>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.UserId)
                .IsRequired();
            
            e.Property(p => p.Token)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(p => p.Emitted)
                .IsRequired();

            e.Property(p => p.Expires)
                .IsRequired();

            e.HasIndex(i => i.Expires);
        });
    }
}
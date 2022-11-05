using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class UserRole: BaseEntity, IBaseEntity
{
    public long UserId { get; set; }
    public long RoleId { get; set; }

    public User User { get; set; }
    public Role Role { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<UserRole>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.UserId)
                .IsRequired();
            
            e.Property(p => p.RoleId)
                .IsRequired();
            
            e.HasIndex(i => new {i.UserId, i.RoleId})
                .IsUnique();
        });
    }
}
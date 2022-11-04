using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class Role: BaseEntity, IBaseEntity
{
    public string Code { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Role>(e =>
        {
            MapBaseEntityProperties(e);

            e
                .Property(p => p.Code)
                .HasMaxLength(10)
                .IsRequired();

            e
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            e
                .Property(p => p.Description)
                .HasMaxLength(250)
                .IsRequired();
        });
    }
}
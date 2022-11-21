using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class Member: BaseEntity, IBaseEntity
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Alias { get; set; }
    public long? ClubId { get; set; }
    public bool IsActive { get; set; }

    public Club? Club { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Member>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(20);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(p => p.Alias)
                .HasMaxLength(20);

            e.Property(p => p.ClubId)
                .IsRequired();

            e.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            e.HasIndex(i => new {i.ClubId, i.Name});

            e.HasOne(o => o.Club)
                .WithMany()
                .HasForeignKey(k => k.ClubId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
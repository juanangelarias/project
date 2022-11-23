using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class ProductCombo : BaseEntity, IBaseEntity
{
    public long? ParentProductId { get; set; }
    public long? ChildProductId { get; set; }

    public Product? ParentProduct { get; set; }
    public Product? ChildProduct { get; set; }

    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<ProductCombo>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.ParentProductId).IsRequired();

            e.Property(p => p.ChildProductId).IsRequired();

            e.HasIndex(i => new { i.ParentProductId, i.ChildProductId }).IsUnique();

            e.HasOne(o => o.ParentProduct)
                .WithMany()
                .HasForeignKey(k => k.ParentProductId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(o => o.ChildProduct)
                .WithMany()
                .HasForeignKey(k => k.ChildProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

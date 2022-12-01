using CM.Entities.Base;
using CM.Model.Enum;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class Product : BaseEntity, IBaseEntity
{
    public long? ConferenceId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? PrimaryPrice { get; set; }
    public decimal? SecondaryPrice { get; set; }
    public bool? RequireName { get; set; }
    public bool? PrintBadge { get; set; }
    public ProductType ProductType { get; set; }

   public Conference? Conference { get; set; }

    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Product>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.ConferenceId).IsRequired();

            e.Property(p => p.Name).IsRequired().HasMaxLength(50);

            e.Property(p => p.Description).HasMaxLength(250);

            e.Property(p => p.PrimaryPrice).IsRequired().HasPrecision(10, 2);

            e.Property(p => p.SecondaryPrice).IsRequired().HasPrecision(10, 2);

            e.Property(p => p.RequireName).IsRequired().HasDefaultValue(false);

            e.Property(p => p.PrintBadge).IsRequired().HasDefaultValue(false);

            e.Property(p => p.ProductType).IsRequired();

            e.HasIndex(i => new {i.ConferenceId, i.Name})
                .IsUnique();
        });
    }
}

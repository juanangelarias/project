using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class Conference : BaseEntity, IBaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Theme { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? City { get; set; }
    public long? HostClubId { get; set; }
    public long? PrimaryCurrencyId { get; set; }
    public long? SecondaryCurrencyId { get; set; }
    public decimal? ExchangeRate { get; set; }

    public Club? HostClub { get; set; }
    public Currency? PrimaryCurrency { get; set; }
    public Currency? SecondaryCurrency { get; set; }
    public List<Product>? Products { get; set; }

    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Conference>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Name).IsRequired().HasMaxLength(75);

            e.Property(p => p.Description).IsRequired().HasMaxLength(250);

            e.Property(p => p.Theme).HasMaxLength(100);

            e.Property(p => p.StartDate).IsRequired();

            e.Property(p => p.EndDate).IsRequired();

            e.Property(p => p.City).IsRequired().HasMaxLength(75);

            e.Property(p => p.HostClubId).IsRequired();

            e.Property(p => p.PrimaryCurrencyId).IsRequired();

            e.Property(p => p.SecondaryCurrencyId).IsRequired();

            e.Property(p => p.ExchangeRate).HasPrecision(10, 4).IsRequired();

            e.HasOne(o => o.HostClub)
                .WithMany()
                .HasForeignKey(k => k.HostClubId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(o => o.PrimaryCurrency)
                .WithMany()
                .HasForeignKey(k => k.PrimaryCurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(o => o.SecondaryCurrency)
                .WithMany()
                .HasForeignKey(k => k.SecondaryCurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(x => x.Products)
                .WithOne(o => o.Conference)
                .HasForeignKey(k => k.ConferenceId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

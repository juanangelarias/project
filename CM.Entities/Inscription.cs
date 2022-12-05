using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class Inscription: BaseEntity, IBaseEntity
{
    public long? ConferenceId { get; set; }

    public DateTime? Date { get; set; }
    public long? CurrencyId { get; set; }
    public long? PaymentMethodId { get; set; }
    public string? BankName { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Name { get; set; }

    public string? Notes { get; set; }

    public decimal? GrandTotal { get; set; }
    
    public Conference? Conference { get; set; }
    public Currency? Currency { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }

    public List<InscriptionDetail> InscriptionDetails { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Inscription>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.ConferenceId)
                .IsRequired();

            e.Property(p => p.Date)
                .IsRequired();
            
            e.Property(p => p.CurrencyId)
                .IsRequired();
            
            e.Property(p => p.PaymentMethodId)
                .IsRequired();

            e.Property(p => p.BankName)
                .HasMaxLength(30);

            e.Property(p => p.DocumentNumber)
                .HasMaxLength(30);

            e.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(p => p.Notes)
                .HasMaxLength(500);

            e.Property(p => p.GrandTotal)
                .HasPrecision(10,2)
                .IsRequired();

            e.HasOne(o => o.Currency)
                .WithMany()
                .HasForeignKey(k => k.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);
            
            e.HasOne(o => o.PaymentMethod)
                .WithMany()
                .HasForeignKey(k => k.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(x => x.InscriptionDetails)
                .WithOne(o => o.Inscription)
                .HasForeignKey(k => k.InscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(i => new {i.ConferenceId, i.Date});
        });
    }
}
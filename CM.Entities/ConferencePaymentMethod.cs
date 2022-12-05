using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class ConferencePaymentMethod: BaseEntity, IBaseEntity
{
    public long? ConferenceId { get; set; }
    public long? PaymentMethodId { get; set; }
    public bool? IsAvailable { get; set; }

    public Conference? Conference { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<ConferencePaymentMethod>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.ConferenceId)
                .IsRequired();

            e.Property(p => p.PaymentMethodId)
                .IsRequired();

            e.Property(p => p.IsAvailable)
                .IsRequired(false)
                .HasDefaultValue(false);

            e.HasOne(o => o.PaymentMethod)
                .WithMany()
                .HasForeignKey(k => k.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(i => new {i.ConferenceId, i.PaymentMethodId})
                .IsUnique();
        });
    }
}
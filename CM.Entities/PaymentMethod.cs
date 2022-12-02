using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class PaymentMethod: BaseEntity, IBaseEntity
{
    public string? Name { get; set; }
    public bool? RequiredBankName { get; set; }
    public bool? RequireDocumentNumber { get; set; }

    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<PaymentMethod>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.Name)
                .HasMaxLength(25)
                .IsRequired();

            e.Property(p => p.RequiredBankName)
                .IsRequired();

            e.Property(p => p.RequireDocumentNumber)
                .IsRequired();

            e.HasIndex(i => i.Name)
                .IsUnique();
        });
    }
}
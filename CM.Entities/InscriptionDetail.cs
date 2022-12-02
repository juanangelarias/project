using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class InscriptionDetail: BaseEntity, IBaseEntity
{
    public long InscriptionId { get; set; }
    public int Line { get; set; }
    public long? ProductId { get; set; }
    public long? MemberId { get; set; }
    public string? Notes { get; set; }
    public decimal Amount { get; set; }

    public Inscription? Inscription { get; set; }
    public Product? Product { get; set; }
    public Member? Member { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<InscriptionDetail>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.InscriptionId)
                .IsRequired();

            e.Property(p => p.Line)
                .IsRequired();

            e.Property(p => p.ProductId)
                .IsRequired();

            e.Property(p => p.MemberId)
                .IsRequired(false);

            e.Property(p => p.Notes)
                .HasMaxLength(100);

            e.Property(p => p.Amount)
                .IsRequired();

            e.HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(k => k.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            
            e.HasOne(o => o.Member)
                .WithMany()
                .HasForeignKey(k => k.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
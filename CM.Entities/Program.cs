using CM.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace CM.Entities;

public class Program: BaseEntity, IBaseEntity
{
    public long? ConferenceId { get; set; }
    public DateTime? DateTime { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Speaker { get; set; }
    
    public string Date => DateTime == null ? "" : Convert.ToDateTime(DateTime).ToString("dd MMMM yyyy");
    public string Time => DateTime == null ? "" : Convert.ToDateTime(DateTime).ToString("hh:mm tt");

    public Conference Conference { get; set; }
    
    public void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Program>(e =>
        {
            MapBaseEntityProperties(e);

            e.Property(p => p.ConferenceId)
                .IsRequired();
            
            e.Property(p => p.DateTime)
                .IsRequired();

            e.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            e.Property(p => p.Description)
                .HasMaxLength(500);

            e.Property(p => p.Speaker)
                .HasMaxLength(100);

            e.HasIndex(i => new {i.ConferenceId, i.DateTime});
        });
    }
}
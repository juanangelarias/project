using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CM.Entities.Base;

public class BaseEntity
{
    public long Id { get; set; }
    public bool IsDeleted { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public long? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public byte[] Timestamp { get; set; }

    public User? CreatedByUser { get; set; }
    public User? ModifiedByUser { get; set; }

    protected void MapBaseEntityProperties<TEntity>(EntityTypeBuilder<TEntity> entity, string primaryKeyColumnName = "")
        where TEntity : class, IBaseEntity
    {
        entity.ToTable(typeof(TEntity).Name);

        if (string.IsNullOrEmpty(primaryKeyColumnName))
        {
            entity.Property(x => x.Id)
                .HasColumnName(typeof(TEntity).Name + "Id")
                .ValueGeneratedOnAdd();
        }
        else
        {
            entity.Property(x => x.Id)
                .HasColumnName(primaryKeyColumnName).ValueGeneratedNever();
        }
        
        entity.Property(x => x.Timestamp).IsRowVersion();

        entity.HasOne(o => o.CreatedByUser)
            .WithMany()
            .HasForeignKey(k => k.CreatedBy);

        entity.HasOne(o => o.ModifiedByUser)
            .WithMany()
            .HasForeignKey(k => k.ModifiedBy);
    }
}
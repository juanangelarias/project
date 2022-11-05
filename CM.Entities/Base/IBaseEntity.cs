namespace CM.Entities.Base;

public interface IBaseEntity : IConfigurableEntity
{
    public long Id { get; set; }
    public bool IsDeleted { get; set; }
    public byte[] Timestamp { get; set; }

    public long? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public long? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public User? CreatedByUser { get; set; }
    public User? ModifiedByUser { get; set; }
}
namespace CM.Model.Dto.Base;

public interface IBaseDto
{
    public long Id { get; set; }
    public byte[] Timestamp { get; set; }
}
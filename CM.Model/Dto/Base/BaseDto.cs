namespace CM.Model.Dto.Base;

public class BaseDto: IBaseDto
{
    public long Id { get; set; }

    public byte[] Timestamp { get; set; }
}
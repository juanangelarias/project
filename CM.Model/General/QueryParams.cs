namespace CM.Model.General;

public class QueryParams
{
    public string? Filter { get; set; }
    public int PageSize { get; set; } = 10;
    public int PageIndex { get; set; } = 0;
    public string? Sort { get; set; }
    public bool Descending { get; set; }
    public bool Expand { get; set; }
}
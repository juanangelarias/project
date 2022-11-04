namespace CM.Model.General;

public class PagedResponse<T>
    where T : class
{
    public PagedResponse(IEnumerable<T> items, int count)
    {
        Items = items;
        Count = count;
    }

    public IEnumerable<T> Items { get; set; }
    public int Count { get; set; }
}
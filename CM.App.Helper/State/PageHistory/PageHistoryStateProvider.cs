namespace CM.App.Helper.State;

public class PageHistoryStateProvider : IPageHistoryStateProvider
{
    private readonly List<string> _previousPages;

    public PageHistoryStateProvider()
    {
        _previousPages = new List<string>();
    }

    public void AddPageToHistory(string pageName)
    {
        _previousPages.Add(pageName);
    }

    public string GetGoBackPage()
    {
        return _previousPages.Count > 1
            ? _previousPages.ElementAt((_previousPages.Count - 2))
            : _previousPages.FirstOrDefault() ?? "";
    }

    public bool CanGoBack()
    {
        return _previousPages.Count > 1;
    }
}
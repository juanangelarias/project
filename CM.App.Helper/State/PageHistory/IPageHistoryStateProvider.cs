namespace CM.App.Helper.State;

public interface IPageHistoryStateProvider
{
    void AddPageToHistory(string pageName);
    string GetGoBackPage();
    bool CanGoBack();
}
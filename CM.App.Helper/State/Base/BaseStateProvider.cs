namespace CM.App.Helper.State;

public class BaseStateProvider
{
    public Action? OnChanges;
    protected void NotifyChanges() => OnChanges?.Invoke();
}
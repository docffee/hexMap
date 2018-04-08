
public interface IAction
{
    void TakeControl();
    string ActionName { get; }
    bool HasControl { get; set; }
}


public interface IResource
{
    ResourceType Type { get; }
    int Amount { get; set; }
}

public enum ResourceType
{
    Money,
    Oil,
    Steel
}


public class GenericResource : IResource
{
    private int amount;
    private ResourceType type;

    public GenericResource(int startAmount, ResourceType type)
    {
        this.amount = startAmount;
        this.type = type;
    }

    public int Amount
    {
        get
        {
            return amount;
        }

        set
        {
            amount = value;
        }
    }

    public ResourceType Type
    {
        get
        {
            return type;
        }
    }
}

public interface IEdge {
    float CalcCost(INode source, INode dest);
    INode GetToNode();
}

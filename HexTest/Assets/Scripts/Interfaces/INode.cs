using System.Collections.Generic;

public interface INode {
    int GetCost();
    int[] GetPos();
    List<INode> GetNeighbors();
}

using System.Collections.Generic;

public class HexNode : INode {

    private int cost;
    private int x;
    private int z;
    private List<INode> map;

    public HexNode(int cost, int x, int z, List<INode> map) {
        this.cost = cost;
        this.x = x;
        this.z = z;
        this.map = map;
    }

    public int GetCost() {
        return cost;
    }

    public override bool Equals(object obj) {

        if (obj == null)
            return false;

        if (!GetType().Equals(obj.GetType()))
            return false;

        HexNode comp = obj as HexNode;
        return this.x == comp.x && this.z == comp.z;
    }

    public override int GetHashCode() {
        return x * 37 + z * 36;
    }

    public List<INode> GetNeighbors() {
        List<INode> tempEdges = new List<INode>();

        int index;


        index = map.FindIndex(a => a.GetPos()[0] == this.GetPos()[0] + 1 && a.GetPos()[1] == this.GetPos()[1]);
        if (index >= 0) {
            tempEdges.Add(map[index]);
        }

        //Debug.Log(this.GetPos () + " + " + nodes[index].getPos());


        index = map.FindIndex(a => a.GetPos()[0] == this.GetPos()[0] - 1 && a.GetPos()[1] == this.GetPos()[1]);
        if (index >= 0) {
            tempEdges.Add(map[index]);
        }
        //Debug.Log(this.GetPos () + " + " + nodes[index].getPos());


        index = map.FindIndex(a => a.GetPos()[1] == this.GetPos()[1] + 1 && a.GetPos()[0] == this.GetPos()[0]);
        if (index >= 0) {
            tempEdges.Add(map[index]);
        }
        //Debug.Log(this.GetPos () + " + " + nodes[index].getPos());


        index = map.FindIndex(a => a.GetPos()[1] == this.GetPos()[1] - 1 && a.GetPos()[0] == this.GetPos()[0]);
        if (index >= 0) {
            tempEdges.Add(map[index]);
        }
        //Debug.Log(this.GetPos () + " + " + nodes[index].getPos());

        index = map.FindIndex(a => a.GetPos()[0] == this.GetPos()[0] + 1 && a.GetPos()[1] == this.GetPos()[1] + 1);
        if (index >= 0)
        {
            tempEdges.Add(map[index]);
        }

        index = map.FindIndex(a => a.GetPos()[0] == this.GetPos()[0] - 1 && a.GetPos()[1] == this.GetPos()[1] - 1);
        if (index >= 0)
        {
            tempEdges.Add(map[index]);
        }

        return tempEdges;
    }

    public int[] GetPos() {
        return new int[] { x, z, -(x + z) };
    }
}

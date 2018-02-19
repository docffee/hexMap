using System.Collections.Generic;

public class PathFinder : IPathFinder {
    public List<INode> GetPath(INode source, INode dest, IHeuristic heuristic) {
        Queue<INode> frontier = new Queue<INode>();
        Dictionary<INode, INode> cameFrom = new Dictionary<INode, INode>();
        frontier.Enqueue(source);

        bool found = false;
        while (frontier.Count != 0) {
            INode cur = frontier.Dequeue();
            
            if (cur.Equals(dest)) {
                found = true;
                break;
            }

            foreach (INode next in cur.GetNeighbors()) {
                if (!cameFrom.ContainsKey(next)) {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, cur);
                }
            }
        }

        if (!found)
            return null;

        List<INode> path = new List<INode>();
        path.Add(dest);

        INode current = dest;
        while (!current.Equals(source)) {
            current = cameFrom[current];
            path.Add(current);
        }

        return path;
    }
}

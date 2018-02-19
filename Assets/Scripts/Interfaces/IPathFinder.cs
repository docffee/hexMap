using System.Collections.Generic;

public interface IPathFinder {
    List<INode> GetPath(INode source, INode dest, IHeuristic heuristic);
}

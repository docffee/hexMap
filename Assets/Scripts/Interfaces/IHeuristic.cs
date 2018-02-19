using System.Collections.Generic;

public interface IHeuristic {
    int CalcDistance(INode source, INode dest);
}

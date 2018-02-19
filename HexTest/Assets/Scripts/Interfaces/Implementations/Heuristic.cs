using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristic : IHeuristic {

    public int CalcDistance(INode source, INode dest) {
        int[] pos1 = source.GetPos();
        int[] pos2 = dest.GetPos();

        return Mathf.Max(Mathf.Abs(pos1[0]) - Mathf.Abs(pos2[0]), 
            Mathf.Abs(pos1[1]) - Mathf.Abs(pos2[1]), 
            Mathf.Abs(pos1[2]) - Mathf.Abs(pos2[2]));
    }
}

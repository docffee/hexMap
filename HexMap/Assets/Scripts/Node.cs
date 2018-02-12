using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node{
    private Vector3 pos;
    public List<Node> edges { get; set; }

    public Node(Vector3 pos) {
        this.pos = pos;
    }

    public Vector3 getPos() {
        return pos;
    }
}

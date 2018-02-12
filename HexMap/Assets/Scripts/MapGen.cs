using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour {

    public GameObject prefab;
    public int x;
    public int z;
    private GameObject[,] tiles;
    private List<Node> nodes = new List<Node>();


    void Start() {
        tiles = new GameObject[x, z];
        for (int i = 0; i < z; i++) {
            for (int j = 0; j < x; j++) {
                Vector3 pos = new Vector3((i), 0, (j));
                GameObject hexGo = (GameObject)Instantiate(prefab, pos, Quaternion.identity, this.transform);
                nodes.Add(new Node(pos));
                TextMesh text = hexGo.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
                if (text != null) {
                    text.text = j + "," + i;
                }
            }
        }

        //foreach (Node node in nodes) {
        //    List<Node> tempEdges = new List<Node>();
        //    int index;
        //    //up 
        //    index = PosToIndex((int)node.getPos().x, (int)node.getPos().z + 1);
        //    if (index > 0) {
        //        tempEdges.Add(nodes[index]);
        //    }
        //    //down
        //    index = PosToIndex((int)node.getPos().x, (int)node.getPos().z - 1);
        //    if (index > 0) {
        //        tempEdges.Add(nodes[index]);
        //    }
        //    //left
        //    index = PosToIndex((int)node.getPos().x - 1, (int)node.getPos().z);
        //    if (index > 0) {
        //        tempEdges.Add(nodes[index]);
        //    }
        //    //right
        //    index = PosToIndex((int)node.getPos().x + 1, (int)node.getPos().z);
        //    if (index > 0) {
        //        tempEdges.Add(nodes[index]);
        //    }
        //    node.edges = tempEdges;
        //    //Debug.Log(PosToIndex(1, 1));
        //    foreach(Node node2 in tempEdges) {
        //        Debug.Log(node.getPos().x +", "+ node.getPos().z + " + " + node2.getPos().x + ", "+ node2.getPos().z);
        //    }
        //}

        //foreach (Node node in nodes) {
        //    List<Node> tempEdges = new List<Node>();
        //    foreach (Node node2 in nodes) {
        //        if (node2.getPos().x == node.getPos().x + 1 && node2.getPos().z == node.getPos().z) {
        //            tempEdges.Add(node2);
        //            Debug.Log(node.getPos() + " + " + node2.getPos());
        //        }

        //        if (node2.getPos().x == node.getPos().x - 1 && node2.getPos().z == node.getPos().z) {
        //            tempEdges.Add(node2);
        //            Debug.Log(node.getPos() + " + " + node2.getPos());
        //        }

        //        if (node2.getPos().x == node.getPos().x && node2.getPos().z == node.getPos().z + 1) {
        //            tempEdges.Add(node2);
        //            Debug.Log(node.getPos() + " + " + node2.getPos());
        //        }

        //        if (node2.getPos().x == node.getPos().x && node2.getPos().z == node.getPos().z - 1) {
        //            tempEdges.Add(node2);
        //            Debug.Log(node.getPos() + " + " + node2.getPos());
        //        }
        //    }
        //}

        foreach (Node node in nodes) {
            List<Node> tempEdges = new List<Node>();
            int index;

            if (node.getPos().x < x-1) {
                index = nodes.FindIndex(a => a.getPos().x == node.getPos().x + 1 && a.getPos().z == node.getPos().z);
                tempEdges.Add(nodes[index]);
                Debug.Log(node.getPos() + " + " + nodes[index].getPos());
            }

            if (node.getPos().x > 0) {
                index = nodes.FindIndex(a => a.getPos().x == node.getPos().x - 1 && a.getPos().z == node.getPos().z);
                tempEdges.Add(nodes[index]);
                Debug.Log(node.getPos() + " + " + nodes[index].getPos());
            }

            if (node.getPos().z < x - 1) {
                index = nodes.FindIndex(a => a.getPos().z == node.getPos().z + 1 && a.getPos().x == node.getPos().x);
                tempEdges.Add(nodes[index]);
                Debug.Log(node.getPos() + " + " + nodes[index].getPos());
            }

            if (node.getPos().z > 0) {
                index = nodes.FindIndex(a => a.getPos().z == node.getPos().z - 1 && a.getPos().x == node.getPos().x);
                tempEdges.Add(nodes[index]);
                Debug.Log(node.getPos() + " + " + nodes[index].getPos());
            }

            node.edges = tempEdges;
        }
    }

    public int PosToIndex(int x, int z) {
        if (x > 0 && x < 10 && z > 0 && z < 10) {
            return x * 10 + z;
        } else {
            return -1;
        }
    }

    void Update() {

    }
}

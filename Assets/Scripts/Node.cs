using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node: MonoBehaviour
{ 
    public int x, y;
    public bool Occupied { get; set; } 

    public int GCost { get; set; } // from start node
    public int HCost { get; set; }// from end node
    public int FCost { get; set; }

    public Node ParentNode { get; set; }

    public void InitNode(int x,int y)
    {
        this.x = x;
        this.y = y;
        Occupied = false;
    }
    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node
{
    public float gCost; //distance from starting node
    public float hCost; //distance from end node
    public float fCost = 1; // g + h

    public Node[] friends = new Node[4];//nieghbors, 0 is up, 1 is right, 2 is down, 3 is left

    public Node parentNode;
    public Node pathNode; //this node is closer to the target

    public bool target = false;
    public bool traverable = true;

    public Vector2 position;

    public GameObject occupance;
    private int healthPerFCost = 5; //if a building is on this tile, how much health it has will detirmin how much it costs to go threw it as a path
    public int pathDistacneFromBase = 0;

    public GameObject resource;
    public bool collectable;

    public Node(int yPos, int xPos, Vector2 targetPos)
    {
        position = new Vector2(xPos, yPos);

        if(position == targetPos)
        {
            target = true;
        }
        else
        {
            int a = (int)Math.Abs(position.x - targetPos.x);
            int b = (int)Math.Abs(position.y - targetPos.y);
            float c = (float)Math.Sqrt(Math.Abs(a + b));
            hCost = c;
        }
    }

    public void MakeFriendsCollectable()
    {
        foreach(Node f in friends) {
            if (f == null) continue;
            if (!f.traverable) continue;
            f.collectable = true;
        }
    }

    public void AssignFriends(Node left, Node down)
    {
        if (left != null)
        {
            friends[3] = left;
            left.friends[1] = this;
        }
        
        if(down != null)
        {
            friends[2] = down;
            down.friends[0] = this;
        }
    }

    public int distanceFrom(Node node)
    {
        int a = (int)Math.Abs(position.x - node.position.x);
        int b = (int)Math.Abs(position.y - node.position.y);
        return Math.Abs(a + b);
    }

    public void SetFCost()
    {
        if (!target)
        {
            float temp = 0;
            if (occupance != null)
                temp = occupance.GetComponent<Building>().currentHealth / healthPerFCost;
            fCost = gCost + hCost  + temp;
        }
        else
            fCost = 0;

    }

     public override string ToString()
     {
        return "Node: " + position;
     }

    public void SetPath(Node n)
    {
        if (n == this)
            return;
        parentNode.pathNode = this;
        parentNode.SetPath(n);
    }

    public void SeperateChildren(int i)
    {
        if(pathNode != null)
        {
            pathNode.parentNode = null;
            pathNode = null;
        }
        if (parentNode == null)
            return;
        if(i > 100)
        {
            Debug.Log(this);
            Debug.LogError("Over 100");
            return;
        }
        i++;
        parentNode.SeperateChildren(i);
    }

    public void Disown()
    {
        pathNode = null;
        parentNode = null;
    }

    public int GetDistance()
    {
        if (pathDistacneFromBase > 0)
            return pathDistacneFromBase;
        else
        {
            CalculatePathDistacneToTarget();
            if (pathDistacneFromBase < 0) Debug.Log("SOmething wrong happened ekdenjerijnoeihrgboeiahbvroeahbvoiehb");
            return pathDistacneFromBase;
        }
    }

    public int CalculatePathDistacneToTarget()
    {
        if (pathNode == null)
        {
            //Debug.LogError("No path node");
            return 0;
        }
        if (pathNode.target)
        {
            return GetTileCost();
        }
        pathDistacneFromBase =  GetTileCost() + pathNode.CalculatePathDistacneToTarget();
        return pathDistacneFromBase;
    }

    private int GetTileCost()
    {
        int tileCost;
        if (occupance != null)
            tileCost = occupance.GetComponent<Building>().currentHealth / healthPerFCost;
        else
            tileCost = 1;
        return tileCost;
    }
}

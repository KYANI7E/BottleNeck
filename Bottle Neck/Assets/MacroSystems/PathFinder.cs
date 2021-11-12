using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathFinder : MonoBehaviour
{
    [SerializeField] int mapHieght = 0;
    [SerializeField] int mapLength = 0;
    public Node[,] nodes; //the first is y than x
    //
    // it will be maek inthis order starting at bottom left corner
    // nodes[*y cordinate, *x cordinate]
    // 31 32 33 34 35 36 37 38 39 30
    // 21 22 23 24 25 26 27 28 29 20
    // 11 12 13 14 15 16 17 18 19 20
    // 1  2  3  4  5  6  7  8  9  10

    [SerializeField] public GameObject target;
    [SerializeField] public List<GameObject> occupancy; //something on the tile like either a mountain or a tower or building
    [SerializeField] public GameObject[] unwalkable;
    [SerializeField] public GameObject[] resources;

    // Start is called before the first frame update
    void Awake()
    {
        //this creates all of the nodes and assaigns there nieghbors
        nodes = new Node[mapHieght,mapLength];
        for(int i = 0; i < mapHieght; i++)
        {
            Node holdLeftNode = null;
            for (int j = 0; j < mapLength; j++)
            {
                Node holding = new Node(i, j, RoundCords(target.transform.position));
                nodes[i, j] = holding;
                if(i > 0) holding.AssignFriends(holdLeftNode, nodes[i - 1, j]);
                holdLeftNode = holding;
            }
        }

        Vector2 p = RoundCords(target.transform.position);
        nodes[(int)p.y, (int)p.x].occupance = target;

        unwalkable = GameObject.FindGameObjectsWithTag("Unwalkable");

        foreach (GameObject g in unwalkable)
        {
            Vector2 pos = RoundCords(g.transform.position);
            nodes[(int)pos.y, (int)pos.x].traverable = false;
        }

        resources = GameObject.FindGameObjectsWithTag("Resource");

        foreach (GameObject g in resources) {
            Vector2 pos = RoundCords(g.transform.position);
            nodes[(int)pos.y, (int)pos.x].traverable = false;
            nodes[(int)pos.y, (int)pos.x].collectable = false;
            nodes[(int)pos.y, (int)pos.x].resource = g;
            nodes[(int)pos.y, (int)pos.x].MakeFriendsCollectable();
        }


    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateNodes();
        }
    }

    public Vector2 RoundCords(Vector2 cords)
    {
        int y = (int)Math.Round(cords.y);
        if (y >= mapHieght) y = mapHieght - 1;
        int x = (int)Math.Round(cords.x);
        if (x >= mapLength) x = mapLength - 1;
        return new Vector2(x, y);
    }

    public void ReUpdatePath(GameObject obj, Vector2 vec)
    {
        Vector2 pos = RoundCords(vec);
        Node temp = nodes[(int)pos.y, (int)pos.x];
        temp.occupance = obj;
        Invoke("UpdateNodes", .01f);
    }

    public Node FindPath(Vector3 pos)
    {

        int y = (int)Math.Round(pos.y);
        if (y >= mapHieght) y = mapHieght - 1;
        int x = (int)Math.Round(pos.x);
        if (x >= mapLength) x = mapLength - 1;

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        Node startingNode = nodes[y, x];
        open.Add(startingNode);

        while (true)
        {
            Node current = null;
            foreach (Node node in open)
            {
                if (current == null || node.fCost < current.fCost)
                    current = node;
            }
            open.Remove(current);
            closed.Add(current);
            if(current == null)
            {
                Debug.Log("No Path");
                return null;
            }
            if (current.target)
            {
                current.SetPath(startingNode);
                return startingNode;
            }

            foreach (Node friend in current.friends)
            {
                if (friend == null) continue;
                if (!friend.traverable || closed.Contains(friend)) continue;
                if (!open.Contains(friend))
                {
                    friend.gCost = friend.distanceFrom(startingNode);
                    friend.SetFCost();
                    friend.parentNode = current;
                    if (!open.Contains(friend))
                        open.Add(friend);
                }
            }
        }
    }


    public void UpdateNodes()
    {
        foreach (Node n in nodes)
        {
            n.Disown();
        }
    }

}

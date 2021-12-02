using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNode : MonoBehaviour
{
    public GameObject theGameObject;
    public Vector2 position;

    public List<GameObject> refrences = new List<GameObject>();
    public List<Connector> connectors = new List<Connector>();
    public List<GameObject> nodesInRange = new List<GameObject>();
    public List<GameObject> resourcesInRange = new List<GameObject>();
    public List<GameObject> recieversInRange = new List<GameObject>();
    public float range;

    public List<GameObject> allRecievers = new List<GameObject>();
    public List<GameObject> allNodes = new List<GameObject>();

    private LineRenderer lr;

    public int connectionssss;

    public struct Connector
    {
        public GameObject desination;
        public GameObject nextNode;
        public float distance;

        public Connector(GameObject g, GameObject n, float f)
        { desination = g; nextNode = n; distance = f; }

        public Connector(Connector con, GameObject b, float f)
        {
            desination = con.desination;
            nextNode = b;
            distance = con.distance + f;
        }

        public bool CheckDest(GameObject g)
        {
            if (desination.Equals(g))
                return true;
            return false;
        }
        public bool CheckDest(Connector con)
        {
            if (desination.Equals(con.desination))
                return true;
            return false;
        }
        public bool CheckNextNode(GameObject g)
        {
            if (nextNode.Equals(g))
                return true;
            return false;
        }
        public bool GreaterThan(Connector con, float dis)
        {
            if (distance > con.distance + dis)
                return true;
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        theGameObject = this.gameObject;
        position = transform.position;
        lr = GetComponent<LineRenderer>();
    }

    public void CheckForRecievers()
    {
        lr = GetComponent<LineRenderer>();
        
        foreach (GameObject r in allRecievers) {
            float dis = Vector2.Distance(r.transform.position, transform.position);
            if(dis <= range) {
                recieversInRange.Add(r);
                NewConnection(r, dis);
            }
        }

        //this has nothing to do with recievers btw
        foreach (GameObject n in allNodes) {
            float dis = Vector2.Distance(n.transform.position, this.transform.position);
            if (dis <= range) nodesInRange.Add(n);
        }
        foreach (GameObject n in allNodes) {
            n.GetComponent<TNode>().NewNode(gameObject);
        }

    }

    public virtual void DrawLine()
    {
        Transform[] points = new Transform[(nodesInRange.Count + resourcesInRange.Count + recieversInRange.Count) * 2];

        lr.positionCount = points.Length;

        int j = 0;
        foreach (GameObject t in nodesInRange) {
            points[j] = t.transform;
            points[j + 1] = this.transform;
            j += 2;
        }
        foreach (GameObject t in resourcesInRange) {
            points[j] = t.transform;
            points[j + 1] = this.transform;
            j += 2;
        }
        foreach (GameObject t in recieversInRange) {
            points[j] = t.transform;
            points[j + 1] = this.transform;
            j += 2;
        }

        for (int i = 0; i < points.Length; i += 2) {
            lr.SetPosition(i, points[i].position);
            lr.SetPosition(i + 1, transform.position);

        }
    }

    public void NewResiever(GameObject res)
    {
        float dis = Vector2.Distance(res.transform.position, this.transform.position);
        if (dis > range) return;

        resourcesInRange.Add(res);
        
        NewConnection(res, dis);
    }

    public void NewNode(GameObject node)
    {
        if (Vector2.Distance(node.transform.position, this.transform.position) > range) return;
        if (node.Equals(gameObject)) return;

        nodesInRange.Add(node);
        GiveConnections(node);
    }

    public virtual void GiveConnections(GameObject node)
    {
        float dis = Vector2.Distance(node.transform.position, this.transform.position);
        TNode newNode = node.GetComponent<TNode>();
        foreach (Connector c in connectors) {
            if(newNode.CheckIfShorterConnection(c, dis, this.gameObject)) {
                break;
            }
            if(!newNode.CheckIfConnectionExists(c))
                newNode.NewConnection(c, this.gameObject, dis);
        }
        Debug.Log(newNode.connectors.Count);
        DrawLine();

    }

    public virtual void GiveConnection(Connector con, GameObject node)
    {
        float dis = Vector2.Distance(node.transform.position, this.transform.position);
        TNode newNode = node.GetComponent<TNode>();
         if (newNode.CheckIfShorterConnection(con, dis, this.gameObject)) {
                return;
            }
            if (!newNode.CheckIfConnectionExists(con))
                newNode.NewConnection(con, this.gameObject, dis);
     
        DrawLine();
    }

    public bool CheckIfConnectionExists(Connector con)
    {
        foreach (Connector g in connectors) {
            if (g.CheckDest(con))
                return true;
        }
        return false;
    }

    public bool CheckIfShorterConnection(Connector con, float dis, GameObject caller)
    {
        foreach (Connector g in connectors) {
            if (g.CheckDest(con) && g.GreaterThan(con, dis)) {
                NewConnection(con, caller, dis);
                connectors.Remove(g);
                return true;
            }
        }
        return false;
    }

    public void NewConnection(Connector con, GameObject ob, float dis)
    {

        Connector connect = new Connector(con, ob, dis);
            Debug.Log(nodesInRange.Count + " Range count");
        foreach (Connector c in connectors)
            if (c.CheckDest(con)) {
                connectors.Remove(c);
                break;
            }
        connectors.Add(connect);
        foreach(GameObject g in nodesInRange) {
            GiveConnection(connect, g);
        }

        connectionssss = connectors.Count;
    }
    public void NewConnection(GameObject ob, float dis)
    {
        Connector connect = new Connector(ob, ob, dis);
        foreach (Connector c in connectors)
            if (c.CheckDest(ob)) {
                connectors.Remove(c);
                break;
            }
        connectors.Add(connect);
        foreach (GameObject g in nodesInRange) {
            GiveConnection(connect, g);
        }
        connectionssss = connectors.Count;
    }
}

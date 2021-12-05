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

    public LineRenderer lr;
    public Shop shop;

    public int connectionssss;
    public float speedOfThing;

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

    public virtual void CheckForRecievers()
    {
        theGameObject = this.gameObject;

        lr = GetComponent<LineRenderer>();
        

        //this has nothing to do with recievers btw
        foreach (GameObject n in allNodes) {
            if (n.GetComponent<ResourceGiver>() != null && GetComponent<ResourceGiver>() != null) continue;
            float dis = Vector2.Distance(n.transform.position, this.transform.position);
            if (dis <= range && RaycastCheck(n)) nodesInRange.Add(n);
        }
        foreach (GameObject n in allNodes) {
            n.GetComponent<TNode>().NewNode(gameObject);
        }

        
        //This is the reciever part
        foreach (GameObject r in allRecievers) {
            float dis = Vector2.Distance(r.transform.position, transform.position);
            if(dis <= range && RaycastCheck(r)) {
                recieversInRange.Add(r);
                NewConnection(r, dis);
            }
        }

        DrawLine();

    }

    private bool RaycastCheck(GameObject thing)
    {
        float dis = Vector2.Distance(thing.transform.position, transform.position);
        if (dis > range) dis = range;
        Vector2 raycastDir = thing.transform.position - transform.position;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, raycastDir, dis, LayerMask.GetMask("Transport"));
        //Debug.DrawRay(transform.position, raycastDir, Color.green);

        bool good = false;
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].collider != null) {
                if (hits[i].collider.gameObject.name.Equals("Mountain Shit")) {
                    good = false;;
                    break;
                }
                if (hits[i].collider.gameObject.Equals(thing))
                    good = true;
                else if (hits[i].collider.gameObject.name.Equals("Trans Collider")) {
                    if (hits[i].collider.gameObject.transform.parent.gameObject.Equals(thing))
                        good = true;
                }
            }
        }
        if (good) return true;
        else return false;
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
        if (dis > range || !RaycastCheck(res)) return;

        recieversInRange.Add(res);
        
        NewConnection(res, dis);
    }

    public void NewNode(GameObject node)
    {
        if (Vector2.Distance(node.transform.position, this.transform.position) > range) return;
        if (!RaycastCheck(node)) return;
        if (node.Equals(gameObject)) return;
        if (node.GetComponent<ResourceGiver>() != null && GetComponent<ResourceGiver>() != null) return;
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
        ob.GetComponent<TNode>().refrences.Add(this.gameObject);
        Connector connect = new Connector(con, ob, dis);
        foreach (Connector c in connectors)
            if (c.CheckDest(con)) {
                connectors.Remove(c);
                break;
            }
        connectors.Add(connect);
        foreach(GameObject g in nodesInRange) {
            GiveConnection(connect, g);
        }
        DrawLine();
        connectionssss = connectors.Count;
    }

    public void NewConnection(GameObject ob, float dis)
    {
        ob.GetComponent<Receiver>().connectedNodes.Add(this);
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
        DrawLine();
        connectionssss = connectors.Count;
    }

    public void Die()
    {
        foreach(GameObject n in nodesInRange) {
            n.GetComponent<TNode>().nodesInRange.Remove(this.gameObject);
            n.GetComponent<TNode>().DrawLine();
            if (refrences.Contains(n))
                n.GetComponent<TNode>().PathBroke(this.gameObject, new List<GameObject>());
        }
        shop.transports.Remove(this);
        shop.allLined.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void PathBroke(GameObject ob, List<GameObject> dest)
    {
        bool checkIfPath = false;
        List<Connector> tempConnections = new List<Connector>();

        if(dest.Count > 0)
            foreach(Connector c in connectors) {
                foreach(GameObject d in dest)
                    if (c.CheckDest(d) && c.CheckNextNode(ob)) {
                        tempConnections.Add(c);
                        checkIfPath = true;
                        break;
                    }
            }
        else
            foreach(Connector c in connectors) {
                if (c.CheckNextNode(ob)) {
                    tempConnections.Add(c);
                    checkIfPath = true;
                    dest.Add(c.desination);
                }
            }

        foreach (Connector c in tempConnections) connectors.Remove(c);

        if (checkIfPath)
            if (GetComponent<ResourceGiver>() == null) {
                foreach (GameObject n in refrences) {
                    if (n == null) continue;
                    n.GetComponent<TNode>().PathBroke(this.gameObject, new List<GameObject>());
                }
            } else {
                foreach (GameObject g in nodesInRange) {
                    g.GetComponent<TNode>().GiveConnections(this.gameObject);
                }
            }
        else {
            GiveConnections(this.gameObject);
        }
        DrawLine();

    }
}

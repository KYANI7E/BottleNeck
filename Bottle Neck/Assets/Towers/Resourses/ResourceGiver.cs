using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGiver : TNode
{

    public Queue<GameObject> sendingToo = new Queue<GameObject>();

    public GameObject resource;
    public float mineRate;
    private float coolDown;

    public string type;
    

    // Start is called before the first frame update
    void Start()
    {
        theGameObject = gameObject;
        //foreach(TNode t in transports) {
        //    if(Vector2.Distance(t.gameObject.transform.position, transform.position) < t.range) {
        //        transInRange.Add(t);
        //        t.resourcesInRange.Add(this.gameObject);
        //        t.DrawLine();
        //    }
        //}

        //TakeDistances();


    }
    public bool say = false;
    void Update()
    {
        if (say) {
            Debug.Log(connectors.Count + " ");
            say = false;
        }
        if (coolDown < 0 && sendingToo.Count > 0) {
            Mined(sendingToo.Dequeue());
            coolDown = mineRate;
        }
        coolDown -= Time.deltaTime;
    }

    private void Mined(GameObject gg)
    {
        GameObject res = GameObject.Instantiate(resource, transform.position, Quaternion.identity);
        res.GetComponent<Resource>().destination = gg;
        foreach(TNode.Connector t in connectors) {
            if(t.desination == gg) {
                res.GetComponent<Resource>().currentNode = t.nextNode;
            }
        }
    }

    public void NewReciever(GameObject g)
    {
        TNode.Connector temp = new TNode.Connector(null, null, 10000);
        bool videCheck = true;
        foreach(GameObject f in nodesInRange) {
            TNode t = f.GetComponent<TNode>();
            float dis = Vector2.Distance(transform.position, t.theGameObject.transform.position);
            foreach(TNode.Connector d in t.connectors) {
                if (d.CheckDest(g) && d.distance < temp.distance - dis) {
                    temp = new TNode.Connector(g, t.theGameObject, d.distance + dis);
                    videCheck = false;
                    break;
                }
            }
        }
        if (videCheck) return;
        connectors.Add(temp);
    }

    public override void CheckForRecievers() {
        theGameObject = this.gameObject;


        //this has nothing to do with recievers btw
        foreach (GameObject n in allNodes) {
            float dis = Vector2.Distance(n.transform.position, this.transform.position);
            if (dis <= range) nodesInRange.Add(n);
        }
        foreach (GameObject n in allNodes) {
            n.GetComponent<TNode>().NewNode(gameObject);
        }
    }
    public override void DrawLine() { }
    public override void GiveConnections(GameObject node) { }
    public override void GiveConnection(Connector con, GameObject node) { }
}
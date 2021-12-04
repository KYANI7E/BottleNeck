using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGiver : TNode
{

    public GameObject sendingToo;

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
        if (coolDown < 0 && sendingToo != null) {
            Mined(sendingToo);
            sendingToo = null;
            coolDown = mineRate;
        }
        if(!shop.paused) coolDown -= Time.deltaTime;
    }

    private void Mined(GameObject gg)
    {
        GameObject res = GameObject.Instantiate(resource, transform.position, Quaternion.identity);
        res.GetComponent<Resource>().destination = gg;
        res.GetComponent<Resource>().shop = shop;
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

    public override void DrawLine()
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
            if(!type.Equals(t.GetComponent<Receiver>().type)) {
                points[j] = this.transform;
                points[j+1] = this.transform;
                j += 2;
                continue;
            }
            
            points[j] = t.transform;
            points[j + 1] = this.transform;
            j += 2;
        }


        for (int i = 0; i < points.Length; i += 2) {
            if (points[i] == null) continue;
            lr.SetPosition(i, points[i].position);
            lr.SetPosition(i + 1, transform.position);

        }
    }

    public override void GiveConnections(GameObject node) { }
    public override void GiveConnection(Connector con, GameObject node) { }
}
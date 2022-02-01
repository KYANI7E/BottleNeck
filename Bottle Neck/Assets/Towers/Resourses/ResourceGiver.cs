using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResourceGiver : TNode
{

    public GameObject sendingToo;

    public GameObject resource;
    public float mineRate;
    private float coolDown;

    public string type;


    public Image cir;

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
        if (!shop.paused) {
            coolDown -= Time.deltaTime;
            float opo = mineRate - coolDown;
            cir.fillAmount = opo / mineRate;
        }
    }

    private void Mined(GameObject gg)
    {
        GameObject res = GameObject.Instantiate(resource, transform.position, Quaternion.identity);
        res.GetComponent<Resource>().destination = gg;
        res.GetComponent<Resource>().shop = shop;
            if(connectors.ContainsKey(gg)) {
                res.GetComponent<Resource>().speed = speedOfThing;
                res.GetComponent<Resource>().currentNode = connectors[gg].nextNode;
        }
    }

    public void NewReciever(GameObject g)
    {
        TNode.Connector temp = new TNode.Connector(null, null, 10000);
        bool videCheck = true;
        foreach(GameObject f in nodesInRange) {
            TNode t = f.GetComponent<TNode>();
            float dis = Vector2.Distance(transform.position, t.theGameObject.transform.position);
            if (connectors.ContainsKey(g))
                if (connectors[g].distance < temp.distance - dis) {
                    temp = new TNode.Connector(g, t.theGameObject, connectors[g].distance + dis);
                    videCheck = false;
                    break;
                }
        }
        if (videCheck) return;
        connectors.Add(temp.desination, temp);
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
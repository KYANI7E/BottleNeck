using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGiver : MonoBehaviour
{

    public List<Transporter.DistanceData> myDistances = new List<Transporter.DistanceData>();
    public List<Transporter> transports = new List<Transporter>();
    public List<Transporter> transInRange = new List<Transporter>();

    public Queue<GameObject> sendingToo = new Queue<GameObject>();

    public GameObject resource;
    public float mineRate;
    private float coolDown;

    public string type;
    

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transporter t in transports) {
            if(Vector2.Distance(t.gameObject.transform.position, transform.position) < t.range) {
                transInRange.Add(t);
                t.otherInRange.Add(this.gameObject);
                t.SetUpLine();
            }
        }

        TakeDistances();


    }

    void Update()
    {
        if(coolDown < 0 && sendingToo.Count > 0) {
            Mined(sendingToo.Dequeue());
            coolDown = mineRate;
        }
        coolDown -= Time.deltaTime;
    }

    private void Mined(GameObject gg)
    {
        GameObject res = GameObject.Instantiate(resource, transform.position, Quaternion.identity);
        res.GetComponent<Resource>().destination = gg;
        foreach(Transporter.DistanceData t in myDistances) {
            if(t.receiver == gg) {
                res.GetComponent<Resource>().currentNode = t.nextNode;
            }
        }
    }

    public void NewTransporter(Transporter tran)
    {
        if (Vector2.Distance(tran.gameObject.transform.position, transform.position) < tran.range) {
            tran.otherInRange.Add(this.gameObject);
            tran.SetUpLine();
            transInRange.Add(tran);
            GetBestDistances(tran);
        }
    }

    private void TakeDistances()
    {
        bool first = true;
        foreach(Transporter t in transInRange) {
            if (first) {
                first = false;
                FirstCopy(t);
                continue;
            }
            GetBestDistances(t);
        }
    }

    private void FirstCopy(Transporter tran)
    {
        float dis = Vector2.Distance(tran.theGameObject.transform.position, transform.position);
        foreach (Transporter.DistanceData t in tran.myDistances) {
            myDistances.Add(new Transporter.DistanceData(t.receiver, tran.theGameObject, t.distance + dis));
        }
    }

    public void GetBestDistances(Transporter tran)
    {
        foreach(Transporter.DistanceData t in tran.myDistances) {
            float dis = Vector2.Distance(tran.theGameObject.transform.position, transform.position);
            bool videCheck = true;
            foreach (Transporter.DistanceData v in myDistances) {
                if(t.receiver == v.receiver) {
                    videCheck = false;
                    if(t.distance + dis < v.distance) {
                        myDistances.Remove(v);
                        myDistances.Add(new Transporter.DistanceData(t.receiver, tran.theGameObject, t.distance + dis));
                        break;
                    }
                }
            }
            if (videCheck) {
                myDistances.Add(new Transporter.DistanceData(t.receiver, tran.theGameObject, t.distance + dis));
            }
            //Debug.Log(myDistances.Count + "Es count");
            //foreach (Transporter.DistanceData d in myDistances) {
            //    Debug.Log(d.distance + " YETETETE");
            //}
        }
    }
}

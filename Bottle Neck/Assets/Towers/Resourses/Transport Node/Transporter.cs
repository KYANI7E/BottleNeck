using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{

    public GameObject theGameObject;

    public List<GameObject> transportersInRange;
    public float range;
    public GameObject[] allTransporters;

    public List<GameObject> receivers = new List<GameObject>();

    public List<DistanceData> myDistances = new List<DistanceData>();
    public List<DistanceData> myDistancesLast = new List<DistanceData>();
    public int amountOfDistances;

    public Vector2 position;
    public struct DistanceData
    {
        public GameObject receiver;
        public GameObject nextNode;
        public float distance;
        public DistanceData(GameObject receivere, GameObject nextNodee, float dis)
        {
            receiver = receivere;
            nextNode = nextNodee;
            distance = dis;
        }
    }

    private LineRenderer lr;
    public List<GameObject> otherInRange = new List<GameObject>();

    private void Awake()
    {
        position = transform.position;
        lr = GetComponent<LineRenderer>();
    }


    // Start is called before the first frame update
    //void Start()
    //{
    //    theGameObject = this.gameObject;
    //    allTransporters = GameObject.FindGameObjectsWithTag("Transporter");

    //    foreach (GameObject t in allTransporters)
    //    {
    //        if (t == this.gameObject) continue;
    //        if (Vector2.Distance(t.transform.position, transform.position) < range) {
    //            t.GetComponent<Transporter>().NewDistance(this.gameObject);
    //            if (!transportersInRange.Contains(t))
    //                transportersInRange.Add(t);
    //        }
    //    }

    //    GetDistances();
    //    foreach(GameObject g in transportersInRange) {
    //        g.GetComponent<Transporter>().CheckPathsAgain();
    //    }
    //    amountOfDistances = myDistances.Count;
    //    Debug.Log("Made data Distance");
    //}

    public void Die()
    {
        foreach(GameObject g in transportersInRange) {
            Debug.Log("Dieedededed");
            g.GetComponent<Transporter>().transportersInRange.Remove(this.gameObject);
            g.GetComponent<Transporter>().SetUpLine();
        }
        Destroy(this.gameObject);
    }

    public void Staterr()
    {
        theGameObject = this.gameObject;
        allTransporters = GameObject.FindGameObjectsWithTag("Transporter");

        foreach (GameObject t in allTransporters) {
            if (t == this.gameObject) continue;
            if (Vector2.Distance(t.transform.position, transform.position) < range) {
                t.GetComponent<Transporter>().NewDistance(this.gameObject);
                if(!transportersInRange.Contains(t))
                    transportersInRange.Add(t);
            }
        }

        GetDistances();

        //foreach (GameObject g in transportersInRange) {
        //    g.GetComponent<Transporter>().CheckPathsAgain();
        //}
        //amountOfDistances = myDistances.Count;


        foreach(DistanceData d in myDistances) {
            bool videCheck = true;
            foreach(GameObject g in transportersInRange) {
                foreach(DistanceData t in g.GetComponent<Transporter>().myDistances) {
                    if(d.receiver == t.receiver) {
                        if(d.distance < t.distance) {
                            g.GetComponent<Transporter>().GetDistance(d.receiver);
                            Debug.Log("Recalutalalalor" + g.GetComponent<Transporter>().myDistances.Count);
                            videCheck = false;
                            break;
                        }
                    }
                }
                if (videCheck) {
                    g.GetComponent<Transporter>().GetDistance(d.receiver);
                    Debug.Log("Recalutalalalor" + g.GetComponent<Transporter>().myDistances.Count);
                }

            }
        }


        SetUpLine();

        //foreach(DistanceData d in myDistances) {
        //    Debug.Log(d.distance);
        //}
    }

    public void SetUpLine()
    {
        Transform[] points = new Transform[(transportersInRange.Count + otherInRange.Count) * 2];

        lr.positionCount = points.Length;

        int j = 0;
        foreach(GameObject t in transportersInRange) {
            points[j] = t.transform;
            points[j + 1] = this.transform;
            j += 2;
        }
        foreach(GameObject t in otherInRange) {
            points[j] = t.transform;
            points[j + 1] = this.transform;
            j += 2;
        }

        for (int i = 0; i < points.Length; i+=2) {
            lr.SetPosition(i, points[i].position);
            lr.SetPosition(i+1, transform.position);

        }
    }

    public void CheckPathsAgain()
    {
        GetDistances();
        SetUpLine();
        if (myDistances != myDistancesLast) {
            myDistancesLast = myDistances;
            foreach(GameObject g in transportersInRange) {
                CheckPathsAgain();
            }
        }
    }

    public void NewDistance(GameObject gg)
    {
        transportersInRange.Add(gg);
        GetDistance(gg);
    }

    private void GetDistances()
    {
        if (receivers.Count == 0) return;
        foreach (GameObject t in receivers)
        {
            GetDistance(t);
        }

    }

    public void GetDistance(GameObject t)
    {
        float dis = Vector2.Distance(t.transform.position, transform.position);
        if (dis < range) {
            if(receivers.Contains(t))
                otherInRange.Add(t);
            foreach (DistanceData d in myDistances) {
                if (d.nextNode == t) {
                    myDistances.Remove(d);
                    break;
                }
            }
            myDistances.Add(new DistanceData(t, t, dis));
            return;
        }

        float shortest = 10000;
        GameObject tempShortest = null;
        foreach (GameObject r in transportersInRange) {

            float dis2 = Vector2.Distance(r.transform.position, transform.position);
            foreach (DistanceData d in r.GetComponent<Transporter>().myDistances) {
                if (t == d.receiver && d.distance + dis2 < shortest) {
                    shortest = d.distance + dis2;
                    tempShortest = r;
                    break;
                }
            }
        }

        if (tempShortest != null) {
            List<DistanceData> temData = new List<DistanceData>();
            foreach (DistanceData d in myDistances) {
                if (d.nextNode == tempShortest) {
                    temData.Add(d); 
                }
            }
            foreach (DistanceData d in temData)
                myDistances.Remove(d);

            myDistances.Add(new DistanceData(t, tempShortest, shortest));
        }

        List<DistanceData> tempData = new List<DistanceData>();
        foreach(DistanceData d in myDistances) {
            foreach(DistanceData w in myDistances) {
                if (d.receiver == w.receiver)
                    if (d.distance < w.distance)
                        tempData.Add(w);
            }
        }

        foreach (DistanceData d in tempData)
            myDistances.Remove(d);

        SetUpLine();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{

    public GameObject theGameObject;

    public List<GameObject> transportersInRange;
    public List<GameObject> pointed = new List<GameObject>();
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
        //public bool Equals(DistanceData d)
        //{
        //    if (receiver.Equals(d.receiver)
        //        && nextNode.Equals(d.nextNode)
        //        && distance == d.distance)
        //        return true;
        //    else
        //        return false;
        //}
        public bool SameDest(GameObject g)
        {
            if (receiver.Equals(g))
                return true;
            else
                return false;
        }
        public bool ChechNext(GameObject g)
        {
            if (nextNode.Equals(g))
                return true;
            else
                return false;
        }
    }

    private LineRenderer lr;
    public List<GameObject> otherInRange = new List<GameObject>();

    private void Awake()
    {
        position = transform.position;
        lr = GetComponent<LineRenderer>();
    }
    public bool say = false;
    public void Update()
    {
        if (say) {
            Debug.Log(myDistances.Count + " ");
            say = false;
        }
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
            g.GetComponent<Transporter>().RemovePath(gameObject, null);
            g.GetComponent<Transporter>().DrawLine();
        }
        Destroy(this.gameObject);
    }

    public void RemovePath(GameObject g, GameObject r)
    {
        transportersInRange.Remove(g);
        foreach(DistanceData d in myDistances) {
            if (r == null) {
                if (d.ChechNext(g)) {
                    myDistances.Remove(d);
                    foreach (GameObject f in transportersInRange) {
                        f.GetComponent<Transporter>().RemovePath(gameObject, d.receiver);
                    }
                    break;
                }
            } else {
                if(d.ChechNext(g) && d.SameDest(r)) {
                    foreach (GameObject f in transportersInRange) {
                        f.GetComponent<Transporter>().RemovePath(gameObject, d.receiver);
                    }
                    break;
                }
            }

        }
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


        DrawLine();

        //foreach(DistanceData d in myDistances) {
        //    Debug.Log(d.distance);
        //}
    }

    public void DrawLine()
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
    public void NewReciever(GameObject g)
    {
        GetDistance(g);
        foreach(GameObject f in transportersInRange) {
            if (f.GetComponent<Transporter>().myDistances.Count != myDistances.Count)
                CheckPathsAgain(g);
        }
        
    }
    public void CheckPathsAgain(GameObject g)
    {
        GetDistances();
        DrawLine();
        
        foreach(GameObject f in otherInRange) {
            if (f.GetComponent<ResourceGiver>() != null) {
                //f.GetComponent<ResourceGiver>().NewReciever(g);
                f.GetComponent<ResourceGiver>().TakeDistances();
            }
        }
        foreach (GameObject f in transportersInRange) {
            //Debug.Log(f.GetComponent<Transporter>().myDistances.Count + "     " + myDistances.Count);
            if (f.GetComponent<Transporter>().myDistances.Count < myDistances.Count) {
                f.GetComponent<Transporter>().CheckPathsAgain(g);
                Debug.Log("Checking Again" + f.GetComponent<Transporter>().myDistances.Count);
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
            if(receivers.Contains(t) && !otherInRange.Contains(t))
                otherInRange.Add(t);
            foreach (DistanceData d in myDistances) {
                if (d.nextNode == t) {
                    myDistances.Remove(d);
                    break;
                }
            }
            myDistances.Add(new DistanceData(t, t, dis));
            DrawLine(); 
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
            tempShortest.GetComponent<Transporter>().pointed.Add(this.gameObject);
        } else {
            Debug.Log("Found Noting");
        }

        //List<DistanceData> tempData = new List<DistanceData>();
        //foreach(DistanceData d in myDistances) {
        //    foreach(DistanceData w in myDistances) {
        //        if (d.SameDest(w.receiver) && !d.Equals(w))
        //            if (d.distance < w.distance)
        //                tempData.Add(w);
        //            else
        //                tempData.Add(d);
        //    }
        //}

        //foreach (DistanceData d in tempData)
        //    myDistances.Remove(d);

        Debug.Log(myDistances.Count);
        amountOfDistances = myDistances.Count;
        DrawLine();
    }

}

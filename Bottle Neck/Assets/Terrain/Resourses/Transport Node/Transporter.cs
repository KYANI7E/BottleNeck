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

    void Awake()
    {
        position = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        theGameObject = this.gameObject;
        allTransporters = GameObject.FindGameObjectsWithTag("Transporter");

        foreach (GameObject t in allTransporters)
        {
            if (t == this.gameObject) continue;
            if (Vector2.Distance(t.transform.position, transform.position) < range) {
                t.GetComponent<Transporter>().NewDistance(this.gameObject);
                transportersInRange.Add(t);
            }
        }

        GetDistances();
        foreach(GameObject g in transportersInRange) {
            g.GetComponent<Transporter>().CheckPathsAgain();
        }
        amountOfDistances = myDistances.Count;
        Debug.Log("Made data Distance");
    }

    public void Staterr()
    {
        theGameObject = this.gameObject;
        allTransporters = GameObject.FindGameObjectsWithTag("Transporter");

        foreach (GameObject t in allTransporters) {
            if (t == this.gameObject) continue;
            if (Vector2.Distance(t.transform.position, transform.position) < range) {
                t.GetComponent<Transporter>().NewDistance(this.gameObject);
                transportersInRange.Add(t);
                t.GetComponent<Transporter>().transportersInRange.Add(this.gameObject);
            }
        }

        GetDistances();
        foreach (GameObject g in transportersInRange) {
            g.GetComponent<Transporter>().CheckPathsAgain();
        }
        amountOfDistances = myDistances.Count;
        Debug.Log("Made data Distance");
    }

    public void CheckPathsAgain()
    {
        GetDistances();
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
            foreach(DistanceData d in myDistances) {
                if (d.nextNode == tempShortest) {
                    myDistances.Remove(d);
                    break;
                }
            }
            myDistances.Add(new DistanceData(t, tempShortest, shortest));
        }
    }

}

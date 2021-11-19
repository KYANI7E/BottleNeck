using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public Queue<GameObject> goldQueue = new Queue<GameObject>();
    public List<ResourceGiver> goldMines = new List<ResourceGiver>();

    public Queue<GameObject> stoneQueue = new Queue<GameObject>();
    public List<ResourceGiver> quaries = new List<ResourceGiver>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (goldQueue.Count > 0 && goldMines.Count > 0) {
            goldQueue = QueueGetter(goldQueue, goldMines);
        }
        if (stoneQueue.Count > 0 && quaries.Count > 0) {
            stoneQueue = QueueGetter(stoneQueue, quaries);
        }
    }

    //private PQ<GameObject> QueueGetter(PQ<GameObject> q, List<ResourceGiver> rg)
    //{
    //    float shortest = 9999;
    //    int pri = q.highestPriority;
    //    GameObject tempObject = q.Dequeue();
    //    ResourceGiver bestGiver = null;

    //    bool videCheck = true;
    //    foreach (ResourceGiver r in rg) {
    //        if (r.sendingToo.Count > 2) continue;
    //        foreach (Transporter.DistanceData d in r.myDistances) {
    //            if (d.receiver == tempObject) {
    //                if (d.distance < shortest) {
    //                    bestGiver = r;
    //                    shortest = d.distance;
    //                    videCheck = false;
    //                    break;
    //                }
    //            }
    //        }
    //    }


    //    if (videCheck) {
    //        q.Enqueue(tempObject, pri);
    //    }

    //    if (bestGiver != null)
    //        bestGiver.sendingToo.Enqueue(tempObject);
    //    return q;
    //}

    private Queue<GameObject> QueueGetter(Queue<GameObject> q, List<ResourceGiver> rg)
    {
        float shortest = 9999;
        GameObject tempObject = q.Dequeue();
        ResourceGiver bestGiver = null;

        bool videCheck = true;
        foreach(ResourceGiver r in rg) {
            if (r.sendingToo.Count > 2) continue;
            foreach (Transporter.DistanceData d in r.myDistances) {
                if (d.receiver == tempObject) {
                    if (d.distance < shortest) {
                        bestGiver = r;
                        shortest = d.distance;
                        videCheck = false;
                        Debug.Log("Send it wefweefw");
                        break;
                    }
                }
            }
        }


        if (videCheck) {
            q.Enqueue(tempObject);
        }

        if (bestGiver != null)
            bestGiver.sendingToo.Enqueue(tempObject);
        return q;
    }
}

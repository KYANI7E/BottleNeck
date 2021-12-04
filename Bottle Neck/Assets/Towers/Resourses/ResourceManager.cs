using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public Queue<GameObject> goldQueue = new Queue<GameObject>();
    public List<ResourceGiver> goldMines = new List<ResourceGiver>();

    public PQ<GameObject> stoneQueue = new PQ<GameObject>();
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
        if (stoneQueue.Count() > 0 && quaries.Count > 0) {
            QueueGetter(stoneQueue, quaries);
        }else if(stoneQueue.Count() == 0) {
            stoneQueue.EnqueueItems(temp);
            temp.Clear();
        }
    }

    List<PQ<GameObject>.Item> temp = new List<PQ<GameObject>.Item>();
    private void QueueGetter(PQ<GameObject> q, List<ResourceGiver> rg)
    {
        float shortest = 9999;
        PQ<GameObject>.Item tempObject = q.Dequeue();
        int pHolder = tempObject.pri;
        ResourceGiver bestGiver = null;


        bool videCheck = false;
        foreach (ResourceGiver r in rg) {
            if (r.sendingToo != null) continue;
            foreach (TNode.Connector d in r.connectors) {
                if (d.desination == tempObject.thing) {
                    if (d.distance < shortest) {
                        bestGiver = r;
                        shortest = d.distance;
                        videCheck = true;
                        break;
                    }
                }
            }
        }
        if (!videCheck) {

            temp.Add(tempObject);
        } else {
            bestGiver.sendingToo = tempObject.thing;
            stoneQueue.EnqueueItems(temp);
            temp.Clear();
        }

        return;
    }

    private Queue<GameObject> QueueGetter(Queue<GameObject> q, List<ResourceGiver> rg)
    {
        float shortest = 9999;
        GameObject tempObject = q.Dequeue();
        ResourceGiver bestGiver = null;

        bool videCheck = true;
        foreach(ResourceGiver r in rg) {
            if (r.sendingToo != null) continue;
            foreach (TNode.Connector d in r.connectors) {
                if (d.desination == tempObject) {
                    if (d.distance < shortest) {
                        bestGiver = r;
                        shortest = d.distance;
                        videCheck = false;
                        break;
                    }
                }
            }
        }


        if (videCheck) {
            q.Enqueue(tempObject);
        }

        if (bestGiver != null)
            bestGiver.sendingToo = tempObject;
        return q;
    }
}

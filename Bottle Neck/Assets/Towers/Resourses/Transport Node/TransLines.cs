using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransLines : MonoBehaviour
{

    private LineRenderer lr;
    public GameObject tran;

    private float range;
    public List<GameObject> allPlaces = new List<GameObject>();
    public List<GameObject> close = new List<GameObject>();

    public bool isGiver;
    public bool isReciever;
    public string type;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        range = tran.GetComponent<TNode>().range;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject g in allPlaces) {
            if (Vector2.Distance(g.transform.position, transform.position) < range && RaycastCheck(g)) {
                if (!close.Contains(g) && CheckIfWant(g)) {
                        close.Add(g);
                    
                }
            } else if (close.Contains(g) || !allPlaces.Contains(g))
                close.Remove(g);
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
                if(hits[i].collider.gameObject.name.Equals("Mountain Shit")) {
                    good = false;
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

    public bool CheckIfWant(GameObject g)
    {
        if (!isGiver && !isReciever) return true;

        if (g.GetComponent<ResourceGiver>() != null) {
            if (isGiver) return false;
            ResourceGiver temp = g.GetComponent<ResourceGiver>();
            if (temp.type.Equals(type))
                return true;
            else return false;
        } else if (g.GetComponent<Receiver>() != null) {
            if (isReciever) return false;
            Receiver temp = g.GetComponent<Receiver>();
            if (temp.type.Equals(type))
                return true;
            else return false;
        } else 
            return true;
    }

    public void DrawLine()
    {
        Transform[] points = new Transform[close.Count * 2];

        lr.positionCount = points.Length;

        List<GameObject> tempRemove = new List<GameObject>();

        int j = 0;
        foreach (GameObject t in close) {
            if (t == null) {
                tempRemove.Add(t);
                j += 2;
                continue;
            }
            points[j] = t.transform;
            points[j + 1] = this.transform;
            j += 2;
        }

        foreach (GameObject g in tempRemove) close.Remove(g);

        for (int i = 0; i < points.Length; i += 2) {
            if (points[i] == null) continue;
            lr.SetPosition(i, points[i].position);
            lr.SetPosition(i + 1, transform.position);
        }
    }
}

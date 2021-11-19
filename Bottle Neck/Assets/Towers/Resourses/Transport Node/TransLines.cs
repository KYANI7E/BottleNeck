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


    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        range = tran.GetComponent<Transporter>().range;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject g in allPlaces) {
            if (Vector2.Distance(g.transform.position, transform.position) < range) {
                if (!close.Contains(g)) {
                    close.Add(g);
                }
            } else if (close.Contains(g))
                close.Remove(g);
        }

        DrawLine();
    }

    public void DrawLine()
    {
        Transform[] points = new Transform[close.Count * 2];

        lr.positionCount = points.Length;

        int j = 0;
        foreach (GameObject t in close) {
            points[j] = t.transform;
            points[j + 1] = this.transform;
            j += 2;
        }

        for (int i = 0; i < points.Length; i += 2) {
            lr.SetPosition(i, points[i].position);
            lr.SetPosition(i + 1, transform.position);
        }
    }
}

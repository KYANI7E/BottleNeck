using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{

    public Vector2 position;
    public ResourceManager rm;
    public bool isBase = false;
    public string type;

    public int maxHolding;
    public int currentHolding;

    // Start is called before the first frame update
    void Start()
    {
        rm = GameObject.FindGameObjectWithTag("RM").GetComponent<ResourceManager>();
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBase) {
            if (rm.goldQueue.Count < 20)
                rm.goldQueue.Enqueue(this.gameObject);
        }
        if (type.Equals("Stone")) {
            if(currentHolding < maxHolding) {
                rm.stoneQueue.Enqueue(this.gameObject);
                currentHolding++;
            }
        }
    }
}

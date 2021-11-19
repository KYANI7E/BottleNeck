using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Receiver : MonoBehaviour
{

    public Vector2 position;
    public ResourceManager rm;
    public bool isBase = false;
    public string type;

    public int priority;

    public int maxHolding;
    public int currentHolding;
    private int ghostHolding;

    public Image cir;

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
            if(ghostHolding < maxHolding) {
                priority = maxHolding - ghostHolding;
                rm.stoneQueue.Enqueue(this.gameObject);
                ghostHolding++;
            }
        }
    }

    public void Less(int amount)
    {
        currentHolding -= amount;
        ghostHolding -= amount;
        cir.fillAmount = (float) currentHolding / (float)maxHolding;
    }

    public void More(int amount)
    {
        currentHolding += amount;
        cir.fillAmount = (float)currentHolding / (float)maxHolding;
    }
}

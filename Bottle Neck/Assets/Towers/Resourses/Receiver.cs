using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    public List<TNode> connectedNodes = new List<TNode>();

    public Vector2 position;
    public ResourceManager rm;
    public bool isBase = false;
    public string type;

    public int priority;

    public int maxHolding;
    public int currentHolding;
    private int ghostHolding;

    public Image cir;

    public Shop shop;

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
            if(ghostHolding < maxHolding /* && rm.stoneQueue.Count() < 5*/) {
                priority = maxHolding - ghostHolding;
                rm.stoneQueue.Enqueue(this.gameObject, ghostHolding);
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

    public void Die()
    {
        List<GameObject> temp = new List<GameObject>();
        temp.Add(this.gameObject);
        foreach(TNode n in connectedNodes) {
            n.recieversInRange.Remove(this.gameObject);
            n.PathBroke(this.gameObject, temp);
        }
        shop.recievers.Remove(this.gameObject);
        shop.allLined.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}

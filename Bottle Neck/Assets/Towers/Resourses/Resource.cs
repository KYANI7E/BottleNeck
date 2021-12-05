using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public GameObject destination;
    private Vector2 desPos;
    public GameObject currentNode;

    public float speed;
    public int amountWorth;
    public float howCloseToCenter;
    public bool arrived = false;

    public Shop shop;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (currentNode == null)
           NoNode();
        else
        if (!arrived) {
            if (currentNode.GetComponent<TNode>() != null)
                desPos = new Vector2(currentNode.GetComponent<TNode>().position.x, currentNode.GetComponent<TNode>().position.y);
            else
                desPos = new Vector2(currentNode.GetComponent<Receiver>().position.x, currentNode.GetComponent<Receiver>().position.y);

            CheckAndChange();
            Move();
        }
    }



    private void CheckAndChange()
    {
        if (Vector2.Distance(transform.position, desPos) <= howCloseToCenter) {
            if (currentNode == destination) {
                Arrived();
            } else {
                foreach (TNode.Connector d in currentNode.GetComponent<TNode>().connectors) {
                    if (d.desination == destination) {
                        speed = currentNode.GetComponent<TNode>().speedOfThing;
                        currentNode = d.nextNode;
                        if (currentNode == null) {
                            NoNode();
                            return;
                        }
                        if (currentNode.GetComponent<TNode>() != null)
                            desPos = new Vector2(currentNode.GetComponent<TNode>().position.x, currentNode.GetComponent<TNode>().position.y);
                        else
                            desPos = new Vector2(currentNode.GetComponent<Receiver>().position.x, currentNode.GetComponent<Receiver>().position.y);
                        break;
                    }
                }
            }
        }
    }

    private void NoNode()
    {
        Destroy(this.gameObject);
    }

    private void Arrived()
    {
        if (currentNode.GetComponent<Base>() != null) {
            currentNode.GetComponent<Base>().shop.AddMoney(amountWorth);
            Destroy(this.gameObject);
        } else {
            currentNode.GetComponent<Receiver>().More(amountWorth);
            Destroy(this.gameObject);
        }
    }

    private void Move()
    {
        if(!shop.paused)
        transform.position = Vector2.MoveTowards(transform.position, desPos, Time.deltaTime * speed);
    }
}

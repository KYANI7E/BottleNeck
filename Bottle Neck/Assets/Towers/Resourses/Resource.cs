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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!arrived)
        {
            if (currentNode.GetComponent<Transporter>() != null)
                desPos = new Vector2(currentNode.GetComponent<Transporter>().position.x, currentNode.GetComponent<Transporter>().position.y);
            else
                desPos = new Vector2(currentNode.GetComponent<Receiver>().position.x, currentNode.GetComponent<Receiver>().position.y);

            CheckAndChange();
            Move();
        }
    }

    private void CheckAndChange()
    {
        if (Vector2.Distance(transform.position, desPos) <= howCloseToCenter)
        {
            if (currentNode == destination)
            {
                Arrived();
            }
            else
            {
                foreach (Transporter.DistanceData d in currentNode.GetComponent<Transporter>().myDistances) {
                    if (d.receiver == destination) {
                        currentNode = d.nextNode;
                        if (currentNode.GetComponent<Transporter>() != null)
                            desPos = new Vector2(currentNode.GetComponent<Transporter>().position.x, currentNode.GetComponent<Transporter>().position.y);
                        else
                            desPos = new Vector2(currentNode.GetComponent<Receiver>().position.x, currentNode.GetComponent<Receiver>().position.y);
                        break;
                    }
                }
            }
        }
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
        transform.position = Vector2.MoveTowards(transform.position, desPos, Time.deltaTime * speed);
    }
}

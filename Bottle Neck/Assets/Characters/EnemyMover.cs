using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public Node currentNode;
    private Node lastNode;

    bool atBase = false;

    [SerializeField] float walkSpeed = 0;
    [SerializeField] float howCloseToCenter = .1f;

    [SerializeField] PathFinder pathFinder;

    public bool go = false;

    public int distanceToGo = 3000;

    [SerializeField] Animator animator;

    public bool dead = false;

    public int damage;
    public float attackSpeed;
    private float coolDown = 0;
    private float rand = 0;
    private bool vertDircetion = false;
    public float distanceToTower;

    private Shop shop;

    private void Awake()
    {
        distanceToGo = 3000;
    }
    // Start is called before the first frame update
    void Start()
    {
        shop = GameObject.Find("Shop").GetComponent<Shop>();
        pathFinder = GameObject.Find("PathFinder").GetComponent<PathFinder>();
        rand = Random.Range(-.4f, .4f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!shop.paused) {
            if (!atBase && go && !dead)
                GetPathAndMove();
            if (atBase)
                Attack();
        }
    }

    private void GetPathAndMove()
    {
        if (currentNode != null)
        {
            if (currentNode.pathNode != null)
            {
                if (Mathf.Abs(transform.position.x - currentNode.position.x - rand) <= howCloseToCenter
                        && Mathf.Abs(transform.position.y - currentNode.position.y - rand) <= howCloseToCenter)
                {
                    lastNode = currentNode;
                    currentNode = currentNode.pathNode;
                    if (currentNode.target == true) atBase = true;
                    else
                    {
                        distanceToGo = currentNode.CalculatePathDistacneToTarget();
                        UpdateAnimation();
                    }
                }
                if (currentNode.occupance != null)
                {
                    Attack();
                }
                else
                {
                    animator.SetBool("TowerDead", true);
                    Move();
                }
            }
            else
            {
                lastNode = currentNode;
                currentNode = pathFinder.FindPath(transform.position);
            }
        }
        else
        {
            lastNode = currentNode;
            currentNode = pathFinder.FindPath(transform.position);
            if (currentNode == null) Debug.LogError("Waiting " + this);
        }
    }

    int dirX = 0;
    int dirY = 0;

    void UpdateAnimation()
    {
        if (currentNode.pathNode == null)
            return;


        int i = 0;
        foreach(Node n in currentNode.friends)
        {
            if(n == lastNode)
            {
                if (i == 0)
                {
                    dirX = 0;
                    dirY = -1;
                    vertDircetion = true;
                } else if (i == 1)
                {
                    dirX = -1;
                    dirY = 0;
                    vertDircetion = false;
                }
                else if (i == 2)
                {
                    dirX = 0;
                    dirY = 1;
                    vertDircetion = true;
                }
                else if (i == 3)
                {
                    dirX = 1;
                    dirY = 0;
                    vertDircetion = false;
                }
                break;
            }
            i++;
        }

        animator.SetInteger("Horizontal", dirX);
        animator.SetInteger("Vertical", dirY);

    }

    private void Attack()
    {
        animator.SetBool("TowerDead", false);

        GameObject tower = currentNode.occupance;
        coolDown -= Time.deltaTime;

        if(coolDown <= 0)
        {
            animator.SetTrigger("Attack");
            if (tower.GetComponent<Building>() != null)
                tower.GetComponent<Building>().Hit(damage);
            else
                tower.GetComponent<Base>().Hit(damage);
            coolDown = attackSpeed;
        }

        if(tower == null)
            animator.SetBool("TowerDead", true);

        AttackMove();

    }

    private void AttackMove()
    {

        Vector2 pos;
        if (vertDircetion)
            if(transform.position.y > currentNode.position.y)
                pos = new Vector2(currentNode.position.x + rand, currentNode.position.y + distanceToTower +.5f);
            else
                pos = new Vector2(currentNode.position.x + rand, currentNode.position.y - distanceToTower -.5f);
        else
            if (transform.position.x > currentNode.position.x)
                pos = new Vector2(currentNode.position.x + distanceToTower +.5f, currentNode.position.y + rand);
            else
                pos = new Vector2(currentNode.position.x - distanceToTower -.5f, currentNode.position.y + rand);


        transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * walkSpeed);

    }

    private void Move()
    {
        Vector2 pos = new Vector2(currentNode.position.x + rand, currentNode.position.y + rand);
        transform.position =  Vector2.MoveTowards(transform.position, pos, Time.deltaTime * walkSpeed);
    }
}

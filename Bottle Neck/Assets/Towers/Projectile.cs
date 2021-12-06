using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public GameObject target;
    public int damage;
    public float speed;

    public float distanceToKill;

    private Shop shop;

    private void Start()
    {
        shop = GameObject.Find("Shop").GetComponent<Shop>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!shop.paused) {
            float disX = Mathf.Abs(transform.position.x - target.transform.position.x);
            float disY = Mathf.Abs(transform.position.y - target.transform.position.y);
            if (disX < distanceToKill && disY < distanceToKill) {
                DamageTarget();
            }
            Move();
            LookAt();
        }

    }

    private void LookAt()
    {
        Vector2 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Move()
    {
        Vector3 vec = new Vector3(transform.position.x, transform.position.y, -5);
        Vector3 vecGo = new Vector3(target.transform.position.x, target.transform.position.y, -5);

        transform.position = Vector3.MoveTowards(vec, vecGo, Time.deltaTime * speed);
    }
    void DamageTarget()
    {
        target.GetComponent<EnemyLife>().Hit(damage);
        Destroy(this.gameObject);
    }
}


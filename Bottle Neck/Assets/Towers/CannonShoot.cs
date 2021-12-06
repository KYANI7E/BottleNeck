using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : MonoBehaviour
{

    private TowerTargeting targeting;
    private GameObject enemy;

    [SerializeField] float rateOfFire = 0;
    [SerializeField] int damage = 0;
    public int amountToFire;
    [SerializeField] GameObject cannonBall;
    private float coolingDown = 0;

    public GameObject gun;

    public Shop shop;

    // Start is called before the first frame update
    void Start()
    {
        targeting = GetComponent<TowerTargeting>();
        shop = GetComponent<Receiver>().shop;
    }

    // Update is called once per frame
    void Update()
    {
        
        enemy = targeting.currentEnemy;
        Shooting();
    }

    void Shooting()
    {
        if (enemy != null)
        {
            LookAt();
            if (enemy.GetComponent<EnemyLife>().ghoastHealth > 0)
            {
                if (coolingDown <= 0 && GetComponent<Receiver>().currentHolding >= amountToFire)
                {
                    Fire();
                    coolingDown = rateOfFire;
                }
            }
            else
            {
                GetComponent<TowerTargeting>().CurrentGhoatDead();

            }
        }

        if (coolingDown > 0 && !shop.paused)
            coolingDown -= Time.deltaTime;
        
    }



    private void LookAt()
    {
        Vector2 dir = enemy.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        gun.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Fire()
    {
        Vector3 vec = new Vector3(transform.position.x, transform.position.y, -5);
        GameObject ball = Instantiate(cannonBall, vec, Quaternion.identity);
        ball.GetComponent<Projectile>().target = enemy;
        ball.GetComponent<Projectile>().damage = damage;
        enemy.GetComponent<EnemyLife>().ghoastHealth -= damage;
        if(enemy.GetComponent<EnemyLife>().ghoastHealth < 1)
            GetComponent<TowerTargeting>().CurrentGhoatDead();
        GetComponent<Receiver>().Less(1);
    }
}

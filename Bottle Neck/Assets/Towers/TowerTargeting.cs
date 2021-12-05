using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTargeting : MonoBehaviour
{

    public GameObject currentEnemy;
    [SerializeField] private List<GameObject> enemies  = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SelectEnemyClosestToBase();
        if(currentEnemy != null)
            if (!RaycastCheck(currentEnemy)) currentEnemy = null;
    }

    void SelectEnemyClosestToBase()
    {
        if(enemies.Count > 0)
            foreach (GameObject g in enemies)
            {
                if (g == null) continue;
                if (!RaycastCheck(g)) continue;
                if (currentEnemy == null) currentEnemy = g;
                if (currentEnemy == null) continue;
                if (g.GetComponent<EnemyLife>().ghoastHealth < 1) continue;
                else if (currentEnemy.GetComponent<EnemyMover>().distanceToGo > g.GetComponent<EnemyMover>().distanceToGo) {
                    currentEnemy = g;
                }
                
            }
    }

    private bool RaycastCheck(GameObject thing)
    {
        Vector2 raycastDir = thing.transform.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position,raycastDir, 40, LayerMask.GetMask("Default"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.Equals(thing))
                return true;
        }
        return false;
    }

    public void CurrentGhoatDead()
    {
        enemies.Remove(currentEnemy);
        currentEnemy = null;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if(other.GetComponent<EnemyLife>().ghoastHealth > 0 && !enemies.Contains(other.gameObject))
                enemies.Add(other.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Remove(other.gameObject);
            if (other.gameObject == currentEnemy)
                currentEnemy = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLife : MonoBehaviour
{

    [SerializeField] public int maxHealth;
    public int currentHealth;
    public int ghoastHealth;

    public Animator animator;

    public Slider healthSlider;
    public Canvas unitCanvas;

    public int moneyDrop;
    public Shop shop;

    public GameObject corps;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        ghoastHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        shop = GameObject.Find("Shop").GetComponent<Shop>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");
            unitCanvas.enabled = false;
        }
        else if (currentHealth < maxHealth)
            unitCanvas.enabled = true;
    }

    public void Hit(int damage)
    {
        animator.SetTrigger("Hit");
        currentHealth -= damage;
        healthSlider.value = currentHealth;
    }

    public void Die()
    {
        Instantiate(corps, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

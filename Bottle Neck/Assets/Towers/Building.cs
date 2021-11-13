using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{

    public int currentHealth;
    [SerializeField] public int maxHealth;
    public int blockPerHpValue;


    public Slider healthSlider;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
            Die();

        if (currentHealth != maxHealth)
            healthSlider.gameObject.SetActive(true);
    }

    private void Die()
    {
        if(GetComponent<Transporter>() != null) {
            GetComponent<Transporter>().Die();
        }else
        Destroy(this.gameObject);
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
    }
}

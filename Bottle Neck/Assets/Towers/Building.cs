using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{

    public int currentHealth;
    [SerializeField] public int maxHealth;

    public Slider healthSlider;
    public Canvas unitCanvas;


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
            Destroy(this.gameObject);

        if (currentHealth != maxHealth)
            unitCanvas.enabled = true;
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;
    }
}

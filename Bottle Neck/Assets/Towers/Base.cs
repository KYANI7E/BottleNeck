using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Base : MonoBehaviour
{

    public int maxHp;
    public int currentHp;

    public GameObject[] ui;
    public Canvas gg;

    public Slider healthSlider;
    public Canvas unitCanvas;
    public Shop shop;

    public float rateOfPassiveMoney;
    private float coolDown;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;


        healthSlider.maxValue = maxHp;
        healthSlider.value = currentHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHp != maxHp)
            unitCanvas.enabled = true;

        if(coolDown < 0) {
            shop.AddMoney(1);
            coolDown = rateOfPassiveMoney;
        }
        coolDown -= Time.deltaTime;
    }

    public void Hit(int damage)
    {
        currentHp -= damage;
        healthSlider.value = currentHp;
        if (currentHp <= 0)
            GameOver();
    }

    private void GameOver()
    {
        foreach(GameObject u in ui)
        {
            u.SetActive(false);
        }
        gg.enabled = true;
    }
}

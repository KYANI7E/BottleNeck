using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaveButton : MonoBehaviour
{

    public Spawning spawn;

    public Image cir;
    public Text count;
    private int totalWaves;

    public float timeBetweenWaves;
    public float coolDown;

    public bool waity = true;

    // Start is called before the first frame update
    void Start()
    {
        coolDown = 0;
        cir.fillAmount = 1;
        waity = true;
        totalWaves = spawn.levelData.waves.Length;
        count.text = "1/" + totalWaves;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawn.nextWave && !waity && spawn.enemiesAlive.Count == 0 && !spawn.lastWave)
            CountDown();
    }

    private void CountDown()
    {
        coolDown += Time.deltaTime;
        cir.fillAmount =  coolDown / timeBetweenWaves;
        if(coolDown > timeBetweenWaves)
        {
            coolDown = 0;
            NextWave();
        }
    }

    public void NextWave()
    {
        if (spawn.lastWave)
            return;

        count.text = spawn.currentWave + 1 + "/" + totalWaves;
        cir.fillAmount = 0;
        coolDown = 0;
        waity = false;
        spawn.NextWave();
    }
}

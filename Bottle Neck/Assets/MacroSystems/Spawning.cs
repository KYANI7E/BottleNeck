using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{

    public LevelWaves levelData;
    public int currentWave = 0;
    private int setIndex = 0;
    private int amountIndex = 0;
    private float coolDown;
    [SerializeField] private float timeBetweenWaves;

    public List<GameObject> enemiesAlive;

    [SerializeField] public bool nextWave = false;

    private Node[] myNode;
    public PathFinder pathFinder;

    public GameObject[] locations;
    private GameObject currentLocation;

    public GameObject winUI;
    public bool lastWave = false;

    private Shop shop;

    // Start is called before the first frame update
    void Start()
    {
        shop = GameObject.Find("Shop").GetComponent<Shop>();

        myNode = new Node[locations.Length];
        Vector2 pos = pathFinder.RoundCords(transform.position);

        for(int i = 0; i < locations.Length; i++)
        {
            myNode[i] = pathFinder.nodes[(int)locations[i].transform.position.y, (int)locations[i].transform.position.x];
        }


        SetParitcles();
    }

    // Update is called once per frame
    void Update()
    {

        if (nextWave == true) 
            SpawnWave();

        if (lastWave && enemiesAlive.Count == 0)
            winUI.SetActive(true);
    }


    public void NextWave()
    {
        nextWave = true;
    }

    bool hellSpawn = false; //if true spawn at all portals
    bool flag = true;

    float coolDownB;
    void SpawnWave()
    {
        
        WaveData data = levelData.waves[currentWave];
        if (data.location[setIndex] != -1)
        {
            currentLocation = locations[data.location[setIndex]];
            hellSpawn = false;
        }
        else
            hellSpawn = true;

        if (flag)
        {
            
            for (int i = 0; i < locations.Length; i++)
            {
                for (int j = 0; j < data.location.Length; j++)
                    if (i == levelData.waves[currentWave].location[j] || levelData.waves[currentWave].location[j] == -1)
                    {
                        locations[i].GetComponent<Animator>().SetBool("Play", true);
                        locations[i].transform.Find("Particles").gameObject.SetActive(false);
                        locations[i].GetComponent<Indicatior>().indicator.SetActive(false);
                        locations[i].GetComponent<Indicatior>().enabled = false;
                    }
            }
            flag = false;
        }


        if (coolDownB <= 0 || setIndex == 0)
            SpawnTiming(data);
        else
            coolDownB -= Time.deltaTime;


        if(setIndex >= data.enemies.Length)
        {
            flag = true;
            for (int i = 0; i < locations.Length; i++)
            {
                for (int j = 0; j < data.location.Length; j++)
                    if (i == levelData.waves[currentWave].location[j]) {
                        locations[i].GetComponent<Animator>().SetBool("Play", false);
                        locations[i].GetComponent<Indicatior>().indicator.SetActive(false);
                        locations[i].GetComponent<Indicatior>().enabled = false;
                    }
            }
            currentWave++;
            if (currentWave < levelData.waves.Length)
                SetParitcles();
            else
                lastWave = true;
            setIndex = 0;
            nextWave = false;
        }

    }

    void SpawnTiming(WaveData data)
    {
        if(!shop.paused) coolDown -= Time.deltaTime;
        if (coolDown < 0)
        {
            if (hellSpawn)
                HellSpawn(data.enemies[setIndex]);
            else
                Spawn(data.enemies[setIndex]);
            coolDown = data.intervalBetweenEnemies[setIndex];
            amountIndex++;
            if (data.enemiesAmount[setIndex] <= amountIndex)
            {
                coolDownB = data.timeBetweenSets[setIndex];
                setIndex++;
                amountIndex = 0;
            }
        }
    }

    private void SetParitcles() 
    {
        foreach(GameObject g in locations) {
            g.transform.Find("Particles").gameObject.SetActive(false);
            g.GetComponent<Indicatior>().indicator.SetActive(false);
            g.GetComponent<Indicatior>().enabled = false;
        }

        for (int i = 0; i < locations.Length; i++)
        {
            for (int j = 0; j < levelData.waves[currentWave].location.Length; j++)
                if (i == levelData.waves[currentWave].location[j] || levelData.waves[currentWave].location[j] == -1)
                {
                    locations[i].transform.Find("Particles").gameObject.SetActive(true);
                    locations[i].GetComponent<Indicatior>().enabled = true;
                }
        }
    }

    private void HellSpawn(GameObject obj)
    {
        for (int i = 0; i < locations.Length; i++)
        {
            GameObject en = Instantiate(obj, locations[i].transform.position, Quaternion.identity);
            en.GetComponent<EnemyMover>().currentNode = myNode[i];
            en.GetComponent<EnemyMover>().go = true;
            enemiesAlive.Add(en);
        }
    }

    void Spawn(GameObject obj)
    {
        GameObject en = Instantiate(obj, currentLocation.transform.position, Quaternion.identity);
        en.GetComponent<EnemyMover>().currentNode = myNode[levelData.waves[currentWave].location[setIndex]];
        en.GetComponent<EnemyMover>().go = true;
        enemiesAlive.Add(en);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            enemiesAlive.Remove(other.gameObject);
        }
    }
}

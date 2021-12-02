using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    public GameObject[] structures;
    public int[] prices;
    public GameObject[] goastUnits;
    public GameObject[] wills;
    public GameObject[] nopes;
    public int selectedUnit;
    public GameObject goastUnit;

    private bool collector;
    public List<GameObject> recievers = new List<GameObject>();
    public  List<TNode> transports = new List<TNode>();
    public List<ResourceGiver> resourceGivers = new List<ResourceGiver>();
    public List<GameObject> allLined = new List<GameObject>();

    public Text moneyText; 
    public int money;

    public Vector3 mouse_Pos;
    public Vector3 destination;

    private Camera cam;

    public PathFinder pathFinder;
    public ResourceManager rm;

    private Animator animator;
    private bool shopOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject basee = GameObject.FindGameObjectWithTag("Base");
        allLined.Add(basee);

        cam = Camera.main;
        animator = GetComponent<Animator>();
        shopOpen = false;
        moneyText.text = money.ToString();
        recievers.Add(GameObject.FindGameObjectWithTag("Base"));
        SetGoasts();
    }

    private void SetGoasts()
    {
        for (int i = 0; i < prices.Length; i++)
        {
            if (prices[i] <= money)
            {
                goastUnits[i] = wills[i];
            }
            else
            {
                goastUnits[i] = nopes[i];
            }
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        moneyText.text = money.ToString();
        SetGoasts();
        if(selectedUnit != -1 )
            if(prices[selectedUnit] <= money)
            { 
                Destroy(goastUnit);
                GameObject currentGoast = goastUnits[selectedUnit];
                goastUnit = Instantiate(currentGoast, new Vector3(destination.x, destination.y, 0), new Quaternion(0, 0, 0, 0));
                if (goastUnit.GetComponent<TransLines>() != null)
                    AssignPlacesForTranLine();
            }
    }

    private void SpendMoney(int amount)
    {
        money -= amount;
        moneyText.text = money.ToString();
        SetGoasts();

    }


    // Update is called once per frame
    void Update()
    {
        ShowGoast();
        if (Input.GetMouseButton(1) && selectedUnit != -1)
            UnSelect();

        if (Input.GetMouseButton(1) && shopOpen)
            CloseShop();

    }

    void UnSelect()
    {
        selectedUnit = -1;
        Destroy(goastUnit);
        OpenShop();
    }

    public void ShopButton()
    {
        if (!shopOpen)
        {
            animator.SetBool("Open", true);
            shopOpen = true;
        }
        else
        {
            animator.SetBool("Open", false);
            shopOpen = false;
        }
    }

    public void CloseShop()
    {
        animator.SetBool("Open", false);
        shopOpen = false;
    }

    public void OpenShop()
    {
        animator.SetBool("Open", true);
        shopOpen = true;
    }

    void ShowGoast()
    {
        if (goastUnit != null)
        {
            mouse_Pos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

            destination = new Vector3(Mathf.Round(mouse_Pos.x), Mathf.Round(mouse_Pos.y), 0); //rounds the cords to the nearst one so it click
            goastUnit.transform.position = new Vector3(Mathf.Round(mouse_Pos.x), Mathf.Round(mouse_Pos.y), 0); //moves the goast unit to the mouse position

            if (Input.GetMouseButton(0) && prices[selectedUnit] <= money &&
                !EventSystem.current.IsPointerOverGameObject())
            {
                Spawn(selectedUnit);
            }
        }
    }

    void Spawn(int i)
    {
        if (!CheckValidPlace(destination))
            return;

        GameObject newObj = Instantiate(structures[i], destination, Quaternion.identity);

        if (collector) {
            //resourceGivers.Add(newObj.GetComponent<ResourceGiver>());
            if(newObj.GetComponent<ResourceGiver>().type.Equals("Gold"))
                rm.goldMines.Add(newObj.GetComponent<ResourceGiver>());
            else if (newObj.GetComponent<ResourceGiver>().type.Equals("Stone"))
                rm.quaries.Add(newObj.GetComponent<ResourceGiver>());
        }

        if (newObj.GetComponent<Receiver>() != null) {
            recievers.Add(newObj);
            foreach (TNode t in transports) {
                t.NewResiever(newObj);
            }
            allLined.Add(newObj);
            newObj.GetComponent<Receiver>().shop = this;
        }

        if (newObj.GetComponent<TNode>() != null) {
            allLined.Add(newObj);

            newObj.GetComponent<TNode>().theGameObject = newObj;
            newObj.GetComponent<TNode>().shop = this;
            newObj.GetComponent<TNode>().allRecievers = recievers;

            foreach(TNode n in transports) {
                newObj.GetComponent<TNode>().allNodes.Add(n.theGameObject);
            }

            newObj.GetComponent<TNode>().CheckForRecievers();
            transports.Add(newObj.GetComponent<TNode>());
        }

        pathFinder.ReUpdatePath(newObj, destination);
        SpendMoney(prices[selectedUnit]);

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            selectedUnit = -1;
            Destroy(goastUnit); 
        }
        else
        {
            Destroy(goastUnit);
            GameObject currentGoast = goastUnits[selectedUnit];
            goastUnit = Instantiate(currentGoast, new Vector3(destination.x, destination.y, 0), new Quaternion(0, 0, 0, 0));
            if (goastUnit.GetComponent<TransLines>() != null)
                AssignPlacesForTranLine();
        }

    }

    bool CheckValidPlace(Vector3 spawnPos)
    {
        Vector2 vec = pathFinder.RoundCords(spawnPos);

        if ((int)vec.y < 0 || (int)vec.y > pathFinder.nodes.GetLength(0) || (int)vec.x < 0 || (int)vec.x > pathFinder.nodes.GetLength(1))
            return false;

        Node spot = pathFinder.nodes[(int)vec.y, (int)vec.x];

        if (collector && !spot.collectable)
            return false;

        if (collector)
            if (!spot.type.Equals(structures[selectedUnit].GetComponent<ResourceGiver>().type))
                return false; 

        if (spot.traverable && spot.occupance == null)
            return true;
        else
            return false;
    }

    // this is selected by the button to select a unit
    public void ChoiseUnit(int num)
    {

        if (goastUnit != null)
        {
            Destroy(goastUnit);
        }

        if (structures[num-1].GetComponent<ResourceGiver>() != null)
            collector = true;
        else
            collector = false;

        selectedUnit = num - 1;
        GameObject currentGoast = goastUnits[num - 1];

        goastUnit = Instantiate(currentGoast, new Vector3(destination.x, destination.y, 0), new Quaternion(0, 0, 0, 0));

        if (goastUnit.GetComponent<TransLines>() != null) 
            AssignPlacesForTranLine();

        CloseShop();
    }

    private void AssignPlacesForTranLine()
    {
        
        goastUnit.GetComponent<TransLines>().allPlaces = allLined;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScropt : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] shit;
    int cur = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && cur < shit.Length -1 ) {
            shit[cur].SetActive(false);
            cur++;
            shit[cur].SetActive(true);
        } else if (Input.GetKeyDown(KeyCode.Space) && cur == shit.Length-1) {
            Destroy(this.gameObject);
        }
    }
}

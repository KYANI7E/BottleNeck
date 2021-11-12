using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Delete", 1);      
    }

    void Delete()
    {
        Destroy(this.gameObject);
    }
}

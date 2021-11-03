using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayRange : MonoBehaviour
{
    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}

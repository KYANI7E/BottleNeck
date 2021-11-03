using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public Animator anim;
    public void Vieew()
    {
        anim.SetBool("View", true);
    }
}

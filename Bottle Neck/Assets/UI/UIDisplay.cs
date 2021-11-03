using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplay : MonoBehaviour
{
    public GameObject stats;

    public void Show()
    {
        stats.SetActive(true);
    }

    public void Hide()
    {
        stats.SetActive(false);
    }
}

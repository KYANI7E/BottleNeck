using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicatior : MonoBehaviour
{
    private GameObject target;
    private Camera cam;
    public GameObject indicator;
    public int scaler;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        target = cam.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<SpriteRenderer>().isVisible) {
            if (indicator.activeSelf == false)
                indicator.SetActive(true);
            RayCast();
            Scale();
        } else {
            if (indicator.activeSelf == true)
                indicator.SetActive(false);
        }   
    }

    private void Scale()
    {

        indicator.transform.localScale = new Vector3(cam.orthographicSize / scaler, cam.orthographicSize / scaler, 1);

    }

    private void RayCast()
    {
        Vector2 dir = target.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, LayerMask.GetMask("Indicator"));
        if (hit.collider != null) { 
            indicator.transform.position = hit.point;
        }


    }
}

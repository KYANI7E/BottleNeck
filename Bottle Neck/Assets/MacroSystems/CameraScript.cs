using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 0;
    [SerializeField] float scrollSpeed = 0;
    Camera cam;

    [SerializeField] float maxZoom;
    [SerializeField] float minZoom;

    float mapMinX, mapMaxX, mapMinY, mapMaxY;
    [SerializeField] Vector2 mapSize;
    [SerializeField] int extraSpace;

    private Vector3 origin;
    private Vector3 difference;
    private bool drag = false;

    private bool dragging = false;

    public BoxCollider2D box;

    // Start is called before the first frame update
    void Start()
    {
        mapMaxY = mapSize.y - .5f + extraSpace;
        mapMinY = -.5f - extraSpace;
        mapMaxX = mapSize.x - .5f + extraSpace;
        mapMinX = -.5f -extraSpace;

        cam = Camera.main;
        box = GetComponent<BoxCollider2D>();
    }



    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        if(!drag)
            ScrollCamera();
        DragBoi();
        cam.transform.position = ClampCamera(cam.transform.position);
    }



    //Code for drag camera movement from
    //https://www.youtube.com/watch?v=Qd3hkKM-UTI
    //GolomOder youtube
    private void DragBoi()
    {
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            difference = (cam.ScreenToWorldPoint(Input.mousePosition)) - cam.transform.position;
            if (!drag)
            {
                drag = true;
                origin = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }
        if (drag)
        {
            cam.transform.position = origin - difference;
        }

    }

    private void ScrollCamera()
    {
        float newSize = cam.orthographicSize - Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        float width = (cam.orthographicSize * 2) * Screen.width / Screen.height;
        float hight = (cam.orthographicSize * 2);
        float divi = 1.04f;
        float minis = width - (width / divi);
        box.size = new Vector2(width - minis,  hight - minis);

    }

    private void MoveCamera()
    {
        Vector2 sidewaysMovementVector = transform.right * Input.GetAxis("Horizontal");
        Vector2 forwardMovementVector = transform.up * Input.GetAxis("Vertical");
        Vector2 movementVector = sidewaysMovementVector + forwardMovementVector;

        transform.position += new Vector3(movementVector.x * Time.deltaTime * cameraSpeed, movementVector.y * Time.deltaTime * cameraSpeed, 0);

    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float CamSpeed = 4;
    public bool Pause { get; set; }
    private float targetZoom;
    private float BorderThickness = 10f;

    Vector2 Limit = new Vector2(0,0);
    private float BorderLimit = 0f;

    // Start is called before the first frame update
    void Start()
    {
        targetZoom = Camera.main.orthographicSize;
    }   

    // Update is called once per frame
    void Update()
    {
        if (!Pause)
        {
            Movement();
            Zoom();
        }
    }

    void Movement()
    {
        Vector2 pos = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - BorderThickness)
        {
            pos.y += CamSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= BorderThickness)
        {
            pos.y -= CamSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - BorderThickness)
        {
            pos.x += CamSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= BorderThickness)
        {
            pos.x -= CamSpeed * Time.deltaTime;
        }
        pos.x = Mathf.Clamp(pos.x,Limit.x - BorderLimit, Limit.y+ BorderLimit);
        pos.y = Mathf.Clamp(pos.y,Limit.x - BorderLimit, Limit.y+ BorderLimit);
        ChangePosition(pos);
    }

    public void ChangePosition(Vector2 pos)
    {
        transform.position = new Vector3(pos.x,pos.y,-11f);
    }

    private void Zoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * 3f;
        targetZoom = Mathf.Clamp(targetZoom,3f,20f);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, Time.deltaTime* 10f);
    }

    public void SetNewLimit(int n)
    {
        Limit = new Vector2(0,n);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float CamSpeed = 5;
    public bool Pause { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }   

    // Update is called once per frame
    void Update()
    {
        if(!Pause)
        Movement();
    }

    void Movement()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        transform.position = new Vector3(transform.position.x + (hor * CamSpeed* Time.deltaTime), transform.position.y + (ver * CamSpeed * Time.deltaTime), -10);
    }

    public void ChangePosition(Vector3 pos)
    {
        transform.position = pos;
    }
}

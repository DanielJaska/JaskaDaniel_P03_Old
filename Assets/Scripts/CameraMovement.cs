using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    public float distance = 2.0f;
    private Vector3 direction;

    private Vector3 offset;
    // Start is called before the first frame update
    void Awake()
    {
        direction = transform.position - target.position;  
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + distance * direction.normalized;
        //transform.position.Set(transform.position.x, Mathf.Clamp(transform.position.y, 2.5f, 10f), transform.position.y);
        transform.LookAt(target);
        if (target)
        {
            if (Input.GetMouseButton(2))
            {
                direction += new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
                //if (transform.position.y < 2.5)
                //{
                //    transform.position.Set(transform.eulerAngles.x, 2.5f, transform.eulerAngles.z);
                //}
            }
        }
    }

    private void Update()
    {
        if(Input.GetMouseButton(2))
        {
            //offset += Input.GetAxis("Horizontal") * Input.GetAxis("Vertical");
        }
    }
}

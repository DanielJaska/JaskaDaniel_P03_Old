using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    public float distance = 2.0f;
    private Vector3 direction;

    //camera scroll
    float cameraDistanceMax = 20f;
    float cameraDistanceMin = 5f;
    float cameraDistance = 10f;
    float scrollSpeed = 0.5f;

    private Vector3 offset;
    // Start is called before the first frame update
    void Awake()
    {
        offset = transform.position - target.position;
        direction = transform.position - target.position;  
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(target);
        if (target)
        {
            if (Input.GetMouseButton(2))
            {
                Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X")  * 10, Vector3.up);
                Quaternion yTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * 10, Vector3.left);

                

                offset = (yTurnAngle * camTurnAngle) * offset;
            }
        }
        Vector3 newPos = target.transform.position + offset;
        transform.position = Vector3.Slerp(transform.position, newPos, .1f);
    }

    private void Update()
    {
        cameraDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
        
    }
}

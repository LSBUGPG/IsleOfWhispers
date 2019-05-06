using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float latMove = .4f, vertMove = 4f;
    
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float l = Input.GetAxis("Mouse ScrollWheel");
        float r = Input.GetAxis("Rotation");

#region

        /* Vector3 forward = transform.TransformDirection(transform.forward);
        forward.y = 0;      forward.Normalize();        Debug.DrawLine(transform.position, transform.position + forward, Color.green);      forward *= v * latMove;

        Vector3 side = transform.TransformDirection(transform.right);
        side.y = 0;     side.Normalize();       Debug.DrawLine(transform.position, transform.position + side, Color.red);     side *= h * latMove;
        
        Vector3 up = transform.TransformDirection(transform.up);
        up.x = 0;       up.z = 0f;      up.Normalize();     Debug.DrawLine(transform.position, transform.position + up, Color.blue);     up *= l * vertMove;
        
        Vector3 comMove = forward + side + up;

        transform.Translate(comMove, Space.World); */

#endregion

        Vector3 forward = transform.forward;
        forward.y = 0;      forward *= (v * latMove);

        Vector3 side = transform.right;
        side.y = 0;     side *= (h * latMove);

        Vector3 up = transform.up;
        up.x = 0;       up.z = 0;       up *= (l * vertMove);
        
        Vector3 comMove = forward + side + up;

        transform.Translate(comMove, Space.World);

        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += r;
        
        if (rot.y > 360f)
        {
            rot.y -= 360f;
        }

        if (rot.y < 0f)
        {
            rot.y += 360f;
        }

        Mathf.Clamp(rot.y, 0f, 360f);
        transform.rotation = Quaternion.Euler(rot);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody rb;
    public float speed = 50;
    public float mov;
    public float rot;
    public GameObject lightball;
    public GameObject staff;
    public float lightball_speed = 300;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mov = 1;
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                mov = -1;
            }
            else
            {
                mov = 0;
            }
            
        }

        Vector3 movement = transform.forward * mov;
        rb.AddForce(movement * speed);
        rot += Input.GetAxis("Mouse X");
        transform.eulerAngles = new Vector3(0, rot, 0);

        if (Input.GetMouseButtonDown(2)||Input.GetKeyDown(KeyCode.E))
        {
            GameObject instLight = Instantiate(lightball, staff.transform.position, Quaternion.identity) as GameObject;
            Rigidbody instLightRB = instLight.GetComponent<Rigidbody>();
            instLightRB.AddForce((staff.transform.forward) * lightball_speed, ForceMode.Impulse);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public float cam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        cam += Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(-cam, transform.eulerAngles.y, 0);
    }
    
}

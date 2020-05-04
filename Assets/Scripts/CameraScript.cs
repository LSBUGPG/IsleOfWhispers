using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public float cam;
    HUDScript hudScript;
    bool paused;

	// Use this for initialization
	void Start () {
        hudScript = FindObjectOfType<HUDScript>();
	}
	
	// Update is called once per frame
	void Update () {
        paused = hudScript.paused;
        if (!paused)
        {
            cam += Input.GetAxis("Mouse Y");
            transform.eulerAngles = new Vector3(-cam, transform.eulerAngles.y, 0);
        }
        
    }
    
}

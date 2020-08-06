using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniff : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CGG_GhostController_Working>())
        {
            other.GetComponent<CGG_GhostController_Working>().isVisible = true;
            Debug.Log("detected");
        }
        if (other.GetComponent<CGG_GhostCon_Modified>())
        {
            other.GetComponent<CGG_GhostCon_Modified>().isvisible = true;
            Debug.Log("detected");
        }
    }
}

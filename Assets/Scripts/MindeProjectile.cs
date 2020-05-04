using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindeProjectile : MonoBehaviour {
    float count;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        count += 1;
        if (count > 200)
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            //Destroy(other.gameObject);
            other.gameObject.BroadcastMessage("Dead");  //activate the public void on the ghost GameObject
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    float count;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        count += 1;
        if (count > 200)
        {
            Debug.Log("Shot disappeared");
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ghost") && other.transform.GetComponent<CGG_GhostController_Working>())
        {
            CGG_GhostController_Working script = other.GetComponent<CGG_GhostController_Working>();
            if (other.gameObject.GetComponent<Ghost_Possessor>())
            {
                other.gameObject.BroadcastMessage("Dead");
                Destroy(gameObject);
            }
            
            else
            {
                if (script.isVisible)
                {
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                
            }
        }
        else
        {
            if (!other.gameObject.GetComponent<SpawnPointScript>())
            {
                Debug.Log("projectile destroyed by " + other.gameObject);
                Destroy(gameObject);
            }

        }
    }
}

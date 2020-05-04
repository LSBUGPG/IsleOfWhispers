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
        if (count > 150)
        {
            Destroy(gameObject);
        }
	}

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ghost") && other.transform.GetComponent<Renderer>())
        {
            if (other.gameObject.GetComponent<Ghost_Possessor>())
            {
                other.gameObject.BroadcastMessage("Dead");
                Destroy(gameObject);
            }
            
            else
            {
                if (other.transform.GetComponent<Renderer>().enabled)
                {
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
                
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

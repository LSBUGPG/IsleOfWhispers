using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashRocks : MonoBehaviour {

    ParticleSystem parSys;
    public GameObject ramp;
    Renderer rend;
    Collider coll;

	// Use this for initialization
	void Start () {
        parSys = GetComponent<ParticleSystem>();
        rend = GetComponent<Renderer>();
        coll = GetComponent<Collider>();
        ramp.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Projectile>())
        {
            parSys.Play();
            rend.enabled = false;
            coll.enabled = false;
            ramp.SetActive(true);
        }
    }
}

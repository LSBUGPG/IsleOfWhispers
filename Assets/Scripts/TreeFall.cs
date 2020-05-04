using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFall : MonoBehaviour {

    Animator anim;
    public GameObject nav;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        nav.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Projectile>())
        {
            Debug.Log("Projectile hit tree");
            anim.SetTrigger("treeFall");
            nav.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost_Possessor : MonoBehaviour {

    CGG_GhostCon_Modified ghostController;
    GameObject dogObject;
    DogScript dog;

	// Use this for initialization
	void Start () {
        ghostController = GetComponent<CGG_GhostCon_Modified>();
        dog = FindObjectOfType<DogScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (ghostController.dogSpotted)
        {
            transform.position = Vector3.Lerp(transform.position, dog.transform.position, 0.5f);
        }
	}
    public void Dead()
    {
        dog.possessed = false;  //set the possesed value of the dog to false
        Destroy(gameObject);
    }
}

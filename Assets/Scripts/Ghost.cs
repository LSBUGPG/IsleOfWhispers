using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject dogObject;  //the dogObject

    public void Dead()
    {
        dogObject.GetComponent<DogScript>().possessed = false;  //set the possesed value of the dog to false
        Destroy(gameObject);
    }
}

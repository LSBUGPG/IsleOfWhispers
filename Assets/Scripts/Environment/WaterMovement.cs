using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour {
    public float speedZ = 1;
    int directionZ = 1;
    public float minZ;
    public float maxZ;
    public float speedY = 1;
    int directionY = 1;
    public float minY;
    public float maxY;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(0, (0.01f * speedY * directionY), (0.01f * speedZ * directionZ));
        if (transform.position.z <= minZ || transform.position.z>= maxZ)
        {
            directionZ *= -1;
        }
        if (transform.position.y <= minY || transform.position.y >= maxY)
        {
            directionY *= -1;
        }
    }
}

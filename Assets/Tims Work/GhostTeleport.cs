using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTeleport : MonoBehaviour {
    CGG_GhostController ghostController;
    bool hasteleported = false;
    GameObject[] teleportlocations;
    int teleportCount = 0;
	// Use this for initialization
	void Start () {
        teleportlocations = GameObject.FindGameObjectsWithTag("spawnpoint");
        ghostController = GetComponent<CGG_GhostController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (ghostController.isvisible && !hasteleported)
        {
            teleportCount += 1;
        }
        if (teleportCount >= 75)
        {
            ghostController.RecalculateNearestSpawnPoint();
            Debug.Log("teleport");
            int spawnvalue = Random.Range(0, teleportlocations.Length);
            transform.position = teleportlocations[spawnvalue].transform.position;
            hasteleported = true;
            ghostController.isvisible = false;
            teleportCount = 0;
            ghostController.movePoint = ghostController.generatePoint();
        }
    }
}

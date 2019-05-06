using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogScript : MonoBehaviour {

    NavMeshAgent agent;
    public GameObject player;
    public float offset;
    public PlayerMovement player_m;
    public bool sit = false;
    public float commandcount = 0;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        offset = Random.Range(-3, 3);
        if (sit)
        {
            agent.SetDestination(player.transform.position + (player.transform.forward * 7));
            transform.LookAt(player.transform.position);
            commandcount += 1;
        }
        else
        {
            if (player_m.mov == 0)
            {
                agent.SetDestination(player.transform.position + (player.transform.forward * (7 + offset)) + (player.transform.right * offset * 4));
            }
            else
            {
                agent.SetDestination(player.transform.position - (player.transform.forward * 4));
            }
        }
        if (commandcount > 600)
        {
            sit = false;
            commandcount = 0;
        }
        
	}
}

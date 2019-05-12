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
    public bool sniff = false;
    public float commandcount = 0;
    Vector3 nearest_ghost;
    public GameObject sniffradius;
    Collider sniffcollider;
    
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        sniffcollider = sniffradius.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        offset = Random.Range(-3, 3);
        float smallest_dist = 1000;
        
        foreach (var ghost in FindObjectsOfType<GhostController>())
        {
           float dist = Vector3.Distance(transform.position, ghost.transform.position);
            if (dist < smallest_dist)
            {
                smallest_dist = dist;
                nearest_ghost = ghost.transform.position;
            }
        }
        if (sit)
        {
            agent.SetDestination(player.transform.position + (player.transform.forward * 7));
            transform.LookAt(player.transform.position);
            commandcount += 1;
        }
        else
        {
            if (sniff)
            {
                agent.SetDestination(nearest_ghost);
                sniffcollider.enabled = true;
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
                    sniffcollider.enabled = false;
                }
            }
            
        }
        if (commandcount > 600)
        {
            sit = false;
            commandcount = 0;
        }

        
	}

}

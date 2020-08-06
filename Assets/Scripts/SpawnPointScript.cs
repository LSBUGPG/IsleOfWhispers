using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SpawnPointScript : MonoBehaviour {
    public bool areThereGhosts;
    Vector3 nearestGhost;
    int totalGhosts;
    public float radius;
    AudioSource audioSrc;
    public AudioMixerSnapshot snapExp;
    public AudioMixerSnapshot snapGho;
    
    // Use this for initialization
    void Start () {
        audioSrc = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        totalGhosts = FindObjectsOfType<CGG_GhostController_Working>().Length;
        Debug.DrawRay(transform.position, Vector3.forward * radius, Color.red, 1);
        float smallest_dist = 1000;
        foreach (var ghost in FindObjectsOfType<CGG_GhostController_Working>())
        {
            float dist = Vector3.Distance(transform.position, ghost.transform.position);
            if (dist < smallest_dist)
            {
                smallest_dist = dist;
                nearestGhost = ghost.transform.position;
            }
        }
        if (totalGhosts == 0 || Vector3.Distance(transform.position, nearestGhost) > radius)
        {
            areThereGhosts = false;
        }
        else
        {
            areThereGhosts = true;
        }
    }
    public void PlayerEnter()
    {
        if (areThereGhosts)
        {
            snapGho.TransitionTo(1.0f);
        }
    }
    public void PlayerStay()
    {
        if (!areThereGhosts)
        {
            snapExp.TransitionTo(1.0f);
        }
    }
    public void PlayerExit()
    {
        snapExp.TransitionTo(1.0f);
    }
}

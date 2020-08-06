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
    public bool inCircle = false;
    public float commandCount = 0;
    Vector3 nearestHiddenGhost;
    Vector3 nearestGhost;
    public GameObject sniffRadius;
    Collider sniffCollider;
    SpawnPointScript[] spawnPoints;
    Vector3 nearestSpawnPoint;
    float nearestSpawnRadius;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    int clipNum;
    public Material matNormal;
    public Material matPossessed;
    public bool possessed = false;
    int audioCount = 0;
    public float spawnDist;
    public float sitDist;
    public float turnSpeed;
    bool isAttacking;
    float attacktimer = 0;
    float timerMax;
    public float startingSpeed;
    float speedMod = 1;
    public float currentSpeed;
    public Animator anim;
    public bool bark = false;
    // Use this for initialization
    void Start () {
        timerMax = Random.Range(4, 5);
        audioSource = gameObject.GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        sniffCollider = sniffRadius.GetComponent<Collider>();
        //spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
        spawnPoints = FindObjectsOfType<SpawnPointScript>();
    }

    // Update is called once per frame
    void Update () {
        agent.speed = startingSpeed * speedMod;

        currentSpeed = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("DogSpeed", currentSpeed);
        //Debug.Log("Nearest Ghost :" + nearestGhost + ", Nearest Hidden Ghost: " + nearestHiddenGhost);
        if (!possessed)
        {
            GetComponent<Renderer>().material = matNormal;
            clipNum = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[clipNum];

            float smallest_dist = 1000;
            foreach (var spawnpoint in spawnPoints)
            {
                float dist = Vector3.Distance(transform.position, spawnpoint.transform.position);
                if (dist < smallest_dist)
                {
                    smallest_dist = dist;
                    nearestSpawnPoint = spawnpoint.transform.position;
                    nearestSpawnRadius = spawnpoint.radius;
                }
            }
            offset = Random.Range(-3, 3);
            float smallest_dist1 = 1000;

            foreach (var ghost in FindObjectsOfType<CGG_GhostController_Working>())
            {
                
                float dist1 = Vector3.Distance(transform.position, ghost.transform.position);
                if (dist1 < smallest_dist1)
                {
                    smallest_dist1 = dist1;
                    nearestGhost = ghost.transform.position;
                    if (!ghost.isVisible)
                    {
                        nearestHiddenGhost = ghost.transform.position;
                    }
                }

            }
            if (!inCircle)
            {
                audioCount = 0;
                sniff = false;
                if (sit)
                {
                    Vector3 dogToPlayer = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                    agent.SetDestination(player.transform.position + (player.transform.forward * 7));
                    if (Vector3.Distance(transform.position, (player.transform.position + (player.transform.forward * 7))) < sitDist)
                    {
                        //transform.LookAt(player.transform.position);
                        Quaternion quat2 = Quaternion.LookRotation(dogToPlayer - transform.position);
                        agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, quat2, turnSpeed * 1.5f * Time.deltaTime);
                        commandCount += 1;
                    }
                    
                }
                else
                {
                    if (player_m.movY == 0 && player_m.movX == 0)
                    {
                        agent.SetDestination(player.transform.position + (player.transform.forward * (7 + offset)) + (player.transform.right * offset * 4));
                    }
                    else
                    {
                        agent.SetDestination(player.transform.position - (player.transform.forward * 4));
                        sniffCollider.enabled = false;
                    }
                }
            }
            if (inCircle)
            {
                audioCount += 1;
                if (audioCount >= 330)
                {
                    audioSource.PlayOneShot(audioClips[clipNum]);
                    audioCount = 0;
                }
                if (sniff)
                {
                    if (Vector3.Distance(transform.position, nearestHiddenGhost) < nearestSpawnRadius)
                    {
                        agent.SetDestination(nearestHiddenGhost);
                    }
                    else
                    {
                        agent.SetDestination(nearestGhost);
                    }
                    bark = false;
                    anim.SetBool("isBarking", bark);
                    sniffCollider.enabled = true;
                }
                else
                {
                    sniffCollider.enabled = false;
                    agent.SetDestination(nearestSpawnPoint);
                    //Debug.Log(Vector3.Distance(transform.position, nearestSpawnPoint));
                    if (Vector3.Distance(transform.position, nearestSpawnPoint) < spawnDist)
                    {
                        //agent.transform.LookAt(new Vector3(nearestGhost.x, transform.position.y, nearestGhost.z));
                        bark = true;
                        anim.SetBool("isBarking", bark);
                    }
                    else
                    {
                        bark = false;
                        anim.SetBool("isBarking", bark);
                    }
                }
            }
            if (commandCount > 600)
            {
                sit = false;
                commandCount = 0;
            }
        }
        if (possessed)
        {
            
            GetComponent<Renderer>().material = matPossessed;
            if (isAttacking)
            {
                speedMod = 1.33f;
                agent.SetDestination(player.transform.position);
                if (Vector3.Distance(transform.position, player.transform.position) < 4f)
                {
                    agent.SetDestination(transform.position);
                }
                print("Charging the player");
            }
            else
            {
                speedMod = 1;
                agent.SetDestination(player.transform.position + (player.transform.forward * 30f));
                attacktimer += Time.deltaTime;
                print(attacktimer + " and then gonna hit the player.");
            }
            if (attacktimer > timerMax)
            {
                attacktimer = 0;
                timerMax = Random.Range(4, 5);
                isAttacking = true;
                print("Retreating");
            }
            Debug.LogFormat("Is Attacking {0}", isAttacking);
            if (Vector3.Distance(transform.position, player.transform.position) < 4f)
            {
                isAttacking = false;
                print("I'm close to the player, stopping attack.");
            }
            

        }
        


        
	}
    
    void FixedUpdate()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<SpawnPointScript>() && !possessed)
        {
            SpawnPointScript spScript = other.gameObject.GetComponent<SpawnPointScript>();
            if (spScript.areThereGhosts)
            {
                audioSource.PlayOneShot(audioClips[clipNum]);
                inCircle = true;
            }
            else
            {
                inCircle = false;
            }
        }
        if (other.gameObject.GetComponent<Ghost_Possessor>())
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            possessed = true;
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<SpawnPointScript>() && !possessed)
        {
            SpawnPointScript spScript = other.gameObject.GetComponent<SpawnPointScript>();
            if (spScript.areThereGhosts)
            {
                inCircle = true;
            }
            else
            {
                inCircle = false;
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Ghost_Possessor>())
        {
            //Debug.Log("exit");
            possessed = false;
            
        }
        if (other.gameObject.GetComponent<SpawnPointScript>())
        {
            sniff = false;
        }
    }
}

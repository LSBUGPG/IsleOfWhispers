using System.Collections;
using UnityEngine;

public class CGG_GhostController_Working : MonoBehaviour
{
    [Tooltip("The ghosts speed in m/s")] public float speed = 0.05f;
    [Tooltip("The time between ghost shots")] public float shotCooldown = 2f;
    [Tooltip("The range of the ghost will detect the player")] public float detectionRange = 30f;
    [Tooltip("The chance for a ghost to dodge"), Range(0f, 1f)] public float dodgeChance = .3f;
    public Vector3 movePoint;
    public GameObject ghostshot;
    public float shot_speed = 300;
    public bool isVisible = false;
    public bool dogSpotted;

    bool dodgingShot;
    bool inRange = false;
    float radius;
    float speedMulti;
    float count = 5;
    float dodgeRadius;
    Vector3 deltaH;
    Vector3 dodgeDirection;
    Vector3 nearestSpawnPoint;
    Renderer rend;
    GameObject player;
    GameObject[] spawnPoints;

    //--------------------------------------------------------
    //------------------------TO DO LIST----------------------
    // - move faster when detected
    // - try to avoid the player's attacks - 30% chance
    // - make more erratic movement patterns

    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
        rend = GetComponent<Renderer>();
        movePoint = generatePoint();
        player = GameObject.FindGameObjectWithTag("Player"); //Getting a reference to the player transfrom
        dodgeRadius = Mathf.Abs(transform.localScale.z + 1f);
        RecalculateNearestSpawnPoint();
        StartCoroutine(checkFire());
    }

    void Update()
    {
        RecalculateNearestSpawnPoint();
        
        if(dodgingShot) 
        {
            transform.position += dodgeDirection * Time.deltaTime * speed * speedMulti;
        }
        else if (!dogSpotted)
        {   
            transform.position += transform.forward * Time.deltaTime * speed * speedMulti;
        }

        float distToCentre = Vector3.Distance(transform.position, nearestSpawnPoint);

        if(distToCentre >= radius)
        {
            movePoint = generatePoint();
            Debug.Log("Changed Direction");
        }

        if (isVisible)
        {
            rend.enabled = true;
            speedMulti = 1.27f;
        }
        else
        {
            rend.enabled = false;
            speedMulti = 1;
        }

        if (count <= 0 && inRange && isVisible)
        {
            if (GetComponent<Ghost_Possessor>())
            {
                dogSpotted = true;
            }
            else
            {
                fireShot();
                count = shotCooldown;
            }
            
        }
        else if (count > 0)
        {   
            count -= Time.deltaTime;
        }
    }

    public void RecalculateNearestSpawnPoint()
    {
        float smallest_dist = 1000;
        foreach (var spawnpoint in spawnPoints)
        {
            float dist = Vector3.Distance(transform.position, spawnpoint.transform.position);
            if (dist < smallest_dist)
            {
                smallest_dist = dist;
                nearestSpawnPoint = spawnpoint.transform.position;
                radius = spawnpoint.transform.localScale.x / 2;
            }
        }
    }

    public Vector3 generatePoint()
    {
        float x = UnityEngine.Random.Range(-radius, radius);
        float y = Mathf.Sqrt((radius * radius) - (x * x));
        y = UnityEngine.Random.Range(-y, y);
        Vector3 mov2 = new Vector3(x, 0, y);
        RecalculateNearestSpawnPoint();
        mov2 += nearestSpawnPoint;
        mov2.y = transform.position.y;
        transform.LookAt(mov2);
        return mov2;
    }

    void OnTriggerEnter(Collider impact)
    {
        if (impact.tag.Equals("LightBall"))
        {
            impact.GetComponent<SphereCollider>().enabled = false;
            Vector3 shotLocalPosition = transform.InverseTransformPoint(impact.transform.position); //get shot local position
            Vector3 shotLocalForward = transform.InverseTransformDirection(impact.transform.forward);//get shot local direction
            float gradient = shotLocalForward.z / shotLocalForward.x; // get gradient of shot based on x z plane of ghost
            float zIntercept = shotLocalPosition.z - (shotLocalPosition.x * gradient); //get intercept of shot (I.e. crosses in front or behind ghost)
            float toShot = Vector3.Dot(transform.forward, shotLocalForward); // get dot product to effectively check the angle of intercept (I.e. coming directly from in front of behind ghost)

            string dodingDir = "Not Dodging";

            //below is statement (should) produce best direction to dodge potential shot. It does not guarantee a dodge, but if enough time warning is given it should dodge

            if(Mathf.Abs(toShot) > .5f) // check if shot is coming from within 45' angle in front or behind of ghost (I.e. mostly directly at ghost)
            {
                if(zIntercept >= 0f) // if shot will pass in front of ghost
                {
                    if(gradient >= 0f) // if shot is coming from positive gradient (back left or front right)
                    {
                        dodgeDirection = transform.right; //dodge to ghost's right
                        dodingDir = "Doding Right";
                    }
                    else // else shot is coming from negative gradient (back right or front left)
                    {
                        dodgeDirection = -transform.right; //dodge to ghost's left
                        dodingDir = "Doding Left";
                    }
                    
                }
                else // else shot will pass behind ghost
                {
                    if(gradient > 0f) // if shot is coming from positive gradient (back left or front right)
                    {
                        dodgeDirection = -transform.right; //dodge to ghost's left
                        dodingDir = "Doding Left";
                    }
                    else // else shot is coming from negative gradient (back right or front left)
                    {
                        dodgeDirection = transform.right; //dodge to ghost's right
                        dodingDir = "Doding Right";
                    }
                }
            }
            else // else shot is coming from an angle outside of a 45' coneof ghost (I.e. mostly from the side of the ghost)
            {
                if(zIntercept >= 0f) // if shot will pass in front of ghost
                {
                    if(gradient >= 0f) // if shot is coming from positive gradient (back left or front right)
                    {
                        dodgeDirection = -transform.forward; // dodge backwards
                        dodingDir = "Doding Backwards";
                    }
                    else
                    {
                        dodgeDirection = transform.forward; //else keep going to outrun
                        dodingDir = "Doding Forwards";
                    }
                }
                else // if shot will pass behind of ghost
                {
                    if(gradient < 0f) // if shot is coming from negative gradient (back right or front left)
                    {
                        dodgeDirection = -transform.forward; // dodge backwards
                        dodingDir = "Doding Backwards";
                    }
                    else
                    {
                        dodgeDirection = transform.forward; //else keep going to outrun
                        dodingDir = "Doding Forwards";
                    }
                }
            }

            float chanceToBeat = UnityEngine.Random.Range(0f, 1f);

            if(chanceToBeat < dodgeChance) // checking if the ghost will attempt dodge the shot, increase the 'dodgeChance' (to a max of 1f) to increase the chance of making a dodge attempt
            {
                dodgingShot = true;
                StartCoroutine(dodgeWaitTime());
                Debug.Log(dodingDir);
            }
            else
            {
                Debug.Log("Not Dodging");
            }
        }
    }

    IEnumerator dodgeWaitTime()
    {
        yield return new WaitForSeconds(1.5f); //This is the time the ghost will be dodging for, change as required
        dodgingShot = false;
        generatePoint();
    }

    IEnumerator checkFire()
    {
        while(true)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            float distance = Vector3.Distance(player.transform.position, transform.position);
            inRange = (distance < detectionRange);

            #region commented out code
           //Vector3 dir = player.transform.position - transform.position;
           // RaycastHit hit;

           // Debug.DrawRay(transform.position + transform.forward, dir);
           // if(Physics.Raycast(transform.position + transform.forward, dir, out hit, detectionRange))
           // {
           //     if(hit.transform.tag == "Player")
           //     {   inRange = true; }
           //     else
           //     {   inRange = false;    }
           // }
           #endregion

            yield return new WaitForSeconds(.1f);
        }
    }

    void fireShot()
    {
//        Debug.Log("Shoot");
        Vector3 dir = (player.transform.position - transform.position).normalized;
        GameObject instGhost = Instantiate(ghostshot, transform.position, Quaternion.identity) as GameObject;
        Rigidbody instLightRB = instGhost.GetComponent<Rigidbody>();
        instLightRB.AddForce(dir * shot_speed, ForceMode.Impulse);
    }
}

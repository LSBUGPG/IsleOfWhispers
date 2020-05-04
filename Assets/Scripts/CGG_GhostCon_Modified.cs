using System.Collections;
using System;
using UnityEngine;

public class CGG_GhostCon_Modified : MonoBehaviour
{
    [Tooltip("The ghosts speed in m/s"), /* Range(0, .4f) */] public float speed = 0.05f;
    [Tooltip("The ghosts vertical speed in m/s"), Range(0, 1f)] public float vSpeed = 0.5f;
    [Tooltip("The time between ghost shots")] public float shotCooldown = 2f;
    [Tooltip("The range of the ghost will detect the player")] public float detectionRange = 30f;

    //
    //Added code
    //

    [Tooltip("The random chance of the ghost changing direction")] public float randDirChance = .1f;
    [Tooltip("The time between each chance of changing direction")] public float directionChangeChance = 2f;
    [Tooltip("The chance for a ghost to dodge"), Range(0f, 1f)] public float dodgeChance = .3f;
    bool dodgeReverse;
    Vector3 inverseDir;

    //
    //End   
    // 

    public int decimalPlaces = 2;

    //[Tooltip("The time in seconds the ghost waits to change directions"), Range(0, 1)] public float changetime = .1f;
    //[Tooltip("The chance that a ghost will change direction, higher values makes it less likely"), Range(0, 10)] public int chanceChange = 5;
    //[Tooltip("The 'time' required before a ghost will switch turning direction, higher values increase the 'time'"), Range(0, 10)] public int switchTime = 5;
    //[Tooltip("The minimum height in meters that a ghost can fly up")] public float minHeight = 1f;
    //[Tooltip("The maximum height in meters that a ghost can fly up")] public float maxHeight = 4f;
    //[Tooltip("The minimum angle in degrees which the ghost can turn each change")] public float minAngle = 5f;
    //[Tooltip("The maximum angle in degrees which the ghost can turn each change")] public float maxAngle = 30f;

    //--------------------------------------------------------
    //------------------------TO DO LIST----------------------
    // - move faster when detected
    // - try to avoid the player's attacks - 30% chance
    // - make more erratic movement patterns

    int liveChance;
    float targetHeight;
    float speedMult;
    float count = 5;
    bool inRange = false;
    Vector3 deltaH;
    public bool isvisible = false;
    Renderer rend;
    public GameObject ghostshot;
    public float shot_speed = 300;
    Vector3 nearestSpawnPoint;
    GameObject[] spawnPoints;
    public Vector3 movePoint;
    float distance, travelledDistance, radius, fracDist, startTime;
    public bool dogSpotted;
    

    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawnpoint");
        //liveChance = chanceChange;
        //StartCoroutine(changeDirection());
        StartCoroutine(checkFire());
        //targetHeight = transform.position.y;
        //tarHeightChange(targetHeight);
        rend = GetComponent<Renderer>();
        RecalculateNearestSpawnPoint();
        movePoint = generatePoint();

        //
        //Added code
        //

        StartCoroutine(randChangeDirection());

        //
        //End
        //
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

    void Update()
    {
        RecalculateNearestSpawnPoint();
        travelledDistance = (Time.time - startTime) * speed * speedMult;
        fracDist = travelledDistance / distance;

        if (!dogSpotted)
        {   transform.position = Vector3.Lerp(transform.position, movePoint, fracDist); }

        //
        //Added code
        //

        else if(dodgeReverse) transform.position += inverseDir * Time.deltaTime * speed * speedMult;

        //
        //End
        //
        
        float TX = (float)Math.Round(transform.position.x, decimalPlaces);
        float TZ = (float)Math.Round(transform.position.z, decimalPlaces);
        float MX = (float)Math.Round(movePoint.x, decimalPlaces);
        float MZ = (float)Math.Round(movePoint.z, decimalPlaces);

        if (TX == MX && TZ == MZ)
        {   movePoint = generatePoint();    }

        if (isvisible)
        {
            rend.enabled = true;
            speedMult = 1.27f;
        }
        else
        {
            rend.enabled = false;
            speedMult = 1;
        }

#region  old movement(?) code
        //Vector3 dir = transform.forward.normalized;
        //float y = transform.position.y;

        //if(y > targetHeight && deltaH.y > 0)
        //{   tarHeightChange(y); }
        //else if(y < targetHeight && deltaH.y < 0)
        //{   tarHeightChange(y); }

        //dir += deltaH;
        //transform.Translate(dir * speed);

        //Debug.LogFormat("count = {0} inRange = {1}, isvisible= {2}", count, inRange, isvisible);
        #endregion

        if (count <= 0 && inRange && isvisible)
        {
            if (GetComponent<Ghost_Possessor>())
            {
                dogSpotted = true;
            }
            else
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                transform.LookAt(player.transform);
                Debug.Log("in range");
                fireShot();
                count = shotCooldown;
            }
            
        }
        else if (count > 0)
        {   count -= Time.deltaTime;    }
    }

    //
    //Added Code
    //

    IEnumerator randChangeDirection()
    {
        while(true)
        {
            yield return new WaitForSeconds(directionChangeChance);
            float rand = UnityEngine.Random.Range(0f, 1f);

            if(rand < randDirChance) generatePoint();
        }
    }

    void OnTriggerEnter(Collider impact)
    {
        if(impact.tag.Equals("LightBall"))
        {
            float toBeat = UnityEngine.Random.Range(0f, 1f);
            
            if(toBeat < dodgeChance) reverseDelayPoint();
        }
    }

    IEnumerator reverseDelayPoint()
    {
        dodgeReverse = true;
        inverseDir = -transform.forward.normalized;
        yield return new WaitForSeconds(1f);
        dodgeReverse = false;
        generatePoint();
    }

    //
    //End
    //

#region old change direction code
    //IEnumerator changeDirection()
    //{
    //    int c = 0;
    //    float LR = 1;

    //    while (true)
    //    {
    //        int i = (int)Random.Range(0, 10);
    //        c += i;
            
    //        if (c * 10 >  switchTime * 10)
    //        {   LR = -LR;   c = 0;  }

    //        if (i < liveChance)
    //        {
    //            Vector3 angChange = new Vector3(0, Random.Range(minAngle, maxAngle) * LR, 0);
    //            angChange += transform.localEulerAngles;
    //            transform.localRotation = Quaternion.Euler(angChange);
    //        }

    //        yield return new WaitForSeconds(changetime);
    //    }
    //}
#endregion

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
        Debug.Log("Shoot");
        GameObject instGhost = Instantiate(ghostshot, transform.position, Quaternion.identity) as GameObject;
        Rigidbody instLightRB = instGhost.GetComponent<Rigidbody>();
        instLightRB.AddForce((transform.forward) * shot_speed, ForceMode.Impulse);
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
        distance = Vector3.Distance(transform.position, mov2);
        startTime = Time.time;
        transform.LookAt(mov2);
        return mov2;
    }

#region  gizmos draw code
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(movePoint, .5f);
    //}
#endregion
}

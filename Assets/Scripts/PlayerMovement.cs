using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//0, 2.89, -4.29

public class PlayerMovement : MonoBehaviour {

    private Rigidbody rb;
    public float speed = 50;
    float speed_mult;
    public float movX;
    public float movY;
    float movDiag;
    public float rot;
    public GameObject lightball;
    public GameObject staff;
    public float lightball_speed = 300;
    public int health = 8;
    AudioSource audioSource;
    bool onGround;
    public float rcLength;
    public float jumpHeight;
    public float upForce;
    public float jumpCooldown = 0.1f;
    float jumpTime;
    HUDScript hudScript;
    bool paused;
    public bool ispossessed = false;
    bool sprint = false;
    public float sprintmeter;
    public float sprintMax;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();
        hudScript = FindObjectOfType<HUDScript>();
        sprintmeter = sprintMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (sprintmeter >= sprintMax)
        {
            sprintmeter = sprintMax;
        }
        //if (sprintmeter <= 0)
       // {
            //sprintmeter = 0;
        //}
        if (ispossessed)
        {
            speed_mult = 0.5f;
        }
        else
        {
            if (sprint)
            {
                speed_mult = 1.5f;
            }
            else
            {
                speed_mult = 1;
            }
            
        }
        paused = hudScript.paused;
        jumpTime += Time.deltaTime;
        if (jumpTime > 100)
        {
            jumpTime = 1;
        }
        CheckIfGrounded();
        //Debug.Log(onGround);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (Input.GetKey(KeyCode.W))
        {
            movY = 1;
        }
        else
        {
            if (Input.GetKey(KeyCode.S))
            {
                movY = -1;
            }
            else
            {
                movY = 0;
            }
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            movX = 1;
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                movX = -1;
            }
            else
            {
                movX = 0;
            }
        }
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            movDiag = 1/(Mathf.Sqrt(2));
        }
        else
        {
            movDiag = 1;
        }
        if (Input.GetKey(KeyCode.LeftShift) && sprintmeter > 0)
        {
            sprint = true;
            sprintmeter -= 1;
        }
        if (!Input.GetKey(KeyCode.LeftShift) || sprintmeter <= 0)
        {
            sprint = false;
            sprintmeter += 0.33f;
        }
        //create a raycast for the jumping animation
        RaycastHit hit;
        Vector3 physicsCentre = this.transform.position + this.GetComponent<BoxCollider>().center;
        Debug.DrawRay(physicsCentre, Vector3.down * rcLength, Color.red, 1);

        if (Input.GetKeyDown("space") && onGround == true && jumpTime > jumpCooldown)
        {
            jumpTime = 0.0f;
            Debug.Log("Jump");
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHeight);
        }
        Vector3 movement = (transform.forward * movY) + (transform.right * movX);
        Vector3 velocity = rb.velocity;
        velocity.y = 0.0f;
        if (onGround)
        {
            Debug.Log(movement * speed * speed_mult * movDiag);
            rb.AddForce(movement * speed * speed_mult * movDiag /*, ForceMode.VelocityChange */);
            rb.AddForce(Vector3.up * upForce);
        }
        if (!paused)
        {
            rot += Input.GetAxis("Mouse X");
            transform.eulerAngles = new Vector3(0, rot, 0);
        }
        

        if ((Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.E)) && !paused && !ispossessed)
        {
            audioSource.PlayOneShot(audioSource.clip);
            GameObject instLight = Instantiate(lightball, staff.transform.position, Quaternion.identity) as GameObject;
            Rigidbody instLightRB = instLight.GetComponent<Rigidbody>();
            instLightRB.AddForce((staff.transform.forward) * lightball_speed, ForceMode.Impulse);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GhostProj"))
        {
            Destroy(other.gameObject);
            health -= 1; 
        }
        if (other.gameObject.GetComponent<SpawnPointScript>())
        {
            SpawnPointScript spawnScript = other.gameObject.GetComponent<SpawnPointScript>();
            spawnScript.PlayerEnter();
        }
        if (other.CompareTag("Exit") && hudScript.ghostcount == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<SpawnPointScript>())
        {
            SpawnPointScript spawnScript = other.gameObject.GetComponent<SpawnPointScript>();
            spawnScript.PlayerStay();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<SpawnPointScript>())
        {
            SpawnPointScript spawnScript = other.gameObject.GetComponent<SpawnPointScript>();
            spawnScript.PlayerExit();
        }
    }

    public bool CheckIfGrounded()
    {
        onGround = Physics.Raycast(transform.position, -Vector3.up, rcLength);
        return onGround;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExitScript : MonoBehaviour {
    HUDScript hud;
	// Use this for initialization
	void Start () {
        hud = FindObjectOfType<HUDScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Next Level");
        if (other.gameObject.CompareTag("Player") && hud.ghostcount <= 0)
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

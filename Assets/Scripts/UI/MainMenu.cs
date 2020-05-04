using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    bool buttonPressed = false;
    bool play = false;
    bool options = false;
    bool quit = false;
    int clickCount = 0;
    public Animator playButton;
    public Animator optionsButton;
    public Animator quitButton;
    public GameObject optionsMenu;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (buttonPressed)
        {
            clickCount += 1;
        }
        if (clickCount >= 45 && play)
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (clickCount >= 45 && quit)
        {
            Application.Quit();
        }
        if (clickCount >= 45 && options)
        {
            OptionsMenu();
        }
    }
    public void PlayGame(){
        if (!buttonPressed){
            buttonPressed = true;
            play = true;
            playButton.SetBool("Play", play);
        }

    }
    public void QuitGame(){
        if (!buttonPressed)
        {
            buttonPressed = true;
            quit = true;
            quitButton.SetBool("Quit", quit);
        }

    }
    public void Options(){
        if (!buttonPressed)
        {
            buttonPressed = true;
            options = true;
            optionsButton.SetBool("Options", options);
        }
    }
    void OptionsMenu()
    {
        clickCount = 0;
        buttonPressed = false;
        options = false;
        optionsMenu.SetActive(true);
        gameObject.SetActive(false);

    }
}

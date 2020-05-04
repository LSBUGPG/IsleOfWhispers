using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour {
    bool buttonPressed = false;
    bool returnToMenu = false;
    int clickCount = 0;
    public Animator returnButton;
    public GameObject mainMenu;
    public AudioMixer mixer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (buttonPressed)
        {
            clickCount += 1;
        }
        if (clickCount >= 45 && returnToMenu)
        {
            MainMenu();
        }
	}
    public void SetMusic(float musicValue)
    {
        mixer.SetFloat("MusicValue", Mathf.Log10(musicValue) * 20);
    }
    public void SetSfx(float sfxValue)
    {
        mixer.SetFloat("SfxValue", Mathf.Log10(sfxValue) * 20);
    }
    public void ReturnToMenu()
    {
        if (!buttonPressed)
        {
            buttonPressed = true;
            returnToMenu = true;
            returnButton.SetBool("Return", returnToMenu);
        }
    }

    void MainMenu()
    {
        clickCount = 0;
        buttonPressed = false;
        returnToMenu = false;
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}

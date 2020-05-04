using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDScript : MonoBehaviour
{
    public int ghostcount;
    int health = 8;
    public Image healthbar;
    public Text ghosttext;
    PlayerMovement playerscript;
    public Animator healthAnim;
    public Animator continueButton;
    public Animator optionsButton;
    public Animator quitButton;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    bool buttonPressed = false;
    bool continueGame = false;
    bool options = false;
    bool quit = false;
    public bool paused;
    int clickCount;
    public Slider sprintSlider;
    // Use this for initialization
    void Start()
    {
        playerscript = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        sprintSlider.value = playerscript.sprintmeter;
        health = playerscript.health;
        healthAnim.SetInteger("Health", health);
        ghostcount = FindObjectsOfType<CGG_GhostController>().Length;
        ghosttext.text = ghostcount.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (buttonPressed)
        {
            clickCount += 1;
        }
        if (clickCount >= 45 && continueGame)
        {
            ResumeGame();
        }
        if (clickCount >= 45 && quit)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }
        if (clickCount >= 45 && options)
        {
            OptionsMenu();
        }
    }
    public void Continue()
    {
        if (!buttonPressed)
        {
            buttonPressed = true;
            continueGame = true;
            continueButton.SetBool("Continue", continueGame);
        }
    }
    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        paused = true;
        buttonPressed = false;
        continueGame = false;
        options = false;
        quit = false;
    }
    void ResumeGame()
    {
        clickCount = 0;
        buttonPressed = false;
        continueGame = false;
        options = false;
        quit = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }
    public void QuitGame()
    {
        if (!buttonPressed)
        {
            buttonPressed = true;
            quit = true;
            quitButton.SetBool("Quit", quit);
        }
    }
    public void Options()
    {
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
        pauseMenu.SetActive(false);

    }
}

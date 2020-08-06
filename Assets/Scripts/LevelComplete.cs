using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public HUDScript hudScript;
    int ghostCount;
    Animator anim;
    public bool ghostsAreCleared = false;
    // Start is called before the first frame update
    void Start()
    {
        ghostCount = hudScript.ghostcount;
        
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ghostCount = hudScript.ghostcount;
        if (ghostCount == 0)
        {
            levelTransition();
        }
    }

    void levelTransition()
    {
        ghostsAreCleared = true;
        anim.SetBool("ghostsCleared", ghostsAreCleared);

    }

    public void sceneTransition()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

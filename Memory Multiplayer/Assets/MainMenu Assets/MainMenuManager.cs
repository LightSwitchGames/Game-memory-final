using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator anim1;
    public Animator anim2;
    public Animator anim3;
    void Start()
    {
        StartCoroutine(StartAnimations());
    }

    private IEnumerator StartAnimations()
    {
        yield return new WaitForSeconds(0.5f);

        anim1.enabled = true;
        anim2.enabled = true;
        anim3.enabled = true;
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}

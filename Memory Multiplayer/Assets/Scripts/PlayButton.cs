using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Play);
    }
    
    private void Play()
    {
        SceneManager.LoadScene(1);
    }
}

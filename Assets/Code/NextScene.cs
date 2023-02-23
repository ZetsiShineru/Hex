using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextScene : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("map1");
    }
    
    public void BacktoMenu()
    {
        SceneManager.LoadScene("Start");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

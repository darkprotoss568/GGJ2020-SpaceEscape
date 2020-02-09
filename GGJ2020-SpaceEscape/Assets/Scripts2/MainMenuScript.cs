using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

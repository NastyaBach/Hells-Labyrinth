using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public GameObject floatingLevel;
    public void ChangeScenes(int SceneNumber)
    {
        SceneManager.LoadScene(SceneNumber);
    }

    public void ExitGame()
    {
        Debug.Log("The game is closed");
        Application.Quit();
    }
}

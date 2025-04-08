using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadLevel(string sceneName)
    {
        Debug.Log("Loaded level");
        SceneManager.LoadScene(sceneName);
    }

    public void QuitLevel()
    {
        Application.Quit();
    }
}

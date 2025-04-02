using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //Load a scene based on a string name
    [SerializeField] private string sceneName;

    public void LoadLevel()
    {
        Debug.Log("Loaded level");
        SceneManager.LoadScene(sceneName);
    }

    public void QuitLevel()
    {
        Application.Quit();
    }
}

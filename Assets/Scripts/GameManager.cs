using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] SceneFader sceneFader;
    public static GameManager sharedInstance;
    const string LEVEL_SELECT = "LevelSelect";

    // Creates the singleton GameManager that will be in every scene becaue we call "DontDestroyOnLoad"
    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(this);
        }
        else if (sharedInstance != this)
        {
            DestroyImmediate(gameObject);
        }

    }

    public void GoToLevelSelect()
    {
        GoToScene(LEVEL_SELECT);
    }
     
    public void PlayLevel(string levelName)
    {
        GoToScene(levelName);
    }

    private void GoToScene(string sceneName)
    {
        sceneFader.FadeTo(sceneName);
        //SceneManager.LoadScene(sceneName);
    }
}

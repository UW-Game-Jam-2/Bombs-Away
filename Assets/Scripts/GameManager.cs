using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] SceneFader sceneFader;
    public static GameManager sharedInstance;
    const string LEVEL_SELECT = "LevelSelect";

    private int[] goldShotTarget = new int[] { 20, 20, 20, 20, 20, 20 };
    private int[] silverShotTarget = new int[] { 40, 40, 40, 40, 40, 40 };

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

    public int GetGoalCoinsByLevel(int level) {
        int coinCountToReturn = 0;
        switch(level) {
            case 1: 
                coinCountToReturn = 40;
                break;
            case 2:
                coinCountToReturn = 40;
                break;
            case 3:
                coinCountToReturn = 40;
                break;
            case 4:
                coinCountToReturn = 40;
                break;
            case 5:
                coinCountToReturn = 40;
                break;
            case 6:
                coinCountToReturn = 40;
                break;
            default: break;
        }

        return coinCountToReturn;
    }

    public int GetMaxChestsByLevel(int level) {
        int chestCountToReturn = 0;
        switch (level) {
            case 1:
                chestCountToReturn = 3;
                break;
            case 2:
                chestCountToReturn = 3;
                break;
            case 3:
                chestCountToReturn = 3;
                break;
            case 4:
                chestCountToReturn = 3;
                break;
            case 5:
                chestCountToReturn = 3;
                break;
            case 6:
                chestCountToReturn = 3;
                break;
            default: break;
        }

        return chestCountToReturn;
    }

    public int GetGoldShotTargetByLevel(int level) {
        int shotTargetToReturn = 0;
        switch (level) {
            case 1:
                shotTargetToReturn = goldShotTarget[0];
                break;
            case 2:
                shotTargetToReturn = goldShotTarget[1];
                break;
            case 3:
                shotTargetToReturn = goldShotTarget[2];
                break;
            case 4:
                shotTargetToReturn = goldShotTarget[3];
                break;
            case 5:
                shotTargetToReturn = goldShotTarget[4];
                break;
            case 6:
                shotTargetToReturn = goldShotTarget[5];
                break;
            default: break;
        }

        return shotTargetToReturn;
    }

    public int GetSilverShotTargetByLevel(int level) {
        int shotTargetToReturn = 0;
        switch (level) {
            case 1:
                shotTargetToReturn = silverShotTarget[0];
                break;
            case 2:
                shotTargetToReturn = silverShotTarget[1];
                break;
            case 3:
                shotTargetToReturn = silverShotTarget[2];
                break;
            case 4:
                shotTargetToReturn = silverShotTarget[3];
                break;
            case 5:
                shotTargetToReturn = silverShotTarget[4];
                break;
            case 6:
                shotTargetToReturn = silverShotTarget[5];
                break;
            default: break;
        }

        return shotTargetToReturn;
    }
}

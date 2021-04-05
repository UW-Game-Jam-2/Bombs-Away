using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManagerScript : MonoBehaviour
{
    private int maxChestCount = 3;
    private int currentChestCount = 0;
    private int goalCoinCount = 40;
    private int currentCoinCount = 0;
    private int shotCount = 0;

    private int currentLevel = 1;
    private int goldShotTarget = 10;
    private int silverShotTarget = 20;

    public static ObjectivesManagerScript instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("currentLevel")) {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
        }

        goalCoinCount = GameManager.sharedInstance.GetGoalCoinsByLevel(currentLevel);
        maxChestCount = GameManager.sharedInstance.GetMaxChestsByLevel(currentLevel);
        goldShotTarget = GameManager.sharedInstance.GetGoldShotTargetByLevel(currentLevel);
        silverShotTarget = GameManager.sharedInstance.GetSilverShotTargetByLevel(currentLevel);
    }

    public void UpdateChestCount() {
        currentChestCount++;

        CheckAllObjectives();
        Debug.Log("Chest count: " + currentChestCount);
    }

    public void UpdateCoinCount(int newCoinValue) {
        currentCoinCount += newCoinValue;
        CheckAllObjectives();
    }

    public void UpdateShotCount() {
        shotCount++;
    }

    void CheckAllObjectives() {
        if (currentChestCount.Equals(maxChestCount) && currentCoinCount >= goalCoinCount) {
            Debug.Log("Level " + currentLevel + " completed!");
            Debug.Log("Coins Collected: " + currentCoinCount);
            Debug.Log("Chests found: " + currentChestCount);
            Debug.Log("Shots Taken: " + shotCount);
            //TODO
            //pop-up you win screen
        }
    }
}

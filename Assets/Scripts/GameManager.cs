using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Static info about the price and level after which the player unlocks a bomb
public class StoreInfo
{
    //public string title;
    //public string explanation;

    public int cost;
    public int unlockedAfterLevel;
    public ExplosionType explosionType;
    public string description;

    public StoreInfo(int cost, int unlockedAfterLevel, ExplosionType explosionType, string description)
    {
        this.cost = cost;
        this.unlockedAfterLevel = unlockedAfterLevel;
        this.explosionType = explosionType;
        this.description = description;
    }
}

public class PlayerInfo
{
    public int gold;
    public int highestLevelBeat;

    public PlayerInfo(int gold, int highestLevelBeat)
    {
        this.gold = gold;
        this.highestLevelBeat = highestLevelBeat;
    }
}


public class GameManager : MonoBehaviour
{
    [Header("StoreInfo")]
    [SerializeField] int clusterCost;
    [SerializeField] int clusterUnlock;
    [SerializeField] int horizontalCost;
    [SerializeField] int horizontalUnlock;
    [SerializeField] int moabCost;
    [SerializeField] int moabUnlock;
    [SerializeField] int verticalCost;
    [SerializeField] int verticalUnlock;
    [SerializeField] int stickyCost;
    [SerializeField] int stickyUnlock;



    [Space]
    [SerializeField] SceneFader sceneFader;
    public static GameManager sharedInstance;
    const string LEVEL_SELECT = "LevelSelect";
    const string CURRENT_LEVEL = "currentLevel";

    private int[] goldShotTarget = new int[] { 20, 20, 20, 20, 20, 20 };
    private int[] silverShotTarget = new int[] { 40, 40, 40, 40, 40, 40 };

    public int highestLevelBeat = 10;
    public int playerAvailableGold = 100;

    public PlayerInfo playerInfo;


    /// <summary>
    ///  Player starts with the Basic bomb.  The other bombs are locked.  They become purchasble. Once purchased they become available
    /// </summary>
    public Dictionary<ExplosionType, StoreInfo> bombStoreInfo = new Dictionary<ExplosionType, StoreInfo>();
    public List<StoreInfo> lockedBombs = new List<StoreInfo>();// { ExplosionType.CLUSTER, ExplosionType.HORIZONTAL, ExplosionType.MOAB, ExplosionType.STICKY, ExplosionType.VERTICAL };
    public List<StoreInfo> purchasableBombs = new List<StoreInfo>();
    public List<ExplosionType> availableBombs = new List<ExplosionType> { ExplosionType.BASIC };

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

        print("awake game manager");
        StoreInfo cluster = new StoreInfo(clusterCost, clusterUnlock, ExplosionType.CLUSTER, "Explodes into three smaller bombs ");
        StoreInfo moab = new StoreInfo(moabCost, moabUnlock, ExplosionType.MOAB, "The largest and loudest bomb");
        StoreInfo horizontal = new StoreInfo(horizontalCost, horizontalUnlock, ExplosionType.HORIZONTAL, "Blasts away a horizontal swath of land");
        StoreInfo vertical = new StoreInfo(verticalCost, verticalUnlock, ExplosionType.VERTICAL, "Blows away a lot the land directly below it.");
        StoreInfo sticky = new StoreInfo(stickyCost, stickyUnlock, ExplosionType.STICKY, "Sticks to the first thing it touches");

        lockedBombs.Add(cluster);
        lockedBombs.Add(moab);
        lockedBombs.Add(horizontal);
        lockedBombs.Add(vertical);
        lockedBombs.Add(sticky);


        bombStoreInfo[ExplosionType.CLUSTER] = cluster;
        bombStoreInfo[ExplosionType.MOAB] = moab;
        bombStoreInfo[ExplosionType.HORIZONTAL] = horizontal;
        bombStoreInfo[ExplosionType.VERTICAL] = vertical;
        bombStoreInfo[ExplosionType.STICKY] = sticky;


        playerInfo = new PlayerInfo(playerAvailableGold, highestLevelBeat);
    }

    private void Start()
    {

    }

    /// <summary>
    ///  PUBLIC METHODS
    /// </summary>
    ///

    /// for testing


    /// Called by the Salty Dog store when the LevelSelect scene reappears.  It updates the invetory data so we know what is purchaseable and what isnt
    public void ReloadInventory()
    {
        print("reload inventory");

        List<StoreInfo> newLockedBombs = new List<StoreInfo>();
        List<StoreInfo> newPurchasableBombs = new List<StoreInfo>();

        foreach (KeyValuePair<ExplosionType, StoreInfo> kvp in bombStoreInfo)
        {

            print(kvp);
            foreach (StoreInfo storeInfo in this.lockedBombs)
            {
                if (kvp.Value.explosionType == storeInfo.explosionType) { 
                    if (playerInfo.highestLevelBeat >= kvp.Value.unlockedAfterLevel)
                    {
                        newPurchasableBombs.Add(storeInfo);
                    }
                    else
                    {
                        newLockedBombs.Add(storeInfo);
                    }
                }
            }
        }

        this.lockedBombs = newLockedBombs;
        this.purchasableBombs = newPurchasableBombs;
    }

    /// <summary>
    /// Called by the Salty Dog store.
    /// </summary>
    /// <param name="explosion">which bomb they bought</param>

    public void BuyBomb(ExplosionType type)
    {
        // take away player money
        playerInfo.gold -= bombStoreInfo[type].cost;

        /// Remove the bomb from purchasable bombs
        purchasableBombs.Remove(bombStoreInfo[type]);

        /// Add it to the available bombs
        availableBombs.Add(type);

        // reload
        ReloadInventory();
    }

    public void GoToLevelSelect()
    {
        GoToScene(LEVEL_SELECT);
    }
     
    public void PlayLevel(string levelName)
    {

        // need for the objective tracking
        int levelIndex = (int)levelName[levelName.Length - 1];
        PlayerPrefs.SetInt("CURRENT_LEVEL", levelIndex);

        GoToScene(levelName);
    }

    private void GoToScene(string sceneName)
    {
        sceneFader.FadeTo(sceneName);
    }

    public void GoToLevelSelectScene() {
        SceneManager.LoadScene("LevelSelect");
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

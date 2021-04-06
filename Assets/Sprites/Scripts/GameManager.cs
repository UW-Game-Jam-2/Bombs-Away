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
    const string AVAILABLE_BOMBS_KEY = "AVAILABLE_BOMBS_KEY";

    private int[] goldShotTarget = new int[] { 20, 20, 20, 20, 20, 20 };
    private int[] silverShotTarget = new int[] { 40, 40, 40, 40, 40, 40 };

    public int highestLevelBeat = 10;
    public int playerAvailableGold = 100;

    public PlayerInfo playerInfo;


    // DONE: Create a Island "Drawer" script.  It will check the highest completed level for the player and render all the islands and colliders for the "revealed" islands.
    // In PROGRESS: work with Jarrod to bring the narrative in in small chunks
    // DONE: create a DialogueLoader that loads the correct narrative based on the most recently finished level and if they have seen it before
    // TODO: Tweak costs for bombs/when they unlock
    // TODO: Show the player's gold in the Salty Dog
    // TODO: bring in Quinns art
    // TODO: Upload a build and get everyone to test for bugs
    // TODO: Show the player's objectives in the level select
    // TODO: Make sure the first and second level are pretty easy with bad bombs
    // TODO: Make sure


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

    private void Update()
    {
        /// ONLY FOR DEBUG PURPOSES
        ///
        if (Input.GetKeyDown(KeyCode.Y))
        {
            playerInfo.highestLevelBeat += 1;
            GoToLevelSelect();
        }


        if(sceneFader == null)
        {
            sceneFader = FindObjectOfType<SceneFader>();
        }
    }

    /// <summary>
    ///  PUBLIC METHODS
    /// </summary>
    ///

    /// for testing


    /// Called by the Salty Dog store when the LevelSelect scene reappears.  It updates the invetory data so we know what is purchaseable and what isnt
    public void ReloadInventory()
    {

        List<StoreInfo> newLockedBombs = new List<StoreInfo>();
        HashSet<StoreInfo> newPurchasableBombs = new HashSet<StoreInfo>();
        newPurchasableBombs.UnionWith(purchasableBombs);

        foreach (KeyValuePair<ExplosionType, StoreInfo> kvp in bombStoreInfo)
        {

            if (!this.availableBombs.Contains(kvp.Value.explosionType)) { 

                foreach (StoreInfo storeInfo in this.lockedBombs)
                {
                    if (kvp.Value.explosionType == storeInfo.explosionType)
                    {
                        // You have unlocked the buy bought you havent bought yet
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
        }

        this.lockedBombs = newLockedBombs;
        this.purchasableBombs = new List<StoreInfo>(newPurchasableBombs);

        //print("INVENTORY RELOADED");
    }

    /// <summary>
    /// Called by the Salty Dog store.
    /// </summary>
    /// <param name="explosion">which bomb they bought</param>

    public void BuyBomb(ExplosionType type)
    {
        // take away player money
        //print(playerInfo.gold);
        playerInfo.gold -= bombStoreInfo[type].cost;
        //print(playerInfo.gold);

        /// Remove the bomb from purchasable bombs
        List < StoreInfo > newPurchasableBombs = new List<StoreInfo>();
        foreach (StoreInfo bomb in purchasableBombs)
        {

            if (bomb.explosionType != type)
            {
                newPurchasableBombs.Add(bomb);
            }
        }
        this.purchasableBombs = newPurchasableBombs;

        /// Add it to the available bombs
        availableBombs.Add(type);

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

        // need for the level config
        string availableBombString = "";
        foreach (ExplosionType type in availableBombs)
        {
            availableBombString += $"{type.ToString()},";
        }
        PlayerPrefs.SetString(AVAILABLE_BOMBS_KEY, availableBombString.Remove(availableBombString.Length - 1, 1));
        //print(availableBombString.Remove(availableBombString.Length - 1, 1));

        GoToScene(levelName);
    }

    private void GoToScene(string sceneName)
    {
        sceneFader.FadeTo(sceneName);
    }

    public void GoToLevelSelectScene()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public int GetGoalCoinsByLevel(int level)
    {
        int coinCountToReturn = 0;
        switch (level)
        {
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

    public int GetMaxChestsByLevel(int level)
    {
        int chestCountToReturn = 0;
        switch (level)
        {
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

    public int GetGoldShotTargetByLevel(int level)
    {
        int shotTargetToReturn = 0;
        switch (level)
        {
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

    public int GetSilverShotTargetByLevel(int level)
    {
        int shotTargetToReturn = 0;
        switch (level)
        {
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

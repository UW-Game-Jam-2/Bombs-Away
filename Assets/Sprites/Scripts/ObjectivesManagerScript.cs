using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectivesManagerScript : MonoBehaviour
{
    private int maxChestCount = 3;
    private int currentChestCount = 0;
    private int goalCoinCount = 40;
    private int currentCoinCount = 0;
    private int shotCount = 0;

    private int currentLevel = 1;
    public int goldShotTarget = 10;
    public int silverShotTarget = 20;
    private string levelGrade = "NO_GRADE";

    [SerializeField]
    private Canvas levelCompleteCanvas;
    [SerializeField]
    private TextMeshProUGUI coinText;
    [SerializeField]
    private TextMeshProUGUI chestText;
    [SerializeField]
    private TextMeshProUGUI bombText;
    [SerializeField]
    private TextMeshProUGUI overallText;

    [SerializeField]
    private TextMeshProUGUI coinObjectiveText;
    [SerializeField]
    private TextMeshProUGUI chestObjectiveText;
    [SerializeField]
    private TextMeshProUGUI bombObjectiveText;
    [SerializeField]
    private TextMeshProUGUI goalObjectiveText;

    public static ObjectivesManagerScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
        }

        goalCoinCount = GameManager.sharedInstance.GetGoalCoinsByLevel(currentLevel);
        maxChestCount = GameManager.sharedInstance.GetMaxChestsByLevel(currentLevel);
        goldShotTarget = GameManager.sharedInstance.GetGoldShotTargetByLevel(currentLevel);
        silverShotTarget = GameManager.sharedInstance.GetSilverShotTargetByLevel(currentLevel);

        coinObjectiveText.text = "0";
        chestObjectiveText.text = "0/" + maxChestCount;
        bombObjectiveText.text = "x0";
        goalObjectiveText.text = "(" + goldShotTarget + ")";
        goalObjectiveText.color = Color.yellow;
    }

    public void UpdateChestCount()
    {
        currentChestCount++;
        chestObjectiveText.text = currentChestCount + "/" + maxChestCount;
        CheckAllObjectives();
    }

    public void UpdateCoinCount(int newCoinValue)
    {
        currentCoinCount += newCoinValue;
        coinObjectiveText.text = currentCoinCount + "";
        CheckAllObjectives();
    }

    public void UpdateShotCount()
    {
        shotCount++;
        bombObjectiveText.text = shotCount + "";

        if (shotCount > goldShotTarget && shotCount <= silverShotTarget)
        {
            goalObjectiveText.text = "(" + silverShotTarget + ")";
            goalObjectiveText.color = Color.gray;
        }
        else if (shotCount > silverShotTarget)
        {
            goalObjectiveText.text = "(" + silverShotTarget + "+)";
            goalObjectiveText.color = Color.magenta;
        }
    }

    IEnumerator GameEndDelay(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SetLevelCompleteValues();
    }

    void SetLevelCompleteValues()
    {
        levelCompleteCanvas.gameObject.SetActive(true);
        coinText.text = currentCoinCount + "";
        chestText.text = currentChestCount + "/" + maxChestCount;
        bombText.text = "x" + shotCount;

        levelGrade = "Overall: ";
        if (shotCount <= goldShotTarget)
        {
            levelGrade += "GOLD";
        }
        else if (shotCount <= silverShotTarget)
        {
            levelGrade += "SILVER";
        }
        else
        {
            levelGrade += "BRONZE";
        }

        overallText.text = levelGrade;

        if (PlayerPrefs.HasKey("totalCoinCount"))
        {
            PlayerPrefs.SetInt("totalCoinCount", PlayerPrefs.GetInt("totalCoinCount") + currentCoinCount);
        }
        else
        {
            PlayerPrefs.SetInt("totalCoinCount", currentCoinCount);
        }

        if (PlayerPrefs.HasKey("Level" + currentLevel + "_Grade"))
        {
            string savedLevelGrade = PlayerPrefs.GetString("Level" + currentLevel + "_Grade");
            if (savedLevelGrade.Equals("BRONZE") && (levelGrade.Equals("SILVER") || levelGrade.Equals("GOLD")))
            {
                PlayerPrefs.SetString("Level" + currentLevel + "_Grade", levelGrade);
            }
            else if (savedLevelGrade.Equals("SILVER") && levelGrade.Equals("GOLD"))
            {
                PlayerPrefs.SetString("Level" + currentLevel + "_Grade", levelGrade);
            }
        }
        else
        {
            PlayerPrefs.SetString("Level" + currentLevel + "_Grade", levelGrade);
        }
    }

    void CheckAllObjectives()
    {
        if (currentChestCount.Equals(maxChestCount) && currentCoinCount >= goalCoinCount)
        {
            StartCoroutine(GameEndDelay(3f));
        }
    }
}

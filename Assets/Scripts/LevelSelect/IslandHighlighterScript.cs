using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandHighlighterScript : MonoBehaviour
{
    [SerializeField] Text theSatlyDogTitleText;
    [SerializeField] GameObject theSaltyDogStoreInCanvas;
    [SerializeField] Text gameTitleText;
    [SerializeField] Text creditsText;
    [SerializeField] Text pressSpaceToPlay;

    [Header ("Level view settings")]
    [SerializeField] Text levelTitleText;
    [SerializeField] Text levelNameText;
    [SerializeField] Text levelGrade;
    [SerializeField] Text levelScoreTitle;
    [SerializeField] List<Image> levelImages;
    [SerializeField] int numLevels;

    private List<string> levelGrades = new List<string>();
    private int currentLevel = 0;


    private string nextLevelName;

    // Start is called before the first frame update
    void Start()
    {
        ShipMovementScriptLevelSelect.didCollideWithIsland += DidCollide;
        ShipMovementScriptLevelSelect.didTriggerOpenOcean += DidMoveToOpenOcean;

        /// get the level data and store it here

        levelGrades.Add("N/A");
        for (int i = 1; i < numLevels+1; i++)
        {
            int currentLevel = i;
            string key = $"Level{currentLevel}_Grade";

            if (PlayerPrefs.HasKey(key))
            {
                string grade = PlayerPrefs.GetString(key);
                //print($"Found grade for level {i}: {grade}");

                levelGrades.Add(grade);
            } else
            {

                //print($"No grade for level {i}");
                levelGrades.Add("N/A");
            }


        }

        TurnOnCredits();

    }

    private void OnDisable()
    {
        ShipMovementScriptLevelSelect.didCollideWithIsland -= DidCollide;
        ShipMovementScriptLevelSelect.didTriggerOpenOcean -= DidMoveToOpenOcean;
    }

    // Credits on
    // Level ui off
    // Salty dog ui off
    void TurnOnCredits()
    {

        creditsText.enabled = true;
        gameTitleText.enabled = true;

        ToggleLevelTextUI(false);
        ToggleSaltyDogUI(false);

    }

    void TurnOffCredits()
    {

        creditsText.enabled = false;
        gameTitleText.enabled = false;
    }

    void ToggleLevelTextUI(bool onOff)
    {
        levelTitleText.enabled = onOff;
        levelNameText.enabled = onOff;
        pressSpaceToPlay.enabled = onOff;
        levelGrade.enabled = onOff;
        levelScoreTitle.enabled = onOff;
        //print(levelGrades[currentLevel]);

        if (levelGrades[currentLevel] == "Overall: BRONZE")
        {
            //print($"Setting image for level {currentLevel} with a Bronze grade");
            levelImages[0].enabled = onOff;
            levelImages[1].enabled = false;
            levelImages[2].enabled = false;
            levelGrade.text = "Bronze";

        } else if (levelGrades[currentLevel] == "Overall: SILVER")
        {
            //print($"Setting image for level {currentLevel} with a Silver grade");
            levelImages[0].enabled = false;
            levelImages[1].enabled = onOff;
            levelImages[2].enabled = false;
            levelGrade.text = "Silver";

        } else if (levelGrades[currentLevel] == "Overall: GOLD")
        {
            //print($"Setting image for level {currentLevel} with a Gold grade");
            levelImages[0].enabled = false;
            levelImages[1].enabled = false;
            levelImages[2].enabled = onOff;
            levelGrade.text = "Gold";

        }
        else if (levelGrades[currentLevel] == "N/A")
        {
            levelImages[0].enabled = false;
            levelImages[1].enabled = false;
            levelImages[2].enabled = false;
            levelGrade.text = "No score yet";
        }
    }

    void ToggleSaltyDogUI(bool onOff)
    {
        theSatlyDogTitleText.enabled = onOff;
        theSaltyDogStoreInCanvas.SetActive(onOff);
    }

    void DidMoveToOpenOcean()
    {
        TurnOnCredits();
    }

    void DidCollide(string islandName)
    {
        if (islandName == "TheSaltyDog")
        {
            DisplaySaltyDog();
        } else
        {
            DisplayLevel(islandName);
        }
        

    }


    void DisplayLevel(string islandName)
    {

        currentLevel = Convert.ToInt32($"{islandName[islandName.Length - 1]}");
        //print($"grade- Level is {currentLevel}");

        TurnOffCredits();
        ToggleLevelTextUI(true);
        ToggleSaltyDogUI(false);
        levelNameText.text = $"{islandName[islandName.Length - 1]}";
        nextLevelName = islandName;
    }

    void DisplaySaltyDog()
    {
        TurnOffCredits();
        ToggleSaltyDogUI(true);
        ToggleLevelTextUI(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (levelTitleText.isActiveAndEnabled)
            {
                GameManager.sharedInstance.PlayLevel(nextLevelName);
            }
        }
    }


}

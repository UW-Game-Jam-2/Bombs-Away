using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandHighlighterScript : MonoBehaviour
{
    [SerializeField] Text theSatlyDogTitleText;
    [SerializeField] GameObject theSaltyDogStore;
    [SerializeField] Text gameTitleText;
    [SerializeField] Text levelTitleText;
    [SerializeField] Text levelNameText;
    [SerializeField] Text creditsText;
    [SerializeField] Text pressSpaceToPlay;


    private string nextLevelName;

    // Start is called before the first frame update
    void Start()
    {
        ShipMovementScriptLevelSelect.didCollideWithIsland += DidCollide;
        ShipMovementScriptLevelSelect.didTriggerOpenOcean += DidMoveToOpenOcean;

        TurnOnCredits();

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
    }

    void ToggleSaltyDogUI(bool onOff)
    {
        theSatlyDogTitleText.enabled = onOff;
        theSaltyDogStore.SetActive(onOff);
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

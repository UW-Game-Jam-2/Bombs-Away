using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandHighlighterScript : MonoBehaviour
{

    [SerializeField] Text levelTitleText;
    [SerializeField] Text levelNameText;
    [SerializeField] Text creditsText;
    [SerializeField] Text pressSpaceToPlay;

    // Start is called before the first frame update
    void Start()
    {
        ShipMovementScriptLevelSelect.didCollideWithIsland += DidCollide;

        levelTitleText.enabled = false;
        levelNameText.enabled = false;
        pressSpaceToPlay.enabled = false;

        creditsText.enabled = true;

    }

    void DidCollide(string islandName)
    {
        print(islandName);
        levelTitleText.enabled = true;
        levelNameText.text = islandName;
        levelNameText.enabled = true;
        pressSpaceToPlay.enabled = true;

        creditsText.enabled = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (levelTitleText.isActiveAndEnabled)
            {
                GameManager.sharedInstance.PlayLevel(levelNameText.text);
            }
        }
    }


}

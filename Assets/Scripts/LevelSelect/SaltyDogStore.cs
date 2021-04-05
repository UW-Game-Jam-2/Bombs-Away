using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaltyDogStore : MonoBehaviour
{

    const string CLUSTER = "ClusterBomb";
    const string MOAB = "MoabBomb";
    const string DRILL = "DrillBomb";
    const string STICKY = "StickyBomb";
    const string LINE = "LineBomb";

    public List<GameObject> bombUpgrades;
    public List<Button> buyButtons;
    public List<Image> lockedImages;
    public List<Image> checkmarkImages;
    public List<Text> costTexts;
    public List<Text> descriptionTexts;


    private void Start()
    {
        //get the inventory
        GameManager.sharedInstance.ReloadInventory();

        // populate store things
        PopulateStore();

    }

    void PopulateStore()
    {
        print("populate stores");

        List<StoreInfo> purchasableBombs = GameManager.sharedInstance.purchasableBombs;
        List<StoreInfo> lockedBombs = GameManager.sharedInstance.lockedBombs;
        List<ExplosionType> availableBombs = GameManager.sharedInstance.availableBombs;


        foreach (ExplosionType type in availableBombs)
        {
            switch (type) {
                case ExplosionType.CLUSTER:
                    buyButtons[0].gameObject.SetActive(false);
                    checkmarkImages[0].enabled = true;
                    break;
                case ExplosionType.VERTICAL:
                    buyButtons[1].gameObject.SetActive(false);
                    checkmarkImages[1].enabled = true;
                    break;
                case ExplosionType.STICKY:
                    buyButtons[2].gameObject.SetActive(false);
                    checkmarkImages[2].enabled = true;
                    break;
                case ExplosionType.MOAB:
                    buyButtons[3].gameObject.SetActive(false);
                    checkmarkImages[3].enabled = true;
                    break;
                case ExplosionType.HORIZONTAL:
                    buyButtons[4].gameObject.SetActive(false);
                    checkmarkImages[4].enabled = true;
                    break;

            }
        }


        /// turn on all the buy buttons
        /// turn off all the locks buttons
        foreach (StoreInfo type in purchasableBombs)
        {
            print(type);

            bool playerCanAfford = GameManager.sharedInstance.playerInfo.gold > type.cost;
            switch (type.explosionType)
            {
                case ExplosionType.CLUSTER:
                    buyButtons[0].gameObject.SetActive(true);
                    buyButtons[0].interactable = playerCanAfford;
                    costTexts[0].color = playerCanAfford ? Color.black : Color.magenta;
                    costTexts[0].text = $"x{type.cost}";
                    lockedImages[0].enabled = false;
                    checkmarkImages[0].enabled = false;
                    descriptionTexts[0].text = type.description;
                    break;
                case ExplosionType.VERTICAL:
                    buyButtons[1].gameObject.SetActive(true);
                    buyButtons[1].interactable = playerCanAfford;
                    costTexts[1].color = playerCanAfford ? Color.black : Color.red;
                    costTexts[1].text = $"x{type.cost}";
                    lockedImages[1].enabled = false;
                    checkmarkImages[1].enabled = false;
                    descriptionTexts[1].text = type.description;
                    break;
                case ExplosionType.STICKY:
                    buyButtons[2].gameObject.SetActive(true);
                    buyButtons[2].interactable = playerCanAfford;
                    costTexts[2].color = playerCanAfford ? Color.black : Color.red;
                    costTexts[2].text = $"x{type.cost}";
                    lockedImages[2].enabled = false;
                    checkmarkImages[2].enabled = false;
                    descriptionTexts[2].text = type.description;
                    break;
                case ExplosionType.MOAB:
                    buyButtons[3].gameObject.SetActive(true);
                    buyButtons[3].interactable = playerCanAfford;
                    costTexts[3].color = playerCanAfford ? Color.black : Color.red;
                    costTexts[3].text = $"x{type.cost}";
                    lockedImages[3].enabled = false;
                    checkmarkImages[3].enabled = false;
                    descriptionTexts[3].text = type.description;
                    break;
                case ExplosionType.HORIZONTAL:
                    buyButtons[4].gameObject.SetActive(true);
                    buyButtons[4].interactable = playerCanAfford;
                    costTexts[4].color = playerCanAfford ? Color.black : Color.magenta;
                    costTexts[4].text = $"x{type.cost}";
                    lockedImages[4].enabled = false;
                    checkmarkImages[4].enabled = false;
                    descriptionTexts[4].text = type.description;
                    break;
                default:
                    break;

            }
        }

        print("====");
        /// turn off all the buy buttons
        /// turn on all the locks buttons
        foreach (StoreInfo type in lockedBombs)
        {
            print(type);
            switch (type.explosionType)
            {
                case ExplosionType.CLUSTER:
                    buyButtons[0].gameObject.SetActive(false);
                    lockedImages[0].enabled = true;
                    checkmarkImages[0].enabled = false;
                    descriptionTexts[0].text = $"Unlocked after level {type.unlockedAfterLevel}";
                    costTexts[0].text = "";
                    break;
                case ExplosionType.VERTICAL:
                    buyButtons[1].gameObject.SetActive(false);
                    lockedImages[1].enabled = true;
                    checkmarkImages[1].enabled = false;
                    descriptionTexts[1].text = $"Unlocked after level {type.unlockedAfterLevel}";
                    costTexts[1].text = "";
                    break;
                case ExplosionType.STICKY:
                    buyButtons[2].gameObject.SetActive(false);
                    lockedImages[2].enabled = true;
                    checkmarkImages[2].enabled = false;
                    descriptionTexts[2].text = $"Unlocked after level {type.unlockedAfterLevel}";
                    costTexts[2].text = "";
                    break;
                case ExplosionType.MOAB:
                    buyButtons[3].gameObject.SetActive(false);
                    lockedImages[3].enabled = true;
                    checkmarkImages[3].enabled = false;
                    descriptionTexts[3].text = $"Unlocked after level {type.unlockedAfterLevel}";
                    costTexts[3].text = "";
                    break;
                case ExplosionType.HORIZONTAL:
                    buyButtons[4].gameObject.SetActive(false);
                    lockedImages[4].enabled = true;
                    checkmarkImages[4].enabled = false;
                    descriptionTexts[4].text = $"Unlocked after level {type.unlockedAfterLevel}";
                    costTexts[4].text = "";
                    break;
                default:
                    break;

            }
        }
    }

    public void BuyBombType(string bombType)
    {
        switch (bombType)
        {
            case CLUSTER:
                GameManager.sharedInstance.BuyBomb(ExplosionType.CLUSTER);
                break;

            case DRILL:
                GameManager.sharedInstance.BuyBomb(ExplosionType.VERTICAL);
                break;

            case LINE:
                GameManager.sharedInstance.BuyBomb(ExplosionType.HORIZONTAL);
                break;

            case MOAB:
                GameManager.sharedInstance.BuyBomb(ExplosionType.MOAB);
                break;

            case STICKY:
                GameManager.sharedInstance.BuyBomb(ExplosionType.STICKY);
                break;

            default:
                break;
        }

        PopulateStore();
    }
}

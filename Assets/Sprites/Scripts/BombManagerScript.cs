using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManagerScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject[] bombList;

    private string bombListFromLevelSelect;
    const string AVAILABLE_BOMBS_KEY = "AVAILABLE_BOMBS_KEY";

    private GameObject[] availableBombList;

    public static BombManagerScript instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        if (PlayerPrefs.HasKey(AVAILABLE_BOMBS_KEY)) {
            bombListFromLevelSelect = PlayerPrefs.GetString(AVAILABLE_BOMBS_KEY);
            Debug.Log(bombListFromLevelSelect);
            ParseBombList();
        }
    }

    void Start()
    {
        
        
    }

    void ParseBombList() {
        string[] bombs = bombListFromLevelSelect.Split(',');

        availableBombList = new GameObject[bombs.Length];

        for (int i = 0; i < bombs.Length; i++) {
            switch(bombs[i]) {
                case "BASIC":
                    availableBombList[i] = LookupBomb("Bomb");
                    break;
                case "CLUSTER":
                    availableBombList[i] = LookupBomb("Bomb_Cluster");
                    break;
                case "MOAB":
                    availableBombList[i] = LookupBomb("Bomb_MOAB");
                    break;
                case "HORIZONTAL":
                    availableBombList[i] = LookupBomb("Bomb_Horizontal");
                    break;
                case "VERTICAL":
                    availableBombList[i] = LookupBomb("Bomb_Vertical");
                    break;
                case "STICKY":
                    availableBombList[i] = LookupBomb("Bomb_Sticky");
                    break;
                default: break;
            }
        }
    }

    private GameObject LookupBomb(string name) {
        foreach (GameObject bomb in bombList) {
            if (bomb.name.Equals(name)) {
                return bomb;
            }
        }
        return null;
    }

    public GameObject[] GetAvailableBombList() {
        return availableBombList;
    }
}

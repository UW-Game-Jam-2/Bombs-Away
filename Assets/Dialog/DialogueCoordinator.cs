using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCoordinator : MonoBehaviour
{

    [SerializeField] List<GameObject> dialogues;
    // Start is called before the first frame update
    void Start()
    {
        int highestLevelBeaten = GameManager.sharedInstance.playerInfo.highestLevelBeat;

        for (int i = 0; i < dialogues.Count; i++)
        {
            // if you beat 1 then you see 1 
            if (i == highestLevelBeaten)
            {
                dialogues[i].GetComponent<DialogueTrigger>().enabled = true;
            }
            else
            {
                dialogues[i].GetComponent<DialogueTrigger>().enabled = false;
            }
        }
    }
}
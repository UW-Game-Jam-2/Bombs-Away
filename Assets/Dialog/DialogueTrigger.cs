using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private DialogueManager dialogueManager;

    /// state to handle the player pressing space but only trigger it once per press
    private bool spaceBarPressedDown = false;

    void Start()
    {
        print("finding");
        dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.StartDialogue(dialogue);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape))
        {
            spaceBarPressedDown = true;
        }

        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape)) && spaceBarPressedDown)
        {
            dialogueManager.DisplayNextSentence();

            // set this back to false so we only do this once per press
            spaceBarPressedDown = false;
        }
    }
}

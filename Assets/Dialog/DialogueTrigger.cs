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

    //void Start()
    //{

    //}

    private void OnEnable()
    { 
        dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.StartDialogue(dialogue);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            spaceBarPressedDown = true;
        }

        if ((Input.GetKeyDown(KeyCode.Return)) && spaceBarPressedDown)
        {
            dialogueManager.DisplayNextSentence();

            // set this back to false so we only do this once per press
            spaceBarPressedDown = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            dialogueManager.EndDialogue();
        }
    }
}

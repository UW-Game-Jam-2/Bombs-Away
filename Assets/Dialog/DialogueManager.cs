using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    // UI text fields
    public Text dialogueName;
    public Text dialogueText;
    public Image dialogueImage;

    // animator
    public Animator animator;

    private Queue<string> sentences;
    private Queue<Sprite> images;

    private void Awake()
    {
        sentences = new Queue<string>();
        images = new Queue<Sprite>();
    }

    /// <summary>
    /// Public Methods
    /// </summary>
    
    /// Starts a diaglogue
    public void StartDialogue(Dialogue dialogue)
    {
        /// start the animator
        animator.SetBool("IsOpen", true);


        /// clear out any sentences in the Queue
        sentences.Clear();

        /// loop thru the sentences in the Dialogue type and add them to the queue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        /// loop thru the images in the Dialogue type and add them to the queue
        foreach (Sprite sprite in dialogue.dialogueImage)
        {
            images.Enqueue(sprite);
        }


        // show the name
        dialogueName.text = dialogue.name;

        // show the sentence
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        /// there is nothing left to display.  Early return
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // show the image
        Sprite sprite = images.Dequeue();
        dialogueImage.sprite = sprite;

        // show the sentence
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }

    // Enumerator the types the setence
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            //wait a single frame
            yield return null;
        }
    }

    public void EndDialogue()
    {
        /// end the animations
        animator.SetBool("IsOpen", false);

    }
}

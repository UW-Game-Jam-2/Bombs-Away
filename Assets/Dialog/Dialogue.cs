using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    
    public string name;
    public Sprite[] dialogueImage;
    [TextArea(3,7)]
    public string[] sentences;
}

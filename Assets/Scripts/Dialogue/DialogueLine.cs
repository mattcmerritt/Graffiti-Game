using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An example of how to make a Dialogue ScriptableObject
// This class will contain all the information that we need
// to show a line of dialogue

[CreateAssetMenu]
public class DialogueLine : ScriptableObject
{
    public string SpeakerName;
    public Sprite SpeakerImage;
    [TextArea(5, 10)]
    public string Message;
    public DialogueLine NextLine; // reference to another line to continue conversation
}

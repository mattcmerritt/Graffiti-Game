using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    // State information
    private bool ConversationActive;

    // Holder for the current line to display
    [SerializeField] private DialogueLine CurrentLine;  // the dialogue line to display
    
    // Necessary components loaded from Editor
    [SerializeField] private Image SpeakerImage;        // the image on the left with the speaker's image
    [SerializeField] private TMP_Text SpeakerName;      // the title field that holds the speaker's name
    [SerializeField] private TMP_Text Message;          // the message field that holds the speaker's text
    [SerializeField] private GameObject DialogueBox;    // the entire dialogue interface at the bottom of the screen

    // Events that can be attached to for detecting conversation completion
    public event Action OnConversationOver;

    private void Update()
    {
        if (ConversationActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GoToNextLine();
            }
        }
    }

    public void DisplayCurrentLine()
    {
        // Use the information from the current DialogueLine to fill out the dialogue UI
        SpeakerName.text = CurrentLine.SpeakerName;
        Message.text = CurrentLine.Message;
        SpeakerImage.sprite = CurrentLine.SpeakerImage;
    }

    public void StartConversation(DialogueLine firstLine)
    {
        // If the dialogue interface is not visible right now, make it appear
        if(DialogueBox.activeSelf == false)
        {
            DialogueBox.SetActive(true);
        }

        // Set the current DialogueLine to the one passed in
        CurrentLine = firstLine;
        DisplayCurrentLine();

        ConversationActive = true;
    }

    public void GoToNextLine()
    {
        // Set the current DialogueLine to the next one in the conversation
        CurrentLine = CurrentLine.NextLine;

        // If there are no more lines in the conversation, hide the dialogue interface
        if(CurrentLine == null)
        {
            DialogueBox.SetActive(false);
            OnConversationOver?.Invoke();

            ConversationActive = false;
        }
        // Otherwise, display the new line
        else
        {
            DisplayCurrentLine();
        }
    }
}

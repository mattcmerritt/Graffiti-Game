using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTester : MonoBehaviour
{
    // Components
    [SerializeField] private DialogueUI DialogueUI;
    [SerializeField] private TMP_Dropdown Dropdown;

    // Lines and conversations
    [SerializeField] private List<DialogueLine> InitialLines;
    [SerializeField] private DialogueLine SelectedLine;

    private void Start()
    {
        if (SelectedLine == null && InitialLines.Count > 0)
        {
            SelectedLine = InitialLines[0];
        }
    }

    public void ActivateDialogue()
    {
        DialogueUI.StartConversation(SelectedLine);
    }

    public void SelectNewConversation(int index)
    {
        SelectedLine = InitialLines[index];
    }
}

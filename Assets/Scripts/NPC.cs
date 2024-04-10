using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName = "NPC1";
    public string dialogueText = "Hello, traveler!";
    private DialogueManager dialogueManager;

    public void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene!");
        }
    }

    public void StartDialogue()
    {
        Debug.Log(dialogueText);
        dialogueManager.ShowDialogue(npcName, dialogueText);
    }
    public void ExitDialogue()
    {
        Debug.Log("Exit dialogue...");
        dialogueManager.HideDialogue();
    }
}

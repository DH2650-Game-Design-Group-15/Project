using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public GameObject dialogueTemplate;
    public GameObject dialogueNameObject;
    public GameObject dialogueTextObject;
    public GameObject dialogueHint;

    TextMeshProUGUI dialogueText;
    TextMeshProUGUI dialogueName;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowDialogue(string name, string dialogue)
    {
        dialogueText = dialogueTextObject.GetComponent<TextMeshProUGUI>();
        dialogueName = dialogueNameObject.GetComponent<TextMeshProUGUI>();
        dialogueText.text = dialogue;
        dialogueName.text = name;
        dialogueTemplate.SetActive(true);
    }

    public void HideDialogue()
    {
        dialogueTemplate.SetActive(false);
    }
}

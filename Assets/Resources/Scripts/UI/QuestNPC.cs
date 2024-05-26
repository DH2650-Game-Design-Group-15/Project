using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    private GameObject questMenu;
    private GameObject questHint;
    private InventoryInput inventoryInput;
    private Transform scoutTransform;
    private bool playerInRange = false;

    void Start()
    {
        // Find and cache the InventoryInput script
        inventoryInput = FindObjectOfType<InventoryInput>();
        if (transform.parent != null){
            scoutTransform = transform.parent;
            Transform scoutSpawnerObject = scoutTransform.parent;
            Transform canvas = scoutSpawnerObject.Find("Canvas");
            questHint = canvas.Find("QuestHint").gameObject;
            questMenu = canvas.Find("QuestMenu").gameObject;
        }
    }

    void Update()
    {
        // Check if the player is in range and the Q key is pressed
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            inventoryInput.SetCursor(true);
            questHint.SetActive(false);
            ToggleQuestMenu();
        }
    }

    // Toggle the QuestMenu's active state
    private void ToggleQuestMenu()
    {
        bool isActive = questMenu.activeSelf;
        questMenu.SetActive(!isActive);
        inventoryInput.SetCursor(!isActive);
        scoutTransform.GetComponent<AIScouting>().SetDialogue(!isActive);
        if (isActive) {
            scoutTransform.GetComponent<AIScouting>().human.isStopped = false;
        }
    }

    // When the player enters the collider
    private void OnTriggerEnter(Collider other)
    {   
        if (GetComponentInParent<Fractions>().IsNeutral() || GetComponentInParent<Fractions>().IsFriendly()) {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                questHint.SetActive(true);
            }
        }
    }

    // When the player exits the collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            questMenu.SetActive(false);
            questHint.SetActive(false);
            inventoryInput.SetCursor(false);
        }
    }

}

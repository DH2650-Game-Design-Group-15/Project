using UnityEngine;

public class QuestMenuController : MonoBehaviour
{
    public GameObject questMenu;
    public GameObject questHint;
    private InventoryInput inventoryInput;
    private bool playerInRange = false;

    void Start()
    {
        // Find and cache the InventoryInput script
        inventoryInput = FindObjectOfType<InventoryInput>();
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
    }

    // When the player enters the collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            questHint.SetActive(true);
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
        }
    }

}

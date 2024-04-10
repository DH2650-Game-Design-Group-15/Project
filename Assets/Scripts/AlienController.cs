using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    public Transform gunFront;
    public GameObject bullet;
    public float speed;
    public float rotationSpeed;
    private DialogueManager dialogueManager;
    private bool inDialogue = false;

    public void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene!");
        }
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");

        // Move the player forward or backward based on the vertical input
        transform.Translate(Vector3.forward * verticalInput * speed * Time.deltaTime, Space.Self);

        float horizontalInput = Input.GetAxis("Horizontal");

        // Rotate the player based on the horizontal input
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        // Press Z to trigger dialog
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.2f, transform.forward, out hit, 1.5f, LayerMask.GetMask("NPC")))
            {
                // Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
                NPC npc = hit.collider.gameObject.GetComponent<NPC>();
                if (npc != null)
                {
                    npc.StartDialogue();
                    inDialogue = true;
                }
            }
            else
            {
                if (inDialogue)
                {
                    dialogueManager.HideDialogue();
                    inDialogue = false;
                }
            }
        }

    }

    public void Fire()
    {
        GameObject newBullet = Instantiate(bullet, gunFront.position, gunFront.rotation);
    }
}
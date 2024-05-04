using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    public Transform gunFront;
    public GameObject bullet;
    public float speed;
    public float rotationSpeed;
    public Animator animator;
    public GameObject myCamera;
    public float dialogueDistance = 5.0f;
    private DialogueManager dialogueManager;
    private bool inDialogue = false;
    private CharacterController characterController;

    public void Start()
    {
        characterController = GetComponent<CharacterController>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene!");
        }
    }

    void Update()
    {

        // Rotate the player based on the camera's forward direction
        Vector3 forward = myCamera.transform.forward;
        forward.y = 0f; // Ignore vertical component to ensure the player stays upright
        forward.Normalize();

        if (forward != Vector3.zero) // Ensure camera is not facing directly downwards
        {
            Quaternion newRotation = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }

        // Get movement input after player rotation
        Vector3 moveDirection = forward * Input.GetAxis("Vertical") + myCamera.transform.right * Input.GetAxis("Horizontal");

        // Move the player based on the rotated direction
        characterController.Move(moveDirection * speed * Time.deltaTime);

        // Set animator parameter for walking
        bool isMoving = moveDirection.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);

        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log(characterController.isGrounded);
            Fire();
        }

        Ray ray = new Ray(myCamera.transform.position, myCamera.transform.forward);
        RaycastHit hit;
        if (!inDialogue && Physics.Raycast(ray, out hit, dialogueDistance, LayerMask.GetMask("NPC")))
        {
            dialogueManager.dialogueHint.SetActive(true);
        }
        else
        {
            dialogueManager.dialogueHint.SetActive(false);
        }

        // Press Z to trigger dialog
        if (Input.GetKeyDown(KeyCode.Z))
        {

            if (Physics.Raycast(ray, out hit, dialogueDistance, LayerMask.GetMask("NPC")))
            {
                // Debug.Log("Raycast has hit the object " + hit.collider.gameObject);

                NPC npc = hit.collider.gameObject.GetComponent<NPC>();
                if (npc != null)
                {
                    dialogueManager.dialogueHint.SetActive(false);
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
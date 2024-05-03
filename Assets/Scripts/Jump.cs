using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    public float jumpHeight = 2.0f; // Jump height
    public float timeToJumpApex = 0.4f; // Time to reach the jump height
    public float accelerationTimeAirborne = 0.2f; // Airborne acceleration time
    public float accelerationTimeGrounded = 0.1f; // Grounded acceleration time

    private float gravity; // Gravity value
    private float jumpVelocity; // Jump velocity
    private float velocityY; // Vertical velocity
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        CalculateJumpValues();
    }

    private void Update()
    {
        if (controller.isGrounded)
        {
            velocityY = 0; // Reset vertical velocity
        }

        // Calculate gravity's effect on vertical velocity
        velocityY += gravity * Time.deltaTime;

        // Apply vertical velocity to the character controller
        Vector3 verticalMovement = Vector3.up * velocityY;
        controller.Move(verticalMovement * Time.deltaTime);

        // Detect character jump input
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocityY = jumpVelocity; // Set jump velocity
        }
    }

    // Calculate gravity and jump velocity based on the set jump height and time to reach height
    private void CalculateJumpValues()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }
}

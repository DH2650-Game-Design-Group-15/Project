using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Animations animations;
    private CharacterController controller;
    private readonly float gravity = -9.81f;
    private readonly float jumpHeight = 0.3f;
    private float speed = 1f;
    Vector3 velocity;
    Vector2 direction;
    float rotationSpeed = 0.05f;
    float rotationSpeedHor = 0.03f;
    public GameObject player;
    public GameObject cam;

    public void OnJump(InputAction.CallbackContext context){
        if (context.started){
            velocity = new(0, jumpHeight * -2f * gravity);
        }
    }

    public void OnRun(InputAction.CallbackContext context){
        if (context.started){
            animations.isRunning(true);
            speed = animations.SpeedRun;
        } else if (context.canceled){
            animations.isRunning(false);
            speed = animations.SpeedWalk;
        }
    }

    public void OnSneak(InputAction.CallbackContext context){
        if (context.started){
            animations.isSneaking(true);
            speed = animations.SpeedSneak;
        } else if (context.canceled){
            animations.isSneaking(false);
            speed = animations.SpeedWalk;
        }
    }

    public void OnMove(InputAction.CallbackContext context){
        animations.isMoving(true);
        direction = context.ReadValue<Vector2>();
        if (context.canceled){
            animations.isMoving(false);
            direction = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context){
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        RotateHorizontal(mouseDelta.x);
        RotateVertical(mouseDelta.y);
    }

    // Method to rotate the horizontal object (yaw)
    private void RotateHorizontal(float mouseDeltaX)
    {
        Vector3 currentRotation = player.transform.rotation.eulerAngles;
        float newRotationY = currentRotation.y + mouseDeltaX * rotationSpeed;
        player.transform.rotation = Quaternion.Euler(currentRotation.x, newRotationY, currentRotation.z);
    }

    // Method to rotate the vertical object (pitch)
    private void RotateVertical(float mouseDeltaY)
    {
        Vector3 currentRotation = cam.transform.rotation.eulerAngles;
        float newRotationX = currentRotation.x - mouseDeltaY * rotationSpeedHor;
        cam.transform.rotation = Quaternion.Euler(newRotationX, currentRotation.y, currentRotation.z);
    }

    void Start(){
        animations = GetComponent<Animations>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update(){
        Vector3 move = - transform.right * direction.y + transform.forward * direction.x;
        controller.Move(move * speed * Time.deltaTime);
        if (!controller.isGrounded){
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}

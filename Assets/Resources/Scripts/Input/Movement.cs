using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Animations animations;
    private CharacterController controller;
    private readonly float gravity = -9.81f;
    private readonly float jumpHeight = 3f;
    private float speed = 1f;
    Vector3 velocity;
    Vector2 direction;

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
        if (context.started){
            animations.isMoving(true);
            direction = context.ReadValue<Vector2>();
        } else if (context.canceled){
            animations.isMoving(false);
            direction = Vector2.zero;
        }

    }

    void Start(){
        animations = GetComponent<Animations>();
        controller = GetComponent<CharacterController>();
    }

    void Update(){
        Vector3 move = transform.right * direction.y - transform.forward * direction.x;
        controller.Move(move * speed * Time.deltaTime);
        if (!controller.isGrounded){
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}

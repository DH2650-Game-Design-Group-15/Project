using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Animations animations;
    private bool isSneaking;
    private bool isRunning;
    private bool isJumping;
    private float endJump = 2f;
    private float jumpDuration;

    public void OnJump(InputAction.CallbackContext context){
        if (context.started){
            jumpDuration = 0;
            isJumping = true;
        }
    }

    public void OnRun(InputAction.CallbackContext context){
        if (context.started){
            isRunning = true;
            animations.isRunning(true);
        } else if (context.canceled){
            isRunning = false;
            animations.isRunning(false);
        }
    }

    public void OnSneak(InputAction.CallbackContext context){
        if (context.started){
            isSneaking = true;
            animations.isSneaking(true);
        } else if (context.canceled){
            isSneaking = false;
            animations.isSneaking(false);
        }
    }

    public void OnMove(InputAction.CallbackContext context){
        if (context.started){
            animations.isMoving(true);
        } else if (context.canceled){
            animations.isMoving(false);
        }

    }

    void Start(){
        animations = GetComponent<Animations>();
    }

    void Update(){
        if (isJumping){
            if (jumpDuration > endJump){
                isJumping = false;
            }
        }
    }
}

using UnityEngine;

public class Animations : MonoBehaviour
{
    public enum AnimationParamter{
        isWalking, 
        WalkingSpeed,
        backwards
    }

    private Animator animator;
    private readonly float speedSneak = 1f;
    private readonly float speedWalk = 2f;
    private readonly float speedRun = 4f;

    public float SpeedSneak => speedSneak;

    public float SpeedWalk => speedWalk;

    public float SpeedRun => speedRun;

    void Start(){
        animator = GetComponent<Animator>();
    }

    public void isMoving(bool value){
        animator.SetBool(AnimationParamter.isWalking.ToString(), value);
    }

    public void makeIdle(){
        animator.SetBool(AnimationParamter.isWalking.ToString(), false);
    }

    public void isSneaking(bool value){
        if (value){
            animator.SetFloat(AnimationParamter.WalkingSpeed.ToString(), SpeedSneak);
        } else {
            animator.SetFloat(AnimationParamter.WalkingSpeed.ToString(), SpeedWalk);
        }
    }

    public void isRunning(bool value){
        if (value){
            animator.SetFloat(AnimationParamter.WalkingSpeed.ToString(), SpeedRun);
        } else {
            animator.SetFloat(AnimationParamter.WalkingSpeed.ToString(), SpeedWalk);
        }
    }

    public void isBackwards(bool value){
        animator.SetBool(AnimationParamter.backwards.ToString(), value);
    }
}

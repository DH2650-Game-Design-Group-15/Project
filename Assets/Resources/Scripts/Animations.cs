using UnityEngine;

public class Animations : MonoBehaviour
{
    public enum AnimationParamter{
        isRunning,
        isWalking, 
        isCrouching,
        backwards
    }

    private Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
    }

    public void isMoving(bool value){
        animator.SetBool(AnimationParamter.isWalking.ToString(), value);
        //Debug.Log("Set moving to " + value);
    }

    public void makeIdle(){
        animator.SetBool(AnimationParamter.isWalking.ToString(), false);
    }

    public void isSneaking(bool value){
        animator.SetBool(AnimationParamter.isCrouching.ToString(), value);
        //Debug.Log("Set sneaking to " + value);
    }

    public void isRunning(bool value){
        animator.SetBool(AnimationParamter.isRunning.ToString(), value);
        //Debug.Log("Set running to " + value);
    }

    public void isBackwards(bool value){
        animator.SetBool(AnimationParamter.backwards.ToString(), value);
        //Debug.Log("Set back to " + value);
    }
}

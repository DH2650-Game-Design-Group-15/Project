using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float turnSpeed = 10.0f; // 控制转向的速度
    public Animator animator;
    private CharacterController characterController;

    private bool isRunning = false; // 初始状态为走路

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

    }

    void Update()
    {
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        // 检测Shift键按下来切换走路和跑步状态
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = !isRunning; // 切换状态
            Debug.Log("IsRunning now: " + isRunning); // 添加这行来检查是否切换

        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isMoving = horizontal != 0 || vertical != 0;
        animator.SetBool("IsWalking", isMoving && !isRunning);
        animator.SetBool("IsRunning", isMoving && isRunning);

        //Vector3 movement = new Vector3(horizontal, 0.0f, vertical).normalized;
        Vector3 movement = new Vector3(horizontal, 0.0f, vertical).normalized;
        if (isMoving)
        {
            // 根据当前状态选择速度
            float speed = isRunning ? runSpeed : walkSpeed;
            //transform.Translate(movement * speed * Time.deltaTime, Space.World);
            characterController.Move(movement * speed * Time.deltaTime);

            // 转向处理
            if (movement != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
            }
        }


    }
}

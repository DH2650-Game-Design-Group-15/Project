using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float turnSpeed = 10.0f; // ����ת����ٶ�
    public Animator animator;
    private CharacterController characterController;

    private bool isRunning = false; // ��ʼ״̬Ϊ��·

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

    }

    void Update()
    {
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        // ���Shift���������л���·���ܲ�״̬
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = !isRunning; // �л�״̬
            Debug.Log("IsRunning now: " + isRunning); // �������������Ƿ��л�

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
            // ���ݵ�ǰ״̬ѡ���ٶ�
            float speed = isRunning ? runSpeed : walkSpeed;
            //transform.Translate(movement * speed * Time.deltaTime, Space.World);
            characterController.Move(movement * speed * Time.deltaTime);

            // ת����
            if (movement != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
            }
        }


    }
}

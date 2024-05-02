using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform mycamera;//存放相机

    public float mousePitchSensitivity;//俯仰的鼠标灵敏度（绕X轴）
    public float mouseYawSensitivity;//偏转的鼠标灵敏度（绕Y轴）

    public float pitchLimit = 75;//俯仰的最大角度

    //将角度限制在（-180，180）
    public float ClampAngle(float angle)
    {
        if (angle <= 180 && angle >= -180)
        {
            return angle;
        }

        while (angle > 180)
        {
            angle -= 360;
        }

        while (angle < -180)
        {
            angle += 360;
        }

        return angle;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//将鼠标光标锁定在窗口中心
        Cursor.visible = false;//隐藏鼠标光标
    }

    private void Update()
    {
        float pitch = -Input.GetAxis("Mouse Y") * mousePitchSensitivity;//当前帧的俯仰值（变化的量）
        float yaw = Input.GetAxis("Mouse X") * mouseYawSensitivity;//当前帧的偏转值（变化的量）

        Vector3 rot = new Vector3(pitch, yaw, 0);//当前帧欧拉角发生的改变量

        Vector3 afterRot;//改变后的欧拉角
        afterRot.x = Mathf.Clamp(ClampAngle(mycamera.localEulerAngles.x) + rot.x, -pitchLimit, pitchLimit);
        afterRot.y = mycamera.transform.localEulerAngles.y + rot.y;
        afterRot.z = 0;
        mycamera.localEulerAngles = afterRot;//将最终的欧拉角赋给摄像机
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform mycamera;//������

    public float mousePitchSensitivity;//��������������ȣ���X�ᣩ
    public float mouseYawSensitivity;//ƫת����������ȣ���Y�ᣩ

    public float pitchLimit = 75;//���������Ƕ�

    //���Ƕ������ڣ�-180��180��
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
        Cursor.lockState = CursorLockMode.Locked;//������������ڴ�������
        Cursor.visible = false;//���������
    }

    private void Update()
    {
        float pitch = -Input.GetAxis("Mouse Y") * mousePitchSensitivity;//��ǰ֡�ĸ���ֵ���仯������
        float yaw = Input.GetAxis("Mouse X") * mouseYawSensitivity;//��ǰ֡��ƫתֵ���仯������

        Vector3 rot = new Vector3(pitch, yaw, 0);//��ǰ֡ŷ���Ƿ����ĸı���

        Vector3 afterRot;//�ı���ŷ����
        afterRot.x = Mathf.Clamp(ClampAngle(mycamera.localEulerAngles.x) + rot.x, -pitchLimit, pitchLimit);
        afterRot.y = mycamera.transform.localEulerAngles.y + rot.y;
        afterRot.z = 0;
        mycamera.localEulerAngles = afterRot;//�����յ�ŷ���Ǹ��������
    }
}

using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera playerCamera;

    [SerializeField]
    private CinemachineVirtualCamera bossCamera;

    private int playerCameraOldPriority;
    private int bossCameraOldPriority;

    private void Start()
    {
        
    }
}

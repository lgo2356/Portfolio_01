using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class BossCinematicEventTrigger : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera playerCamera;

    [SerializeField]
    private CinemachineVirtualCamera bossCamera;

    [SerializeField]
    private GameObject bossObject;
    
    private void OnTriggerEnter(Collider other)
    {
        print("BossCinematicEventTrigger");

        StartCoroutine(Coroutine_Camera());
    }

    private IEnumerator Coroutine_Camera()
    {
        // To Boss
        bossCamera.Priority = 99;
        playerCamera.Priority = 1;

        yield return new WaitForSeconds(0.5f);

        Boss boss = bossObject.GetComponent<Boss>();
        boss.SetTaunt();

        yield return new WaitForSeconds(4.5f);
        
        // To Player
        playerCamera.Priority = 99;
        bossCamera.Priority = 1;
    }
}

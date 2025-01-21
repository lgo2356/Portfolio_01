using Cinemachine;
using System.Collections;
using UnityEngine;

public class BossCinematicEventTrigger : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera playerCamera;

    [SerializeField]
    private CinemachineVirtualCamera bossCamera;

    [SerializeField]
    private GameObject bossObject;

    [SerializeField]
    private GameObject cinematicObjects;

    private new Collider collider;
    private Boss boss;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void Start()
    {
        boss = bossObject.GetComponent<Boss>();
        boss.OnTauntAction += () =>
        {
            cinematicObjects.SetActive(false);
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        collider.enabled = false;

        CustomGameManager.IsPlayerInput = false;
        
        StartCoroutine(Coroutine_Camera());
    }

    private IEnumerator Coroutine_Camera()
    {
        // To Boss
        bossCamera.Priority = 99;
        playerCamera.Priority = 1;

        yield return new WaitForSeconds(0.5f);

        boss.SetTaunt();

        yield return new WaitForSeconds(4.5f);
        
        // To Player
        playerCamera.Priority = 99;
        bossCamera.Priority = 1;

        bossObject.GetComponent<PerceptionComponent>().enabled = true;

        CustomGameManager.IsPlayerInput = true;
        
        Destroy(gameObject);
    }
}

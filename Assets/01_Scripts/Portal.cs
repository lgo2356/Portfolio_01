using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private RectTransform uiGameClearObject;
    
    private new Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == false)
            return;

        UI_GameClearPanel uiGameClearPanel = uiGameClearObject.GetComponent<UI_GameClearPanel>();
        uiGameClearPanel.gameObject.SetActive(true);
        uiGameClearPanel.Show();
    }
}

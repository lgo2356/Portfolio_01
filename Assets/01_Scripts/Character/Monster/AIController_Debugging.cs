using TMPro;
using UnityEngine;

public partial class AIController
{
    [Header("[ 디버깅 ]")]
    [SerializeField]
    private bool isDebugging = true;

    [SerializeField]
    private string uiAIStatePrefabName = "UI_MonsterAIState";

    [SerializeField]
    private GameObject uiAIStatePrefab;

    private Canvas uiAIStateCanvas;
    private TextMeshProUGUI uiAIStateText;

    private void Start_InitAIStateCanvas()
    {
        if (isDebugging == false)
            return;

        if (uiAIStatePrefab != null)
        {
            GameObject go = Instantiate(uiAIStatePrefab, transform);
            {
                uiAIStateCanvas = go.GetComponent<Canvas>();
                uiAIStateText = go.GetComponentInChildren<TextMeshProUGUI>();
            }
        }
        else
        {
            GameObject resource = Resources.Load<GameObject>(uiAIStatePrefabName);
            GameObject go = Instantiate(resource, transform);
            {
                uiAIStateCanvas = go.GetComponent<Canvas>();
                uiAIStateText = go.GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }

    private void Update_UpdateAIStateText()
    {
        if (uiAIStateText == null)
            return;

        uiAIStateText.text = $"{stateComponent.CurrentState.ToString()} (0.00)";
    }

    private void LateUpdate_Billboard()
    {
        if (uiAIStateCanvas == null)
            return;

        uiAIStateCanvas.transform.rotation = Camera.main.transform.rotation;
    }
}

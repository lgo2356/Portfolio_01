using UnityEditor;
using UnityEngine;

public class MonsterHpComponent : HpComponent
{
    [SerializeField]
    private GameObject uiHpBarPrefab;

    private UI_MonsterHpBar uiHpBar;

    protected override void Awake()
    {
        base.Awake();

        Awake_InstantiatePrefab();
    }

    private void Awake_InstantiatePrefab()
    {
        Debug.Assert(uiHpBarPrefab != null, "UI_MonsterHpBar prefab is NULL");
        GameObject hpBarObj = Instantiate<GameObject>(uiHpBarPrefab, transform);

        Debug.Assert(hpBarObj != null, "Instantiating of UI_MonsterHpBar prefab is failed");
        uiHpBar = hpBarObj.GetComponent<UI_MonsterHpBar>();
    }

    protected override void Start()
    {
        base.Start();

        uiHpBar.SetMaxHpAmount(maxHP);
        uiHpBar.SetCurrentHpAmount(currentHP);
    }

    public override void AddDamage(float amount)
    {
        base.AddDamage(amount);

        uiHpBar.SetCurrentHpAmount(currentHP);
    }

    public override void AddHp(float amount)
    {
        base.AddHp(amount);

        uiHpBar.SetCurrentHpAmount(currentHP);
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MonsterHpComponent))]
    public class HpComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MonsterHpComponent hpComponent = (MonsterHpComponent)target;

            if (GUILayout.Button("Hp 10 Up"))
            {
                hpComponent.AddHp(10f);

                Debug.Log($"현재 체력 : {hpComponent.currentHP}");
            }

            if (GUILayout.Button("Hp 10 Down"))
            {
                hpComponent.AddDamage(10f);

                Debug.Log($"현재 체력 : {hpComponent.currentHP}");
            }
        }
    }
#endif
}

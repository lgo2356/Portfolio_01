using UnityEditor;
using UnityEngine;

public class PlayerHpComponent : HpComponent
{
    [SerializeField]
    private UI_PlayerHpBar uiHpBar;

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
    [CustomEditor(typeof(PlayerHpComponent))]
    public class HpComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PlayerHpComponent hpComponent = (PlayerHpComponent)target;

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

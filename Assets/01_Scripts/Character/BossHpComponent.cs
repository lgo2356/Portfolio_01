using UnityEngine;

public class BossHpComponent : HpComponent
{
    [SerializeField]
    private UI_BossHpBar uiHpBar;

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

    public void ShowHpBar()
    {
        uiHpBar.ShowHpBar();
    }
}

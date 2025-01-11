using UnityEditor;
using UnityEngine;

public class HpComponent : MonoBehaviour
{
    [SerializeField]
    protected float maxHP = 100f;

    protected float currentHP;

    public bool IsDead => currentHP <= 0f;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        currentHP = maxHP;
    }

    public virtual void AddDamage(float amount)
    {
        if (amount < 1f)
            return;

        currentHP += (amount * -1f);
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);
    }

    public virtual void AddHp(float amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);
    }
}

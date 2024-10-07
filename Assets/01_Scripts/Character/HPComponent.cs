using UnityEngine;

public class HPComponent : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 100f;

    private float currentHP;

    public bool IsDead => currentHP <= 0f;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void AddDamage(float amount)
    {
        if (amount < 1f)
            return;

        currentHP += (amount * -1f);
        currentHP = Mathf.Clamp(currentHP, 0f, maxHP);

        Debug.Log($"{gameObject.name} - Ã¼·Â : {currentHP} / {maxHP}");
    }
}

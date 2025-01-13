using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHpBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHpAmount(float amount)
    {
        slider.maxValue = (int)amount;
    }

    public void SetCurrentHpAmount(float amount)
    {
        slider.value = (int)amount;
    }
}

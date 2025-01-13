using UnityEngine;
using UnityEngine.UI;

public class UI_BossHpBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = transform.GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetMaxHpAmount(float amount)
    {
        slider.maxValue = (int)amount;
    }

    public void SetCurrentHpAmount(float amount)
    {
        slider.value = (int)amount;
    }

    public void ShowHpBar()
    {
        gameObject.SetActive(true);
    }
}

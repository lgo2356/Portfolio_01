using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterHpBar : MonoBehaviour
{
    private Slider slider;

    private float hpBarHideTimer;

    private void Awake()
    {
        slider = transform.GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Update_HpBarBillboard();
        Update_HpBarHideTimer();
    }

    private void Update_HpBarBillboard()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    private void Update_HpBarHideTimer()
    {
        if (hpBarHideTimer > 0.0f)
        {
            hpBarHideTimer -= Time.deltaTime;
        }

        if (hpBarHideTimer <= 0.0f && gameObject.activeSelf)
        {
            hpBarHideTimer = 0.0f;

            gameObject.SetActive(false);
        }
    }

    public void SetMaxHpAmount(float amount)
    {
        slider.maxValue = (int)amount;

        ShowHpBar();
    }

    public void SetCurrentHpAmount(float amount)
    {
        slider.value = (int)amount;

        ShowHpBar();
    }

    private void ShowHpBar()
    {
        hpBarHideTimer = 5.0f;

        gameObject.SetActive(true);
    }
}

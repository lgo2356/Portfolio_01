using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameClearPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI uiText;
    
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        Color imageColor = image.color;
        Color textColor = uiText.color;

        imageColor.a = 0.0f;
        textColor.a = 0.0f;

        image.color = imageColor;
        uiText.color = textColor;
        
        Show();
    }

    public void Show()
    {
        StartCoroutine(Coroutine_ChangeColor(0.5f, 1.0f));
    }

    private IEnumerator Coroutine_ChangeColor(float duration, float textDuration)
    {
        float time = 0.0f;
        Color color = image.color;
        
        while (time < duration)
        {            
            color.a = Mathf.Lerp(0f, 1.0f, time / duration);
            image.color = color;
            
            time += Time.deltaTime;
            
            yield return null;
        }

        color.a = 1.0f;
        image.color = color;

        time = 0.0f;
        color = uiText.color;

        while (time < textDuration)
        {
            color.a = Mathf.Lerp(0.0f, 1.0f, time / textDuration);
            uiText.color = color;

            time += Time.deltaTime;

            yield return null;
        }

        color.a = 1.0f;
        uiText.color = color;
    }
}

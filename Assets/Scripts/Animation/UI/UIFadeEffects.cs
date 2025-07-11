using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIFadeEffects : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private CanvasGroup canvasGroup;
    
    void Start()
    {
        // Сделать изображение прозрачным за 1 секунду
        if (image != null)
            image.DOFade(0f, 1f);
        
        // Сделать группу полупрозрачной за 1 секунду
        if (canvasGroup != null)
            canvasGroup.DOFade(0.5f, 1f);
    }
}
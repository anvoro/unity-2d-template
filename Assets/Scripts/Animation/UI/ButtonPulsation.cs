using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonPulsation : MonoBehaviour
{
    [SerializeField] private Button button;
    private Tween pulseTween;
    
    void Start()
    {
        if (button != null)
        {
            // Бесконечная пульсация кнопки
            pulseTween = button.transform.DOScale(1.2f, 0.3f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
    
    void OnDestroy()
    {
        // Важно убивать твины при уничтожении объекта
        pulseTween?.Kill();
    }
}
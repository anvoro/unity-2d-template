using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Animation.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonOnClickAnimation : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float clickScale = 1.1f; // Уменьшил с 1.2f
        [SerializeField] private float animationDuration = 0.3f; // Уменьшил с 1f
        [SerializeField] private Ease scaleUpEase = Ease.OutQuad; // Плавное увеличение
        [SerializeField] private Ease scaleDownEase = Ease.InOutQuad; // Плавное уменьшение
        
        [Header("Advanced Settings")]
        [SerializeField] private bool preventSpamClick = true; // Предотвращение спам-кликов
        
        private Button button;
        private Vector3 originalScale;
        private Sequence currentSequence;
        private bool isAnimating = false;
        
        void Start()
        {
            originalScale = transform.localScale;
            button = GetComponent<Button>();
            
            button.onClick.AddListener(AnimateClick);
        }
        
        void AnimateClick()
        {
            // Предотвращаем спам-клики если нужно
            if (preventSpamClick && isAnimating) return;
            
            // Плавно завершаем предыдущую анимацию
            if (currentSequence != null && currentSequence.IsActive())
            {
                currentSequence.Complete();
            }
            
            isAnimating = true;
            
            // Создаем новую последовательность
            currentSequence = DOTween.Sequence();
            
            currentSequence
                // Плавное увеличение
                .Append(transform.DOScale(originalScale * clickScale, animationDuration * 0.4f)
                    .SetEase(scaleUpEase))
                // Плавное возвращение с небольшим overshoot
                .Append(transform.DOScale(originalScale, animationDuration * 0.6f)
                    .SetEase(scaleDownEase))
                // Callback по завершению
                .OnComplete(() => {
                    isAnimating = false;
                    transform.localScale = originalScale; // Гарантируем точный возврат
                })
                // На случай прерывания
                .OnKill(() => {
                    isAnimating = false;
                    transform.localScale = originalScale;
                });
        }
        
        void OnDestroy()
        {
            currentSequence?.Kill();
            button.onClick.RemoveListener(AnimateClick);
        }
    }
}
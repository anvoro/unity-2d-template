using UnityEngine;
using DG.Tweening;

public class PunchEffects : MonoBehaviour
{
    [Header("Punch Settings")]
    [SerializeField] private float punchDuration = 0.5f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float elasticity = 1f;
    
    // Сохраняем оригинальные значения
    private Vector3 originalPosition;
    private Vector3 originalScale;
    
    // Ссылки на текущие твины
    private Tween positionTween;
    private Tween scaleTween;
    
    void Start()
    {
        // Запоминаем начальные значения
        originalPosition = transform.position;
        originalScale = transform.localScale;
    }
    
    void Update()
    {
        // Punch позиция по нажатию Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PunchPosition();
        }
        
        // Punch масштаб по нажатию W
        if (Input.GetKeyDown(KeyCode.W))
        {
            PunchScale();
        }
    }
    
    void PunchPosition()
    {
        // Убиваем предыдущую анимацию и возвращаем к оригиналу
        positionTween?.Kill();
        transform.position = originalPosition;
        
        // Запускаем новую анимацию
        positionTween = transform.DOPunchPosition(Vector3.up * 2, punchDuration, vibrato, elasticity)
            .OnComplete(() => {
                // Гарантируем возврат к оригинальной позиции
                transform.position = originalPosition;
            });
    }
    
    void PunchScale()
    {
        // Убиваем предыдущую анимацию и возвращаем к оригиналу
        scaleTween?.Kill();
        transform.localScale = originalScale;
        
        // Запускаем новую анимацию
        scaleTween = transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, vibrato, elasticity)
            .OnComplete(() => {
                // Гарантируем возврат к оригинальному масштабу
                transform.localScale = originalScale;
            });
    }
    
    void OnDestroy()
    {
        // Очищаем твины при уничтожении объекта
        positionTween?.Kill();
        scaleTween?.Kill();
    }
}
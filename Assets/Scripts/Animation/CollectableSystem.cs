using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CollectableSystem : MonoBehaviour
{
    [Header("Collection Settings")]
    [SerializeField] private Transform collectTarget; // UI элемент счетчика
    [SerializeField] private float collectDuration = 0.5f;
    
    private int score = 0;
    
    void Start()
    {
        // Находим все коллектаблы и добавляем им компонент
        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Collectable");
        foreach (var item in collectables)
        {
            item.AddComponent<CollectableItem>().Initialize(this);
        }
    }
    
    public void CollectItem(Transform item)
    {
        // Отключаем коллайдер чтобы избежать повторного сбора
        item.GetComponent<Collider2D>().enabled = false;
        
        // Создаем последовательность анимаций
        Sequence collectSequence = DOTween.Sequence();
        
        collectSequence
            // Подпрыгивание
            .Append(item.DOLocalMoveY(item.position.y + 1f, 0.2f).SetEase(Ease.OutQuad))
            // Вращение
            .Join(item.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360))
            // Уменьшение
            .Join(item.DOScale(0.5f, 0.5f))
            // Полет к счетчику
            .Append(item.DOMove(collectTarget.position, collectDuration).SetEase(Ease.OutQuad))
            // Финальное исчезновение
            .Join(item.DOScale(0f, 0.2f).SetDelay(collectDuration - 0.2f))
            .OnComplete(() => {
                Destroy(item.gameObject);
                UpdateScore();
                AnimateScoreUI();
            });
    }
    
    void UpdateScore()
    {
        score++;
        Debug.Log($"Score: {score}");
    }
    
    void AnimateScoreUI()
    {
        // Анимация счетчика при получении очков
        collectTarget.DOPunchScale(Vector3.one * 0.3f, 0.3f, 10, 1);
    }
}

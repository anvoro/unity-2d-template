using UnityEngine;
using DG.Tweening;

public class SequenceExample : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    private Sequence mySequence;
    
    void Start()
    {
        CreateSequence();
    }
    
    void CreateSequence()
    {
        // Создаем последовательность анимаций
        mySequence = DOTween.Sequence();
        
        mySequence
            // Сначала двигаем по X
            .Append(targetObject.transform.DOMoveX(5, 1))
            // Затем двигаем по Y
            .Append(targetObject.transform.DOMoveY(3, 0.5f))
            // Одновременно с движением по Y масштабируем
            .Join(targetObject.transform.DOScale(2, 0.5f))
            // Ждем 0.5 секунды
            .AppendInterval(0.5f)
            // Поворачиваем
            .Append(targetObject.transform.DORotate(new Vector3(0, 180, 0), 1))
            // Добавляем callback в конце
            .OnComplete(() => Debug.Log("Sequence completed!"));
    }
    
    void OnDestroy()
    {
        mySequence?.Kill();
    }
}
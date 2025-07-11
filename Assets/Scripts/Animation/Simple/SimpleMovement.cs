using UnityEngine;
using DG.Tweening;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] private Vector2 moveVector = new  Vector2(5f, 0f);
    [SerializeField] private float duration = 2f;
    
    void Start()
    {
        // Переместить объект в позицию [moveVector] за [duration] секунд
        transform.DOMove(moveVector, duration);
    }
}
using UnityEngine;
using DG.Tweening;

public class SimpleScale : MonoBehaviour
{
    [SerializeField] private Vector2 scale = new Vector2(2, 2);
    [SerializeField] private float duration = 1f;
    
    void Start()
    {
        transform.DOScale(scale, duration);
    }
}
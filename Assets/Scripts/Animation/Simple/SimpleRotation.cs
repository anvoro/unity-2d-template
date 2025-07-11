using UnityEngine;
using DG.Tweening;

public class SimpleRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotation = new Vector3(0, 0, 360);
    [SerializeField] private float duration = 2f;
    
    void Start()
    {
        transform.DORotate(rotation, duration, RotateMode.FastBeyond360);
    }
}
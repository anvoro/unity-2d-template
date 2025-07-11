using System;
using DG.Tweening;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private CollectableSystem collectSystem;
    private Tween idleTween;

    public void Initialize(CollectableSystem system)
    {
        collectSystem = system;

        // Idle анимация (покачивание)
        idleTween = transform.DORotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    private void OnMouseDown()
    {
        idleTween?.Kill();
        collectSystem.CollectItem(transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            idleTween?.Kill();
            collectSystem.CollectItem(transform);
        }
    }

    void OnDestroy()
    {
        idleTween?.Kill();
    }
}
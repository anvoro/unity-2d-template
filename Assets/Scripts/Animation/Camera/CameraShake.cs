using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float strength = 1f;
    [SerializeField] private int vibrato = 10;
    [SerializeField] private float randomness = 90f;
    
    private Camera mainCamera;
    private Vector3 originalPosition;
    
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
            originalPosition = mainCamera.transform.position;
    }
    
    void Update()
    {
        // Shake по нажатию Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShakeCamera();
        }
    }
    
    public void ShakeCamera()
    {
        if (mainCamera != null)
        {
            mainCamera.DOShakePosition(duration, strength, vibrato, randomness)
                .OnComplete(() => mainCamera.transform.position = originalPosition);
        }
    }
}
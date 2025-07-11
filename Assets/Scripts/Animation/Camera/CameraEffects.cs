using UnityEngine;
using DG.Tweening;

public class CameraEffects : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform player;
    
    [Header("Perspective Camera Settings")]
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float zoomFOV = 30f;
    
    [Header("Orthographic Camera Settings")]
    [SerializeField] private float normalOrthoSize = 5f;
    [SerializeField] private float zoomOrthoSize = 3f;
    
    [Header("Zoom Settings")]
    [SerializeField] private float zoomDuration = 0.5f;
    [SerializeField] private Ease zoomEase = Ease.OutQuad;
    
    private Tween currentCameraTween;
    private bool isZoomed = false;
    private bool isOrthographic;
    
    // Сохраняем начальные значения
    private float initialFOV;
    private float initialOrthoSize;
    
    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        // Определяем тип камеры и сохраняем начальные значения
        isOrthographic = mainCamera.orthographic;
        
        if (isOrthographic)
        {
            initialOrthoSize = mainCamera.orthographicSize;
            // Если не заданы значения в инспекторе, используем текущие
            if (normalOrthoSize == 5f && zoomOrthoSize == 3f)
            {
                normalOrthoSize = initialOrthoSize;
                zoomOrthoSize = initialOrthoSize * 0.6f; // 60% от нормального размера
            }
        }
        else
        {
            initialFOV = mainCamera.fieldOfView;
            if (normalFOV == 60f && zoomFOV == 30f)
            {
                normalFOV = initialFOV;
                zoomFOV = initialFOV * 0.5f; // 50% от нормального FOV
            }
        }
    }
    
    void LateUpdate()
    {
        // Zoom controls
        if (Input.GetMouseButtonDown(1)) // Right click
        {
            ZoomIn();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ZoomOut();
        }
        
        // Дополнительный контроль через колесико мыши
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0)
        {
            SmoothZoom(scrollDelta);
        }
    }
    
    public void ZoomIn()
    {
        if (!isZoomed)
        {
            isZoomed = true;
            currentCameraTween?.Kill();
            
            if (isOrthographic)
            {
                currentCameraTween = mainCamera.DOOrthoSize(zoomOrthoSize, zoomDuration)
                    .SetEase(zoomEase);
            }
            else
            {
                currentCameraTween = mainCamera.DOFieldOfView(zoomFOV, zoomDuration)
                    .SetEase(zoomEase);
            }
        }
    }
    
    public void ZoomOut()
    {
        if (isZoomed)
        {
            isZoomed = false;
            currentCameraTween?.Kill();
            
            if (isOrthographic)
            {
                currentCameraTween = mainCamera.DOOrthoSize(normalOrthoSize, zoomDuration)
                    .SetEase(zoomEase);
            }
            else
            {
                currentCameraTween = mainCamera.DOFieldOfView(normalFOV, zoomDuration)
                    .SetEase(zoomEase);
            }
        }
    }
    
    // Плавный зум колесиком мыши
    private void SmoothZoom(float delta)
    {
        currentCameraTween?.Kill();
        
        if (isOrthographic)
        {
            float targetSize = mainCamera.orthographicSize - delta * 2f;
            targetSize = Mathf.Clamp(targetSize, zoomOrthoSize * 0.5f, normalOrthoSize * 1.5f);
            
            currentCameraTween = mainCamera.DOOrthoSize(targetSize, 0.2f)
                .SetEase(Ease.OutQuad);
        }
        else
        {
            float targetFOV = mainCamera.fieldOfView - delta * 20f;
            targetFOV = Mathf.Clamp(targetFOV, zoomFOV * 0.5f, normalFOV * 1.5f);
            
            currentCameraTween = mainCamera.DOFieldOfView(targetFOV, 0.2f)
                .SetEase(Ease.OutQuad);
        }
    }
    
    // Утилитарные методы для установки конкретных значений зума
    public void SetZoomLevel(float zoomPercent)
    {
        zoomPercent = Mathf.Clamp01(zoomPercent);
        currentCameraTween?.Kill();
        
        if (isOrthographic)
        {
            float targetSize = Mathf.Lerp(normalOrthoSize, zoomOrthoSize, zoomPercent);
            currentCameraTween = mainCamera.DOOrthoSize(targetSize, zoomDuration)
                .SetEase(zoomEase);
        }
        else
        {
            float targetFOV = Mathf.Lerp(normalFOV, zoomFOV, zoomPercent);
            currentCameraTween = mainCamera.DOFieldOfView(targetFOV, zoomDuration)
                .SetEase(zoomEase);
        }
        
        isZoomed = zoomPercent > 0.5f;
    }
    
    void OnDestroy()
    {
        currentCameraTween?.Kill();
    }
    
    // Для отладки в редакторе
    void OnValidate()
    {
        // Проверяем корректность значений
        if (normalOrthoSize <= 0) normalOrthoSize = 5f;
        if (zoomOrthoSize <= 0) zoomOrthoSize = 3f;
        if (zoomOrthoSize >= normalOrthoSize) zoomOrthoSize = normalOrthoSize * 0.6f;
        
        if (normalFOV <= 0) normalFOV = 60f;
        if (zoomFOV <= 0) zoomFOV = 30f;
        if (zoomFOV >= normalFOV) zoomFOV = normalFOV * 0.5f;
    }
}
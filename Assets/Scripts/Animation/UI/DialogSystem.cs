using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    [Header("Dialog Elements")]
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Image characterPortrait;
    [SerializeField] private float textSpeed = 0.05f;
    
    private Tween textTween;
    
    void Start()
    {
        // Скрываем диалог в начале
        dialogBox.SetActive(false);
    }
    
    public void ShowDialog(string text, Sprite portrait = null)
    {
        // Останавливаем предыдущую анимацию текста
        textTween?.Kill();
        
        // Настраиваем элементы
        dialogText.text = "";
        if (portrait != null)
        {
            characterPortrait.sprite = portrait;
            characterPortrait.color = new Color(1, 1, 1, 0);
        }
        
        // Показываем диалоговое окно
        dialogBox.transform.localScale = Vector3.zero;
        dialogBox.SetActive(true);
        
        // Анимация появления
        Sequence showSequence = DOTween.Sequence();
        showSequence
            .Append(dialogBox.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack))
            .Join(characterPortrait.DOFade(1f, 0.3f))
            .AppendCallback(() => {
                // Анимация печатания текста
                AnimateTextV3(text, text.Length * textSpeed);

                // textTween = dialogText.DOText(text, text.Length * textSpeed)
                // .SetEase(Ease.Linear);
            });
    }
    
    // Вариант 1: Через DOTween.To
    public void AnimateTextV1(string fullText, float duration)
    {
        dialogText.text = "";
        
        DOTween.To(() => 0, 
            x => dialogText.text = fullText.Substring(0, x), 
            fullText.Length, 
            duration);
    }
    
    // Вариант 3: Кастомный твинер
    public void AnimateTextV3(string fullText, float duration)
    {
        int charCount = 0;
        dialogText.text = "";
        
        DOTween.To(() => charCount, x => {
                charCount = x;
                dialogText.text = fullText.Substring(0, Mathf.Min(charCount, fullText.Length));
            }, fullText.Length, duration)
            .SetEase(Ease.Linear);
    }
    
    public void HideDialog()
    {
        textTween?.Kill();
        
        Sequence hideSequence = DOTween.Sequence();
        hideSequence
            .Append(dialogBox.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack))
            .OnComplete(() => dialogBox.SetActive(false));
    }
    
    // Пример использования
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ShowDialog("Привет! Это пример анимированного диалога с DOTween.");
        }
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            HideDialog();
        }
    }
}
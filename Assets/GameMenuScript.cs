using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuScript : MonoBehaviour
{
    [SerializeField] private Button ExitToMainMenuButton;
    
    void Start()
    {
        ExitToMainMenuButton.onClick.AddListener(ExitToMainMenu);
    }

    void ExitToMainMenu()
    {
        GameManager.Instance.LoadScene("MainMenu");
    }
}

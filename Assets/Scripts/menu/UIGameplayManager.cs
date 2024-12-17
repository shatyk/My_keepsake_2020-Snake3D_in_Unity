using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplayManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private GameObject pausePanel;
    
    [Header("Gameplay panel")]
    [SerializeField] private Text textCoins;

    [Header("Gameover panel")]
    [SerializeField] private Text textResultScore;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextScore(int score)
    {
        textCoins.text = $"{score}"; 
    }

    #region openClose

    public void OpenPausePanel()
    {
        gameplayPanel.SetActive(false);
        pausePanel.SetActive(true);
    }
    public void ClosePausePanel()
    {
        gameplayPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void OpenGameoverPanel(int score)
    {
        gameplayPanel.SetActive(false);
        gameoverPanel.SetActive(true);
        textResultScore.text = $"SCORE: {score}";
    }
    public void CloseGameoverPanel(bool isRetry)
    {
        
    }

    #endregion
}

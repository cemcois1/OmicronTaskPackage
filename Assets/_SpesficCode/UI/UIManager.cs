using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _SpesficCode.UI
{
    [DefaultExecutionOrder(-100)]
    public class UIManager : MonoBehaviour
    {
        #region Singleton

        public static UIManager Instance;
        private void Awake()
        { 
            Instance = this;
        }

        #endregion


        #region Win-Fail-Gameplay-Tutorial Canvas Group

        [SerializeField] private CanvasGroup LevelWinCanvas;
        [SerializeField] private CanvasGroup LevelFailCanvas;
        
        [SerializeField] private CanvasGroup gameplayCanvas;

        [SerializeField] private CanvasGroup tutorialCanvas;
        [SerializeField] private TextMeshProUGUI levelShowText;

        public void ShowGameplayCanvas()
        {
            gameplayCanvas.OpenPanel();
        }
        public void HideGameplayCanvas()
        {
            gameplayCanvas.ClosePanel();
        }
        
        public void ShowTutorialCanvas()
        {
            tutorialCanvas.OpenPanel();
        }
        /// <summary>
        /// Method Triggered in Editor
        /// </summary>
        public void HideTutorialCanvas()
        {
            tutorialCanvas.ClosePanel();
        }
        public void ShowLevelWinCanvas()
        {
            HideGameplayCanvas();
            LevelWinCanvas.OpenPanel();
           
        }
        public void HideLevelWinCanvas()
        {
            LevelWinCanvas.ClosePanel();
        }
        public void ShowLevelFailCanvas()
        {
            HideGameplayCanvas();
            LevelFailCanvas.OpenPanel();
        }
        public void HideLevelFailCanvas()
        {
            LevelFailCanvas.ClosePanel();
        }

        #endregion

        #region Progressbar

        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI nextLevelText;
        
        [SerializeField] private Image progressBar;
        [SerializeField] private float fullyFillDuration=1f;
        [SerializeField] private TextMeshProUGUI percentageText;
        
        public void UpdateProgressbar(float fillrate)
        {
            progressBar.DOKill(false);
            var fillDuration = StaticMethods.GetLerpedValue(0, 1, (fillrate - progressBar.fillAmount), 0, fullyFillDuration);
            progressBar.DOFillAmount(fillrate, fillDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (progressBar.fillAmount >= 1&&!GameManager.instance.LevelFinished)
                {
                    GameManager.levelWined?.Invoke();
                }
            });

            percentageText.text = "%"+(fillrate * 100).ToString("F0");
            
        }

        #endregion

        private void OnEnable()
        {
            GameManager.levelWined += ShowLevelWinCanvas;
            GameManager.levelFailed += ShowLevelFailCanvas;
            GameManager.LevelStarted += ShowTutorialCanvas;
            GameManager.PrepareLevel += UpdateLeveltexts;
        }

        private void OnDisable()
        {
            GameManager.levelWined -= ShowLevelWinCanvas;
            GameManager.levelFailed -= ShowLevelFailCanvas;
            GameManager.LevelStarted -= ShowTutorialCanvas;
            GameManager.PrepareLevel -= UpdateLeveltexts;

        }

        private void UpdateLeveltexts(int currentLevel)
        {
            percentageText.text = "%0";
            progressBar.fillAmount = 0;
            levelShowText.text = "Level " + (currentLevel+1);
            currentLevelText.text = (currentLevel + 1).ToString();
            nextLevelText.text = (currentLevel + 2).ToString();
            
        }
    }
}
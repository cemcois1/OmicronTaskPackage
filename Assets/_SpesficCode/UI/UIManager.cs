using UnityEngine;

namespace _SpesficCode.UI
{
    public class UIManager : MonoBehaviour
    {
        #region Singleton

        public static UIManager Instance;
        private void Awake()
        { 
            Instance = this;
        }

        #endregion
        
        
        [SerializeField] private CanvasGroup LevelWinCanvas;
        [SerializeField] private CanvasGroup LevelFailCanvas;
        [SerializeField] private CanvasGroup tutorilaCanvas;
        
        public void ShowTutorialCanvas()
        {
            tutorilaCanvas.OpenPanel();
        }
        public void ShowLevelWinCanvas()
        {
            LevelWinCanvas.OpenPanel();
           
        }
        public void HideLevelWinCanvas()
        {
            LevelWinCanvas.ClosePanel();
        }
        public void ShowLevelFailCanvas()
        {
            LevelFailCanvas.OpenPanel();
        }
        public void HideLevelFailCanvas()
        {
            LevelFailCanvas.ClosePanel();
        }
        
    }
}
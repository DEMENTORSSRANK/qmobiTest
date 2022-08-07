using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.UserInterface
{
    public class MenuScreen : MonoBehaviour
    {
        [SerializeField] private Button _complete;

        [SerializeField] private Button _newGame;

        [SerializeField] private Button _changeControl;

        [SerializeField] private Button _exit;

        [SerializeField] private ToggleView _changeControlView;
        
        public event Action CompleteClicked;

        public event Action NewGameClicked;

        public event Action<int> ControlChangeClicked; 

        public void OnControlIndexChanged(int index)
        {
            _changeControlView.IndexChanged(index);
        }
        
        public void OnGameActiveChanged(bool isActive)
        {
            _complete.interactable = isActive;
        }

        private void OnCompleteClicked()
        {
            gameObject.SetActive(false);
            
            CompleteClicked?.Invoke();
        }

        private void OnNewGameClicked()
        {
            gameObject.SetActive(false);
            
            NewGameClicked?.Invoke();
        }

        private void OnChangeControlClicked()
        {
            ControlChangeClicked?.Invoke(_changeControlView.GetOppositeLastIndex);
        }

        private void OnExitClicked()
        {
            Application.Quit();
        }

        private void Start()
        {
            _complete.onClick.AddListener(OnCompleteClicked);

            _newGame.onClick.AddListener(OnNewGameClicked);

            _changeControl.onClick.AddListener(OnChangeControlClicked);

            _exit.onClick.AddListener(OnExitClicked);
        }
    }
}
using System.Threading.Tasks;
using Sources.Control;
using Sources.GameActive;
using Sources.Heal;
using Sources.Player;
using UnityEngine;

namespace Sources.UserInterface
{
    public class UiRouter : MonoBehaviour
    {
        [SerializeField] private GameScreen _gameScreen;

        [SerializeField] private MenuScreen _menuScreen;

        [SerializeField] private GameOverScreen _gameOverScreen;
        
        [SerializeField] private Ship _ship;

        [SerializeField] private GameActivator _activator;

        [SerializeField] private ControlManagement _controlManagement;

        private Health ShipHealth => _ship.Health;

        private Score ShipScore => _ship.Score;
        
        private void OnHealthChanged(int health)
        {
            _gameScreen.HealthView.UpdateValue(health);
        }

        private void OnScoreChanged(int score)
        {
            _gameScreen.ScoreView.UpdateValue(score);
            
            _gameOverScreen.FinalScoreView.UpdateValue(score);
        }

        private void ActivatorOnPaused()
        {
            _menuScreen.gameObject.SetActive(true);
        }

        private void ActivatorOnUnPaused()
        {
            _menuScreen.gameObject.SetActive(false);
        }

        private void ActivatorOnGameHasEnd()
        {
            _gameOverScreen.gameObject.SetActive(true);
        }

        private void ActivatorOnGameStarted()
        {
            _menuScreen.gameObject.SetActive(false);
            
            _gameOverScreen.gameObject.SetActive(false);
        }

        private void MenuScreenOnNewGameClicked()
        {
            if (!_activator.IsGameStarted)
                _activator.StartGame();
            else
                _activator.RestartGame();
        }

        private async void OnEnable()
        {
            await Task.Delay(1);
            
            ShipHealth.Changed += OnHealthChanged;
            
            ShipScore.Changed += OnScoreChanged;

            _activator.Paused += ActivatorOnPaused;
            
            _activator.UnPaused += ActivatorOnUnPaused;
            
            _activator.GameHasEnd += ActivatorOnGameHasEnd;
            
            _activator.GameStarted += ActivatorOnGameStarted;

            _activator.GameStateUpdated += _menuScreen.OnGameActiveChanged;

            _menuScreen.NewGameClicked += MenuScreenOnNewGameClicked;

            _menuScreen.CompleteClicked += _activator.UnPause;

            _menuScreen.ControlChangeClicked += _controlManagement.SetCurrentControl;

            _controlManagement.ControlUpdated += _menuScreen.OnControlIndexChanged;
        }

        private void Start()
        {
            OnHealthChanged(ShipHealth.Value);
            
            OnScoreChanged(ShipScore.Value);
            
            _menuScreen.OnControlIndexChanged(_controlManagement.CurrentControlIndex);
            
            _menuScreen.OnGameActiveChanged(_activator.IsGameStarted);
            
            _menuScreen.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            ShipHealth.Changed -= OnHealthChanged;
            
            ShipScore.Changed -= OnScoreChanged;
            
            _activator.Paused -= ActivatorOnPaused;
            
            _activator.UnPaused -= ActivatorOnUnPaused;
            
            _activator.GameHasEnd -= ActivatorOnGameHasEnd;
            
            _activator.GameStarted -= ActivatorOnGameStarted;
            
            _activator.GameStateUpdated -= _menuScreen.OnGameActiveChanged;
            
            _menuScreen.NewGameClicked -= MenuScreenOnNewGameClicked;
            
            _menuScreen.CompleteClicked -= _activator.UnPause;
            
            _menuScreen.ControlChangeClicked -= _controlManagement.SetCurrentControl;
            
            _controlManagement.ControlUpdated -= _menuScreen.OnControlIndexChanged;
        }
    }
}
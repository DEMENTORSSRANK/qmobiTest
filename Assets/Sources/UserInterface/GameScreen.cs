using UnityEngine;

namespace Sources.UserInterface
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private FormatViewText _scoreView;

        [SerializeField] private FormatViewText _healthView;

        public FormatViewText ScoreView => _scoreView;

        public FormatViewText HealthView => _healthView;
    }
}
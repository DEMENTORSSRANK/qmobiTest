using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sources.UserInterface
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private Button _menu;

        [SerializeField] private FormatViewText _finalScoreView;

        public FormatViewText FinalScoreView => _finalScoreView;

        private void OnMenuClicked()
        {
            SceneManager.LoadScene(0);
        }
        
        private void Start()
        {
            _menu.onClick.AddListener(OnMenuClicked);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace RedPanda.Project.UI
{
    public sealed class LobbyView : View
    {
        [SerializeField] private Button _startButton;

        private void Start()
        {
            _startButton.onClick.AddListener(OnStartClick);
        }

        private void OnStartClick()
        {
            UIService.Close("LobbyView");
            UIService.Show("PromoView");
        }
    }
}
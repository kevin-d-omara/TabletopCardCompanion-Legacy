using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace TabletopCardCompanion
{
    public class MainMenuPanel : MonoBehaviour
    {
        public static event EventHandler<EventArgs> ClickedSinglePlayer;
        public static event EventHandler<EventArgs> ClickedHostLan;
        public static event EventHandler<EventArgs> ClickedJoinLan;

        // Children
        [SerializeField] private Button singleplayerButton;
        [SerializeField] private Button hostLanButton;
        [SerializeField] private Button joinLanButton;
        [SerializeField] private Button exitGameButton;

        private void Start()
        {
            // Add button callbacks.
            singleplayerButton.onClick.AddListener(ClickSingleplayer);
            hostLanButton     .onClick.AddListener(ClickHostLan);
            joinLanButton     .onClick.AddListener(ClickJoinLan);
            exitGameButton    .onClick.AddListener(ClickExitGame);
        }

        private void OnEnable()
        {
            CustomNetworkDiscovery.ReceivedBroadcast += UpdateJoinButton;
        }

        private void OnDisable()
        {
            CustomNetworkDiscovery.ReceivedBroadcast -= UpdateJoinButton;
        }

        public void ClickSingleplayer()
        {
            ClickedSinglePlayer?.Invoke(this, EventArgs.Empty);
        }

        public void ClickHostLan()
        {
            ClickedHostLan?.Invoke(this, EventArgs.Empty);
        }

        public void ClickJoinLan()
        {
            ClickedJoinLan?.Invoke(this, EventArgs.Empty);
        }

        public void ClickExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void UpdateJoinButton(object sender, EventArgs e)
        {
            var text = joinLanButton.GetComponentInChildren<Text>();
            text.text = "Join Game: " + NetworkManager.singleton.networkAddress;
        }
    }
}
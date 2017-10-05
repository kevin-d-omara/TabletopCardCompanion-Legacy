using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace TabletopCardCompanion
{
    /// <summary>
    /// This component continually searches for available LAN games as soon as the game starts.
    /// </summary>
    public class CustomNetworkDiscovery : NetworkDiscovery
    {
        public static event EventHandler<EventArgs> ReceivedBroadcast;

        public static CustomNetworkDiscovery Singleton { get; private set; }

        public void PlaySingleplayer(object sender, EventArgs e)
        {
            StopBroadcast();
            NetworkManager.singleton.StartHost();
        }

        public void PlayHostLan(object sender, EventArgs e)
        {
            StopBroadcast();
            Initialize();
            StartAsServer();
            NetworkManager.singleton.StartHost();
        }

        public void PlayJoinLan(object sender, EventArgs e)
        {
            StopBroadcast();
            NetworkManager.singleton.StartClient();
        }

        public void LeaveMatch(object sender, EventArgs e)
        {
            if (running) StopBroadcast();
            NetworkManager.singleton.StopHost();
        }

        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            NetworkManager.singleton.networkAddress = fromAddress;
            Debug.Log("Broadcast Received: " + fromAddress);
            ReceivedBroadcast?.Invoke(this, EventArgs.Empty);
        }

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Singleton != this)
            {
                Destroy(this);
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            MainMenuPanel.ClickedSinglePlayer += PlaySingleplayer;
            MainMenuPanel.ClickedHostLan += PlayHostLan;
            MainMenuPanel.ClickedJoinLan += PlayJoinLan;
            UI.InGame.LeaveMatch.ClickedLeaveMatch += LeaveMatch;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            MainMenuPanel.ClickedSinglePlayer -= PlaySingleplayer;
            MainMenuPanel.ClickedHostLan -= PlayHostLan;
            MainMenuPanel.ClickedJoinLan -= PlayJoinLan;
            UI.InGame.LeaveMatch.ClickedLeaveMatch += LeaveMatch;
        }

        /// <summary>
        /// When the MainMenu scene is loaded, Initialize LAN.
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 0)
            {
                if (Initialize()) Debug.Log("Initialized");
                StartAsClient();
            }
        }
    }
}
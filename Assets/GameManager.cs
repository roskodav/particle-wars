using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Assets
{
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        ///     This client's version number. Users are separated from each other by gameVersion (which allow make breaking
        ///     changes).
        /// </summary>
        public string GameVersion = "0.0.1";

        public Color LocalPlayerColor = Color.blue;

        [Tooltip(
            "The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        public byte MaxPlayers = 2;

        public List<Player.Player> Players;
        public Color RemotePlayerColor = Color.red;


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public Player.Player GetPlayer(PhotonView photonView)
        {
            var selected = Players.FirstOrDefault(p => p.photonView.Owner == photonView.Owner);
            return selected;
        }

        public Player.Player GetPlayer(int id)
        {
            var selected = Players.FirstOrDefault(p => p.ID == id);
            return selected;
        }

        public void AddPlayer(Player.Player player)
        {
            if (Players == null)
                Players = new List<Player.Player>();
            Players.Add(player);
        }
    }
}
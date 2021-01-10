using System.Collections.Generic;
using System.Linq;
using Photon.Utilities;
using Unity.Collections;
using UnityEngine;

namespace Assets
{
    public class GameManager : Singleton<GameManager>
    {
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        public byte MaxPlayers = 2;

        public List<Player.Player> Players;

        public void Awake()
        {
            Players = FindObjectsOfType<Player.Player>().ToList();
        }

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allow make breaking changes).
        /// </summary>
        public string GameVersion = "0.0.1";
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// Caching purpose
        /// </summary>
        public int MaxPlayers = 5;

        public List<Player.Player> Players;

        public void Awake()
        {
            Players = GameObject.FindObjectsOfType<Player.Player>().ToList();
        }
    }
}

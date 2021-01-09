using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        ///     Caching purpose
        /// </summary>
        public int MaxPlayers = 5;

        public List<Player.Player> Players;

        public void Awake()
        {
            Players = FindObjectsOfType<Player.Player>().ToList();
        }
    }
}
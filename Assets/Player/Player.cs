using UnityEngine;

namespace Assets.Player
{
    public class Player : MonoBehaviour
    {
        public Color Color;
        public int ID;

        [SerializeField] private bool isLocalPlayer;
        public GameObject PlayerControl;

        public string PlayerName;

        public bool IsLocalPlayer
        {
            //TODO for multiplayer
            get => isLocalPlayer;
            set => isLocalPlayer = value;
        }

        private void Start()
        {
            name = $"Player {PlayerName}";
            var playerControl = Instantiate(PlayerControl, transform);
            playerControl.GetComponent<PlayerControl>().Player = this;
            if (isLocalPlayer)
                playerControl.gameObject.AddComponent<LocalPlayerControl>();
            else
                playerControl.gameObject.AddComponent<RemotePlayerControl>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return ID == ((Player) obj).ID;
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }
    }
}
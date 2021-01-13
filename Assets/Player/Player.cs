using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

namespace Assets.Player
{
    public class Player : MonoBehaviourPunCallbacks
    {
        public Color Color;
        public int ID;

        public GameObject PlayerControl;

        public string PlayerName;

        private void Awake()
        {
            if (photonView.IsMine) PlayerManager.LocalPlayerInstance = gameObject;
        }

        private void Start()
        {
            GameManager.Instance.AddPlayer(this);

            if (photonView.IsMine)
                Color = GameManager.Instance.LocalPlayerColor;
            else
                Color = GameManager.Instance.RemotePlayerColor;

            if (photonView.IsMine)
            {
                name = $"Player {PlayerName}";
                var playerControl = Instantiate(PlayerControl, transform);
                playerControl.GetComponent<PlayerControl>().Player = this;
                if (photonView.IsMine)
                    playerControl.gameObject.AddComponent<LocalPlayerControl>();
                else
                    playerControl.gameObject.AddComponent<RemotePlayerControl>();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return ID == ((Player) obj).ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
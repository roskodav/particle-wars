using System.Collections.Generic;
using UnityEngine;

namespace Assets.Particle
{
    [System.Serializable]
    public class PlayerInfluence
    {
        public Player.Player Player;
        public float MaxInfluence { get; set; }
        public float Influence { get; set; }

        public void AddInfluence(float influence = 1)
        {
            Influence += influence;
        }

        public void RemoveInfluence(float influence = 1)
        {
            Influence -= influence;
            Influence = Mathf.Clamp(Influence, 0, 50);
        }
    }
}

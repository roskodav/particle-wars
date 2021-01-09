using System;
using UnityEngine;

namespace Assets.Particle
{
    [Serializable]
    public class PlayerInfluence
    {
        public Player.Player Player;
        public float Influence { get; set; }

        public void AddInfluence(float influence = 1)
        {
            Influence += influence;
        }

        public void RemoveInfluence(float influence = 1)
        {
            Influence -= influence;
            Influence = Mathf.Clamp(Influence, 0, float.MaxValue);
        }
    }
}
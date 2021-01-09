using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Player
{
    public class Spawner : MonoBehaviour
    {
        public Player Player;
        public int MaxSpawnCount = 100;
        public int CurrentCount = 0;
        private static int particleID = 0;
        public float SpawnRadius = 1;
        public GameObject Particle;

        void Start()
        {
            InvokeRepeating(nameof(Spawning), Random.value * 0.1f, 0.005f);

            if (Player == null)
                Player = GetComponent<Player>();
        }

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, SpawnRadius);
        }

        private void Spawning()
        {
            global::Particle.Spawn(
                Particle, $"Particle {particleID}",
                transform,
                (Vector2)this.transform.position + (Random.insideUnitCircle * SpawnRadius),
                Player);

            CurrentCount++;
            if (MaxSpawnCount <= CurrentCount)
            {
                CancelInvoke(nameof(Spawning));
            }
        }
    }
}

using Assets.Player;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviourPunCallbacks
{
    private static int _particleId;
    public int CurrentCount;
    public int MaxSpawnCount = 100;
    public GameObject ParticleObj;
    public Player Player;
    public float SpawnRadius = 1;
    public bool LocalDebug = false;

    private void Start()
    {
        InvokeRepeating(nameof(Spawning), Random.value * 0.1f, 0.005f);

        if (Player == null)
            Player = GetComponent<Player>();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.back, SpawnRadius);
    }
#endif

    private void Spawning()
    {
        if (photonView.IsMine || LocalDebug)
        {
            Particle.Spawn(
                ParticleObj,
                $"Particle {_particleId++}",
                transform,
                (Vector2) transform.position + Random.insideUnitCircle * SpawnRadius,
                Player, LocalDebug);

            CurrentCount++;
            if (MaxSpawnCount <= CurrentCount) CancelInvoke(nameof(Spawning));
        }
        else
        {
            CancelInvoke(nameof(Spawning));
        }
    }
}
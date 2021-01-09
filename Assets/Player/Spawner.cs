using Assets.Player;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private static int _particleId;
    public int CurrentCount;
    public int MaxSpawnCount = 100;
    public GameObject ParticleObj;
    public Player Player;
    public float SpawnRadius = 1;

    private void Start()
    {
        InvokeRepeating(nameof(Spawning), Random.value * 0.1f, 0.005f);

        if (Player == null)
            Player = GetComponent<Player>();
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.back, SpawnRadius);
    }

    private void Spawning()
    {
        Particle.Spawn(
            ParticleObj, $"Particle {_particleId++}",
            transform,
            (Vector2) transform.position + Random.insideUnitCircle * SpawnRadius,
            Player);


        CurrentCount++;
        if (MaxSpawnCount <= CurrentCount) CancelInvoke(nameof(Spawning));
    }
}
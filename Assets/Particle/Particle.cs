using System.Collections.Generic;
using System.Linq;
using Assets;
using Assets.Particle;
using Assets.Player;
using UnityEditor;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private float _currentControledTime;
    public float ControledInfluenceBonus = 1.2f;
    public float ControledInfluenceBonusLifeTime = 0.5f;

    /// <summary>
    ///     Current power to influence other cells
    /// </summary>
    public float CurrentInfluencePower;

    public float DefaultInfluencePower = 1;

    /// <summary>
    ///     Points per update
    /// </summary>
    public float HealingSpeed = 1;

    public float InfluenceRadius = 1;
    public bool IsControlledByMouse;
    public float Life = 100;
    public float MaxLife = 100;

    public Player Owner;

    /// <summary>
    ///     Used for counting witch player have most influence on this particle
    /// </summary>
    public Dictionary<int, PlayerInfluence> PlayersInfluence;

    public void SetControlled()
    {
        IsControlledByMouse = true;
        _currentControledTime = 0;
    }

    public void RemoveInfluenceBonus()
    {
        IsControlledByMouse = false;
        _currentControledTime = 0;
    }

    public static void Spawn(GameObject particlePrefab, string name, Transform parent, Vector2 position, Player owner)
    {
        var particle = Instantiate(particlePrefab, position, new Quaternion(0, 0, 0, 0));
        particle.transform.parent = parent;
        var particleComponent = particle.GetComponent<Particle>();
        particleComponent.Owner = owner;
        particle.name = name;
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.back, InfluenceRadius);
    }

    private void Update()
    {
        _currentControledTime += Time.deltaTime;
        if (IsControlledByMouse && _currentControledTime > ControledInfluenceBonusLifeTime)
            RemoveInfluenceBonus();

        CurrentInfluencePower = DefaultInfluencePower * (IsControlledByMouse ? ControledInfluenceBonus : 1);
    }

    // Start is called before the first frame update
    private void Start()
    {
        //Cache max possible player
        PlayersInfluence = new Dictionary<int, PlayerInfluence>(GameManager.Instance.MaxPlayers);
        InvokeRepeating(nameof(HealingUpdate), 1f, 1f);
        InvokeRepeating(nameof(ApplyInfluence), 1f, 1f);
        InvokeRepeating(nameof(CheckInfluence), Random.value, 0.5f);

        if (Owner != null)
            ChangePlayer(Owner);

        foreach (var player in GameManager.Instance.Players)
            PlayersInfluence[player.ID] = new PlayerInfluence
            {
                Player = player
            };
    }

    /// <summary>
    ///     Run every second
    /// </summary>
    private void HealingUpdate()
    {
        if (Life < MaxLife)
            Life += HealingSpeed;
    }

    private void ChangePlayer(Player targetPlayer)
    {
        Owner = targetPlayer;
        Life = MaxLife / 2;
    }

    private void AddInfluence(Particle enemyParticle)
    {
        if (enemyParticle.Owner == null)
            return;

        //Add influce bonus if controlled
        PlayersInfluence[enemyParticle.Owner.ID].AddInfluence(enemyParticle.CurrentInfluencePower);
    }

    public void ApplyInfluence()
    {
        var totalInfluence = PlayersInfluence
            .Sum(p =>
                p.Value.Player.Equals(Owner) ? -p.Value.Influence : +p.Value.Influence);

        Life += totalInfluence;
        Life = Mathf.Clamp(Life, 0, MaxLife);

        if (Life <= 0)
        {
            var topEnemyInfluencer = PlayersInfluence
                .Select(p => p.Value)
                .Where(p => !p.Player.Equals(Owner))
                .OrderByDescending(p => p.Influence)
                .FirstOrDefault();

            ChangePlayer(topEnemyInfluencer.Player);
        }

        foreach (var playerInfluence in PlayersInfluence)
            playerInfluence.Value.Influence = 0;
    }

    private void CheckInfluence()
    {
        foreach (var coll in Physics2D.OverlapCircleAll(transform.position, InfluenceRadius))
        {
            var particle = coll.gameObject.GetComponent<Particle>();
            if (particle != null) AddInfluence(particle);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Assets;
using Assets.Particle;
using Assets.Player;
using Unity.Profiling;
using UnityEngine;
using Random = UnityEngine.Random;

public class Particle : MonoBehaviour
{
    /// <summary>
    ///     Points per update
    /// </summary>
    public float Healing = 1;
    public float InfluencePower = 1;
    public float Life = 100;
    public float MaxLife = 100;
    public Player Owner;
    public bool EnableDebug = false;
    public float influenceRadius = 1;
    /// <summary>
    ///     Used for counting witch player have influence on cell
    /// </summary>
    public Dictionary<int, PlayerInfluence> PlayersInfluence;

    private SpriteRenderer rend;


    private void OnDrawGizmos()
    {
       
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, influenceRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Cache max possible player
        PlayersInfluence = new Dictionary<int, PlayerInfluence>(GameManager.Instance.MaxPlayers);
        rend = GetComponent<SpriteRenderer>();
        InvokeRepeating(nameof(HealingUpdate), 1f, 1f);
        InvokeRepeating(nameof(ApplyInfluence), 1f, 1f);
        InvokeRepeating(nameof(CheckInfluence), Random.value,0.5f);

        ChangePlayer(GameManager.Instance.Players[Random.Range(0, GameManager.Instance.Players.Count)]);

        if (Owner != null)
            ChangePlayer(Owner);

        foreach (var player in GameManager.Instance.Players)
        {
            PlayersInfluence[player.ID] = new PlayerInfluence()
            {
                Player = player
            };
        }
    }

    /// <summary>
    ///     Run every second
    /// </summary>
    private void HealingUpdate()
    {
        if (Life < MaxLife)
            Life += Healing;
    }

    //void RemoveNullParticles()
    //{
    //    foreach (var playerInfluence in PlayersInfluence)
    //    {
    //        if (playerInfluence.Value == null || playerInfluence.Value.Particle == null)
    //            PlayersInfluence.Remove(playerInfluence.Key);
    //    }
    //}

    private void ChangePlayer(Player targetPlayer)
    {
        Owner = targetPlayer;
        rend.color = targetPlayer.Color;
        Life = MaxLife / 2;
    }



    private void AddInfluence(Particle enemyParticle)
    {
        if (enemyParticle.Owner == null)
            return;

        PlayersInfluence[enemyParticle.Owner.ID].AddInfluence(enemyParticle.InfluencePower);
    }

    public void ApplyInfluence()
    {
        var totalEnemyInfluence = PlayersInfluence
            .Where(p => !p.Value.Player.Equals(Owner))
            .Sum(p => p.Value.Influence);

        var totalAlliesInfluence = PlayersInfluence
            .Where(p => p.Value.Player.Equals(Owner))
            .Sum(p => p.Value.Influence);

        Life = Life + totalAlliesInfluence - totalEnemyInfluence;

        if (EnableDebug)
            Debug.Log($"totalAlliesInfluence:{totalAlliesInfluence} totalEnemyInfluence:{totalEnemyInfluence} life:{Life}");

        Life = Mathf.Clamp(Life, 0, MaxLife);
        if (Life == 0)
        {
            var topEnemyInfluencer = PlayersInfluence
                .Select(p => p.Value)
                .Where(p => !p.Player.Equals(Owner))
                .OrderByDescending(p => p.Influence)
                .FirstOrDefault();

            ChangePlayer(topEnemyInfluencer.Player);

            if (EnableDebug)
                Debug.Log("Change player");
        }

        foreach (var playerInfluence in PlayersInfluence)
        {
            playerInfluence.Value.Influence = 0;
        }
    }

    private void CheckInfluence()
    {
        Debug.Log("Check influence");

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, influenceRadius) )
        {
            var particle = collider.gameObject.GetComponent<Particle>();
            if (particle != null)
            {
                AddInfluence(particle);
            }
        }
    }

    //private void AddInfluence(Particle enemyParticle)
    //{
    //    if(enemyParticle.Owner == null)
    //        return;

    //    if (!PlayersInfluence.ContainsKey(enemyParticle.Owner.ID))
    //        PlayersInfluence[enemyParticle.Owner.ID] = new PlayerInfluence()
    //        {
    //            Particle = enemyParticle
    //        };

    //    PlayersInfluence[enemyParticle.Owner.ID].AddInfluence(enemyParticle.InfluencePower);
    //}

    //private void RemoveInfluence(Particle enemyParticle)
    //{
    //    if (enemyParticle.Owner == null)
    //        return;

    //    if (!PlayersInfluence.ContainsKey(enemyParticle.Owner.ID))
    //        PlayersInfluence[enemyParticle.Owner.ID] = new PlayerInfluence()
    //        {
    //            Particle = enemyParticle
    //        };

    //    PlayersInfluence[enemyParticle.Owner.ID].RemoveInfluence(enemyParticle.InfluencePower);
    //}

    //public void ApplyInfluence()
    //{

    //    var totalEnemyInfluence = PlayersInfluence
    //        .Where(p => !p.Value.Player.Equals(Owner))
    //        .Sum(p => p.Value.Influence);

    //    var totalAlliesInfluence = PlayersInfluence
    //        .Where(p => p.Value.Player.Equals(Owner))
    //        .Sum(p => p.Value.Influence);

    //    Life = Life + totalAlliesInfluence - totalEnemyInfluence;

    //    if (EnableDebug)
    //        Debug.Log($"totalAlliesInfluence:{totalAlliesInfluence} totalEnemyInfluence:{totalEnemyInfluence} life:{Life}");


    //    Life = Mathf.Clamp(Life, 0, MaxLife);
    //    if (Life == 0)
    //    {
    //        var topEnemyInfluencer = PlayersInfluence
    //            .Select(p => p.Value)
    //            .Where(p => !p.Player.Equals(Owner))
    //            .OrderByDescending(p => p.Influence)
    //            .FirstOrDefault();

    //        ChangePlayer(topEnemyInfluencer.Player);

    //        if (EnableDebug)
    //            Debug.Log("Change player");
    //    }
    //}

    //When the Primitive collides with the walls, it will reverse direction
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (EnableDebug)
    //        Debug.Log("Enter colision");

    //    var particle = collision.gameObject.GetComponent<Particle>();

    //    if (particle != null)
    //    {
    //        AddInfluence(particle);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (EnableDebug)
    //        Debug.Log("Exit colision");

    //    var particle = collision.gameObject.GetComponent<Particle>();

    //    if (particle != null)
    //    {
    //        RemoveInfluence(particle);
    //    }
    //}

}
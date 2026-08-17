using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotManager : Singleton<BotManager>
{
    [Header("Settings")]
    [SerializeField] private int maxBotsOnField = 10;
    [SerializeField] private int maxTotalBots = 50;
    [SerializeField] private float spawnRadius = 25f;

    public List<Bot> ActiveBots { get; private set; } = new List<Bot>();

    private int totalSpawnedBots = 0;
    
    private Transform tf;
    public Transform TF
    {
        get
        {
            if (tf == null) tf = transform;
            return tf;
        }
    }

    private void Start()
    {
        GameManager.ChangeState(GameState.Playing);
        
        OnInit();
    }

    public void OnInit()
    {
        ActiveBots.Clear();
        totalSpawnedBots = 0;
        
        int initialSpawnCount = Mathf.Min(maxBotsOnField, maxTotalBots);
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnBot();
        }
    }

    private void SpawnBot()
    {
        if (totalSpawnedBots >= maxTotalBots) return;

        if (ActiveBots.Count >= maxBotsOnField) return;

        Vector3 spawnPos = GetRandomSpawnPoint();

        Bot bot = SimplePool.Spawn<Bot>(PoolType.Bot, spawnPos, Quaternion.identity);
        
        bot.OnInit(); 
        
        ActiveBots.Add(bot);
        totalSpawnedBots++;
    }

    public void OnBotDeath(Bot bot)
    {
        if (ActiveBots.Contains(bot))
        {
            ActiveBots.Remove(bot);
        }

        if (totalSpawnedBots < maxTotalBots)
        {
            Invoke(nameof(SpawnBot), 2f);
        }
    }

    public void OnReset()
    {
        for (int i = ActiveBots.Count - 1; i >= 0; i--)
        {
            if (ActiveBots[i] != null && ActiveBots[i].gameObject.activeSelf)
            {
                SimplePool.Despawn(ActiveBots[i]);
            }
        }
        ActiveBots.Clear();
        totalSpawnedBots = 0;
        CancelInvoke(nameof(SpawnBot));
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * spawnRadius;
        randomDir += TF.position;
        randomDir.y = 0;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        
        return TF.position;
    }
}

using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [Header("Movement")]
    [SerializeField] private NavMeshAgent agent;

    private BotState currentState;
    private Vector3 destination;

    public bool IsAgentMoving => agent != null && agent.velocity.sqrMagnitude > 0.0001f;

    public float IdleTimer { get; set; }

    public bool IsDestinationReached 
    {
        get 
        {
            if (agent == null || !agent.isOnNavMesh) return true;
            
            Vector3 dest = destination;
            dest.y = TF.position.y; 
            
            return Vector3.Distance(TF.position, dest) < 0.1f;
        }
    }

    public override void OnInit()
    {
        base.OnInit();

        float randomSize = Random.Range(0.8f, 1.2f);
        SetSize(randomSize);

        EnableAgent();
        ChangeBotStateTo(BotStates.Idle);
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        DisableAgent();
        SimplePool.Despawn(this);
    }

    private void Update()
    {
        if (!GameManager.IsState(GameState.Playing)) return;

        currentState?.OnExecute(this);
    }

    public void ChangeBotStateTo(BotState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }

    public void DisableAgent()
    {
        if (agent != null)
        {
            if (agent.isOnNavMesh) agent.isStopped = true;
            agent.enabled = false;
        }
    }

    public void EnableAgent()
    {
        if (agent != null)
        {
            agent.enabled = true;
            if (agent.isOnNavMesh) agent.isStopped = false;
        }
    }

    public void SetDestination(Vector3 targetPos)
    {
        if (agent == null || !agent.isOnNavMesh) return;
        CancelAttack();
        destination = targetPos;
        agent.SetDestination(destination);
        agent.isStopped = false;
        
        ChangeAnim(GameConfig.ANIM_RUN); 
    }

    public void StopMoving()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
        
        ChangeAnim(GameConfig.ANIM_IDLE); 
    }

    public bool HasAttackTarget()
    {
        return CanAttack();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        DisableAgent();
        ChangeBotStateTo(null); 

        if (BotManager.Ins != null)
        {
            BotManager.Ins.OnBotDeath(this);
        }
    }
    
    public void MoveToRandomDestination()
    {
        Vector3 randomPoint = GetRandomNavMeshPoint(TF.position, 15f);
        SetDestination(randomPoint);
    }

    private Vector3 GetRandomNavMeshPoint(Vector3 center, float range)
    {
        Vector3 randomDir = Random.insideUnitSphere * range;
        randomDir += center;
        randomDir.y = 0;

        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomDir, out hit, range, UnityEngine.AI.NavMesh.AllAreas))
        {
            return hit.position;
        }
        return center;
    }
    
    private float currentIdleDuration;
    
    public void StartIdleTimer()
    {
        IdleTimer = 0f;
        currentIdleDuration = Random.Range(1.5f, 2.5f);
    }

    public bool IsIdleTimeFinished()
    {
        IdleTimer += Time.deltaTime;
        return IdleTimer >= currentIdleDuration;
    }
}
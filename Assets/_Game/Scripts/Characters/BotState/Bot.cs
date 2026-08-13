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

    public override void OnInit()
    {
        base.OnInit();

        float randomSize = Random.Range(1.5f, 2.5f);
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
        //if (!GameManager.IsState(GameState.GamePlay)) return;

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

    public bool CheckDestinationReached()
    {
        if (agent == null || !agent.isOnNavMesh) return true;
        if (agent.pathPending) return false;
        if (agent.remainingDistance > agent.stoppingDistance) return false;
        if (agent.hasPath && agent.velocity.sqrMagnitude > 0.001f) return false;

        return true;
    }

    public bool HasAttackTarget()
    {
        return CanAttack();
    }

    public new void OnHit()
    {
        if (IsDead) return;

        IsDead = true;
        DisableAgent();
        ChangeBotStateTo(null);
        ChangeAnim(GameConfig.ANIM_DEAD);

        Invoke(nameof(DespawnBot), 2f);
    }

    private void DespawnBot()
    {
        OnDespawn();
    }
}
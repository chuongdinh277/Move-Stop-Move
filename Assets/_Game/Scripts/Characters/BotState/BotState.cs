using System;
using UnityEngine;

public class BotState
{
    public readonly Action<Bot> OnEnter;
    public readonly Action<Bot> OnExecute;
    public readonly Action<Bot> OnExit;

    public BotState(Action<Bot> onEnter, Action<Bot> onExecute, Action<Bot> onExit)
    {
        OnEnter = onEnter;
        OnExecute = onExecute;
        OnExit = onExit;
    }
}
/*
    Todo : logic state machine 
*/
public static class BotStates
{
    public static readonly BotState Idle = new BotState(IdleState.OnEnter, IdleState.OnExecute, IdleState.OnExit);
    public static readonly BotState Patrol = new BotState(PatrolState.OnEnter, PatrolState.OnExecute, PatrolState.OnExit);
    
    public static readonly BotState Attack = new BotState(AttackState.OnEnter, AttackState.OnExecute, AttackState.OnExit);

}

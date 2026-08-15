using UnityEngine;

public static class PatrolState
{
    public static void OnEnter(Bot bot)
    {
        bot.MoveToRandomDestination();
    }

    public static void OnExecute(Bot bot)
    {
        if (bot.HasAttackTarget())
        {
            bot.ChangeBotStateTo(BotStates.Attack);
            return;
        }

        if (bot.IsDestinationReached)
        {
            bot.ChangeBotStateTo(BotStates.Idle);
        }
    }

    public static void OnExit(Bot bot)
    {
        bot.StopMoving();
    }
}

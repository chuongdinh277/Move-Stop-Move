using UnityEngine;

public static class IdleState
{
    public static void OnEnter(Bot bot)
    {
        bot.StopMoving();
        bot.StartIdleTimer();
    }

    public static void OnExecute(Bot bot)
    {
        if (bot.HasAttackTarget())
        {
            bot.ChangeBotStateTo(BotStates.Attack);
            return;
        }

        if (bot.IsIdleTimeFinished())
        {
            bot.ChangeBotStateTo(BotStates.Patrol);
        }
    }

    public static void OnExit(Bot bot)
    {
    }
}

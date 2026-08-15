using UnityEngine;

public static class AttackState
{
    public static void OnEnter(Bot bot)
    {
        bot.StopMoving();
        bot.Attack(); 
    }

    public static void OnExecute(Bot bot)
    {
        if (!bot.isAttacking)
        {
            if (bot.HasAttackTarget())
            {
                bot.ChangeBotStateTo(BotStates.Attack);
            }
            else
            {
                bot.ChangeBotStateTo(BotStates.Idle);
            }
        }
    }

    public static void OnExit(Bot bot)
    {
    }
}

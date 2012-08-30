using System;

/// <summary>
/// Werewolf hunt.
/// 
/// This state is a hierarchial state machine, and 
/// should contain WerewolfIdle, WerewolfCharge and WerewolfAttack
/// </summary>

public class WerewolfHunt : AgentStateMachine
{


	public WerewolfHunt(Agent agent) : base(agent)
	{
		// add all substates
		AddStates(new WerewolfPatrol(), 
		          new WerewolfCharge(), 
		          new WerewolfAttack());
	}

	public void InitAction()
	{
		CurrentState = typeof(WerewolfPatrol);

	}

	public void ExitAction()
	{
        agent.RemoveBehaviour("avoid");
        agent.RemoveBehaviour("obstacleAvoid");
	}
	
	public override void Update(out Type nextState)
	{

		nextState = GetType();
        var targets = agent.GetAgentsInArea(Config.DefaultWerewolfVisionRange);
        float minDistance = -1.0f;
        Agent target = null;
        foreach (var camper in targets)
        {
            if (camper.GetComponent<Camper>() != null)
            {
                if (minDistance < -1.0f || minDistance < agent.distanceTo(camper) &&
                    !AttackPair.IsTarget(camper))
                {
                    target = camper;
                }
            }
        }

        if (target != null)
        {
            var currentTarget = AttackPair.GetTargetOrNull(agent);

            // Examine the two distance, if the new camper is significantly
            // close then the old one, start chasing the new one.

            if (currentTarget == null || 
                agent.distanceTo(currentTarget) * 1.5 > agent.distanceTo(target) ||
                currentTarget.Health < 0)
            {
                AttackPair.RemoveByAttacker(agent);
                AttackPair.Add(agent, target);
            }


        }
	}
}
using System;
using UnityEngine;

/// <summary>
/// Werewolf hunt.
/// 
/// This state is a hierarchial state machine, and 
/// should contain WerewolfIdle, WerewolfCharge and WerewolfAttack
/// </summary>

public class WerewolfHunt : AgentStateMachine
{
    bool beingAttacked = false;

	public WerewolfHunt(Agent agent) : base(agent)
	{
		// add all substates
		AddStates(new WerewolfPatrol(), 
		          new WerewolfCharge(), 
		          new WerewolfAttack(),
                  new WerewolfDead()
                  );
	}

	public void InitAction()
	{
		CurrentState = typeof(WerewolfPatrol);
        beingAttacked = false;

	}

	public void ExitAction()
	{
        agent.RemoveBehaviour("avoid");
        agent.RemoveBehaviour("obstacleAvoid");
        AttackPair.RemoveByAttacker(agent);
	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();
    
        if (agent.Health <= 0) 
        {
            CurrentState = typeof(WerewolfDead);
        }

        if (beingAttacked)
        {
            // We are a cowardly werewolf who runs when shot at.
            nextState = typeof(WerewolfEvade);
            return;
        }

        var targets = agent.GetAgentsInArea(Config.DefaultWerewolfVisionRange);
        float minDistance = -1.0f;
        Agent target = null;
        foreach (var camper in targets)
        {
            if (camper.GetComponent<Camper>() != null)
            {
                // Perform a raycast check.

                if (minDistance < -1.0f || minDistance < agent.distanceTo(camper) &&
                    !AttackPair.IsTarget(camper))
                {
                    Vector3 direction = (camper.transform.position - agent.transform.position).normalized;
                    var hits =  Physics.RaycastAll(agent.transform.position, direction,Config.DefaultWerewolfVisionRange);
                    bool visible = true;
                    foreach (var hit in hits)
                    {
                        if (hit.collider.gameObject != agent.gameObject && hit.collider.gameObject != camper.gameObject)
                        {
                            visible = false;
                        }
                    }
                    if (visible)
                    {
                       target = camper;
                    }
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

    /// <summary>
    /// The werewolf has take a shot.
    /// </summary>
    public void TakeHit()
    {
        // Swap to evasive state.
        beingAttacked = true;
    }
}
using System;
using UnityEngine;

public class WerewolfAttack : AgentState
{
	Agent target;

	public void InitAction()
	{
		target = AttackPair.GetTargetOrNull(agent);
	}

	public void ExitAction()
	{
		AttackPair.RemoveByAttacker(agent);
	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();
		 
		target = AttackPair.GetTargetOrNull(agent);

		// has no target -> Patrol
		if (target == null) {
			nextState = typeof(WerewolfPatrol);
		}

		//  Has target, but not in range for attack -> Charge
		else if (target != null && agent.distanceTo(target) > Config.DefaultWerewolfAttackRange) {
			nextState = typeof(WerewolfCharge);
		}

		// chew on it
		// TODO: Throttle this action
		else {
			target.Health -= 1;
			Debug.Log("Werewolf is chewing on its target.");
		}
	}
}
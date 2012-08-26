using System;

public class WerewolfAttack : AgentState
{
	Agent target;

	public void InitAction()
	{

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
		else if (target != null && agent.distanceTo(target) > Config.WerewolfAttackRange) {
			nextState = typeof(WerewolfCharge);
		}

		// chew on it
		else {
			target.Health -= 1;
		}
	}
}
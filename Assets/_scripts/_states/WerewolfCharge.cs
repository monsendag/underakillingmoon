using System;

public class WerewolfCharge : AgentState
{
	Agent target;

	public void InitAction()
	{

	}

	public void ExitAction()
	{

	}

	public override void Update(out Type nextState)
	{
		nextState = GetType();

		target = AttackPair.GetTargetOrNull(agent);

		/// Has no target -> go back to Patrolling
		if (target == null) {
			nextState = typeof(WerewolfPatrol);
		}

		/// Has target, target in range for Attacking -> Attack
		if (target != null && agent.distanceTo(target) < Config.DefaultWerewolfAttackRange) {
			nextState = typeof(WerewolfAttack);
		}
	}
}


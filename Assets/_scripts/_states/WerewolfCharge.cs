using UnityEngine;

public class WerewolfCharge : AgentStateMachine
{
	Agent target, attacker;
	
	public void InitAction()
	{

	}

	public void ExitAction()
	{

	}

	public override void Update(out AgentStateMachine nextState)
	{
		nextState = this;

		/// Has no target -> Idle
		if (target == null) {
			nextState = new WerewolfPatrol();
		}

		/// Has target, target in range -> Attack
		if (target != null && agent.distanceTo(target) < Config.WerewolfAttackRange) {
			nextState = new WerewolfAttack();

		}
		
	}

}


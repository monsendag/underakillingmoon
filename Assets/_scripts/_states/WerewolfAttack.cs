using UnityEngine;

public class WerewolfAttack : AgentStateMachine
{
	Agent target;
	Agent attacker;

	public void InitAction()
	{

	}

	public void ExitAction()
	{

	}
	
	public override void Update(out AgentStateMachine nextState)
	{
		nextState = this;
		 
		/// Attacked -> Evade
		if (attacker != null) {
			nextState = new WerewolfEvade();
		}
		
		//  no attacker, no target -> Idle
		if (attacker == null && target == null) {
			nextState = new WerewolfIdle();
		}
		
		//  Has target, target not in range -> Charge
		if (target != null && agent.distanceTo(target) > Config.DefaultWerewolfChargeRange) {
			nextState = new WerewolfCharge();
		}
	}
}
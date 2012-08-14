using UnityEngine;

public class WerewolfAttack : AgentStateMachine
{
	Agent target;
	Agent attacker;

	public void initAction()
	{

	}

	public void exitAction()
	{

	}
	
	public new void Update(out AgentStateMachine nextState)
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
		if (target != null && agent.distanceTo(target) > Config.WerewolfChargeRange) {
			nextState = new WerewolfCharge();
		}
	}
}
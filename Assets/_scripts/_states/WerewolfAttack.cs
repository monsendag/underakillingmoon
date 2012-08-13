using UnityEngine;

public class WerewolfAttack : IAgentState
{
	Agent target;
	Agent attacker;
	
	public WerewolfAttack(Agent target)
	{
		this.target = target;
	}
	
	public void Update(Agent agent, out IAgentState nextState)
	{
		nextState = this;
		 
		/// Attacked -> Evade
		if (attacker != null) {
			nextState = new WerewolfEvade(attacker);
		}
		
		//  no attacker, no target -> Idle
		if (attacker == null && target == null) {
			nextState = new WerewolfIdle();
		}
		
		//  Has target, target not in range -> Charge
		// TODO: move distance setting to configuration
		if (target != null && agent.distanceTo(target) > 5) {
			nextState = new WerewolfCharge(target);
		}
	}
}
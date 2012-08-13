using UnityEngine;

public class WerewolfCharge : IAgentState
{
	Agent target, attacker;
	
	public WerewolfCharge(Agent target)
	{
		this.target = target;
	}
	
	public void Update(Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// Has target, target in range -> Attack
		/// TODO: move distance setting to configuration
		if (target != null && agent.distanceTo(target) < 5f) {
			nextState = new WerewolfAttack(target);
		}
		
		/// Being attacked -> Evade
		if (attacker != null) {
			nextState = new WerewolfEvade(attacker);
		}
		
		/// Has no target -> Idle
		if (target == null) {
			nextState = new WerewolfIdle();
		}
	}

	public void attackedStart(Agent attacker)
	{
		this.attacker = attacker;
	}

	public void attackedEnd()
	{
		this.attacker = null;
	}
}


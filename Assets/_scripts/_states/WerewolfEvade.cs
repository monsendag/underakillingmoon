using UnityEngine;

public class WerewolfEvade : IAgentState
{
	Agent attacker, target;

	public WerewolfEvade(Agent attacker)
	{
		this.attacker = attacker;
	}
	
	public void Update(Agent agent, out IAgentState nextState)
	{
		nextState = this;

		float attackRange = 5f;
		
		/// Not attacked, target in range -> Attack
		/// TODO: move attack range to configuration
		if (attacker == null && target != null && agent.distanceTo(target) < attackRange) {
			nextState = new WerewolfAttack(target);
		}
		
		/// Not attacked, target not in range -> Charge
		///
		if (attacker == null && target != null && agent.distanceTo(target) > attackRange) {
			nextState = new WerewolfCharge(target);
		}
		
		/// Not attacked, has no target -> Idle
		if (attacker == null && target == null) {
			nextState = new WerewolfIdle();
		}
	}

	public void attackStart(Agent target)
	{
		this.target = target;
	}

	public void attackEnd()
	{
		this.target = null;
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



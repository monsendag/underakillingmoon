using UnityEngine;

public class WerewolfIdle : IAgentState
{
	public WerewolfIdle()
	{
		
	}
	
	public void Update(Agent agent, out IAgentState nextState)
	{
		nextState = this;

		Agent target = null, attacker = null;

		
		/// Has target, target not in range -> Charge
		if (true) {
			nextState = new WerewolfCharge(target);
		}
		
		/// Has target, target in range -> Attack
		if (true) {
			nextState = new WerewolfAttack(target);
		}
		
		/// Attacked -> Evade
		if (true) {
			nextState = new WerewolfEvade(attacker);
		}
	}
}


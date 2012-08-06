using UnityEngine;

public class WerewolfAttack : IAgentState
{
	public WerewolfAttack ()
	{
		
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// Attacked -> Evade
		if (true) { 
			nextState = new WereWolfEvade ();
		}
		
		//  Not attacked, has no target -> Idle
		if (true) {
			nextState = new WerewolfIdle ();
		}
		
		//  Has target, target not in range -> Charge
		if (true) {
			nextState = new WerewolfCharge ();
		}
		
	}
}
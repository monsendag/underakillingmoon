using UnityEngine;

public class WereWolfEvade : IAgentState
{
	public WereWolfEvade ()
	{
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// Not attacked, target in range -> Attack
		if (true) {
			nextState = new WerewolfAttack ();
		}
		
		/// Not attacked, target not in range -> Charge
		if (true) {
			nextState = new WerewolfAttack ();
		}
		
		/// Not attacked, has no target -> Idle
		if (true) {
			nextState = new WerewolfAttack ();
		}
	}
}



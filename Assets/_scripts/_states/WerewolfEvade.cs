using UnityEngine;

public class WereWolfEvade : IAgentState
{
	public WereWolfEvade ()
	{
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// werewolf is not attacked anymore -> attack
		if (true) {
			nextState = new WerewolfAttack ();
		}
	}
}



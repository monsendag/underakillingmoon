using UnityEngine;

public class WerewolfAttack : IAgentState
{
	public WerewolfAttack ()
	{
		
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// werewolf is attacked -> evade
		if (true) { 
			nextState = new WereWolfEvade ();
		}
		
		// werewolf kills it's prey -> find a new one
		if (true) {
			nextState = new WerewolfIdle ();
		}
		
	}
}
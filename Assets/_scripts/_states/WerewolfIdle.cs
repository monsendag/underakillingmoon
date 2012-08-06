using UnityEngine;

public class WerewolfIdle : IAgentState
{
	public WerewolfIdle ()
	{
		
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		
		nextState = this;
		
		/// werewolf discovers a prey -> charge
		if (true) {
			nextState = new WerewolfCharge ();
		}
		
	}
}


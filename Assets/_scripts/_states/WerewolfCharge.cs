using UnityEngine;

public class WerewolfCharge : IAgentState
{
	public WerewolfCharge ()
	{
		
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		if (true) { //query world nextState)
//			nextState = new SomeAgentState ();
		}
		
	}
}


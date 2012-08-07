using UnityEngine;

public class WerewolfCharge : IAgentState
{
	public WerewolfCharge ()
	{
		
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// Has target, target in range -> Attack
		if (true) { 
			nextState = new WerewolfAttack ();
		}
		
		/// Being attacked -> Evade
		if (true) { 
			nextState = new SomeAgentState ();
		}
		
		/// Has no target -> Idle
		if (true) { 
			nextState = new SomeAgentState ();
		}
		
	}
}


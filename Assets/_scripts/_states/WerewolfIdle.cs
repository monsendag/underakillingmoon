using UnityEngine;

public class WerewolfIdle : IAgentState
{
	public WerewolfIdle ()
	{
		
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// Has target, target not in range -> Charge
		if (true) {
			nextState = new WerewolfCharge ();
		}
		
		/// Attacked -> Evade
		if (true) {
			nextState = new WerewolfCharge ();
		}
		
		/// Attacked -> Evade
		if (true) {
			nextState = new WerewolfCharge ();
		}
	}
}


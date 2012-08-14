using UnityEngine;

/// <summary>
/// Werewolf hunt.
/// 
/// This state is a hierarchial state machine, and 
/// should contain WerewolfIdle, WerewolfCharge and WerewolfAttack
/// </summary>

public class WerewolfHunt : AgentStateMachine
{

	public void initAction()
	{

	}

	public void exitAction()
	{

	}
	
	public new void Update(out AgentStateMachine nextState)
	{
			
		/// camper is attacked -> Evade
		//if (true) { 
		nextState = new CamperEvade(); 
		//} 


		nextState = this;
		// Allow the agent state to update.
		if (CurrentState != null) {
			AgentStateMachine nextSubstate;
			CurrentState.Update(out nextSubstate);
			CurrentState = nextSubstate;
		}
	}	
}
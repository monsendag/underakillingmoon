using UnityEngine;

/// <summary>
/// Werewolf hunt.
/// 
/// This state is a hierarchial state machine, and 
/// should contain WerewolfIdle, WerewolfCharge and WerewolfAttack
/// </summary>

public class WerewolfHunt : AgentStateMachine
{

	public WerewolfHunt()
	{
		// construction phase. All substates are also constructed
		SetStates(new WerewolfPatrol(), 
		          new WerewolfCharge(), 
		          new WerewolfAttack());
	}

	public void InitAction()
	{
		// add behaviours related to hunt state


		SetState(typeof(WerewolfPatrol)); // call Init on substate
	}

	public void ExitAction()
	{
		// remove behaviours related to hunt state

	}
	
	public override void Update(out AgentStateMachine nextState)
	{
			
		/// camper is attacked -> Evade
		if (true) { 
			nextState = new CamperEvade(); 
		}
		nextState = this;
	}
}
using System;

/// <summary>
/// Werewolf hunt.
/// 
/// This state is a hierarchial state machine, and 
/// should contain WerewolfIdle, WerewolfCharge and WerewolfAttack
/// </summary>

public class WerewolfHunt : AgentStateMachine
{

	public WerewolfHunt(Agent agent) : base(agent)
	{
		// add all substates
		SetStates(new WerewolfPatrol(), 
		          new WerewolfCharge(), 
		          new WerewolfAttack());
	}

	public void InitAction()
	{
		// TODO: add relevant behaviours
		SetState(typeof(WerewolfPatrol)); 
	}

	public void ExitAction()
	{
		// TODO: remove relevant behaviours

	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();
	}
}
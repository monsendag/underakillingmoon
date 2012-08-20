using System;

public class CamperCamp : AgentStateMachine
{
	private bool attacked = false;

	public CamperCamp(Agent agent) : base(agent)
	{
		// construction phase. All substates are also constructed
		SetStates(new CamperIdle(), 
		          new CamperFlock());
	}

	public void InitAction()
	{
		// 
		SetState(typeof(CamperIdle));
	}

	public void ExitAction()
	{
		attacked = false;
	}

	public override void Update(out Type nextState)
	{
		nextState = GetType();
		/// camper is attacked -> Evade
		if (attacked) { 
			nextState = typeof(CamperEvade);
		}
	}

	/// <summary>
	/// Notifies the Camper that it's being attacked.
	/// </summary>
	public void AttackNotify(out Type nextState)
	{
		nextState = GetType();
		attacked = true;
	}
} 
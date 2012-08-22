using System;

public class CamperDead : AgentState
{
	public void InitAction()
	{

	}

	public void ExitAction()
	{

	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();
		/// timer... wake up as werewolf.. What should happen here?
		/// TODO: Remove Camper script component and add a Werewolf component
		/// to the UnityObject. This would basically result in
		/// Camper being garbage collected, and a new Werewolf object created.
		/// TODO: figure out a javascript setTimeout equivalent in C#.
		/// 
		if (true) { 
			
		}
	}
}



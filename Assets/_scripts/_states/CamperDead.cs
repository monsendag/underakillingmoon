using System;
using UnityEngine;

public class CamperDead : AgentState
{
	public void InitAction()
	{
		Debug.Log("Camper died.");
	}

	public void ExitAction()
	{

	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();

	}
}



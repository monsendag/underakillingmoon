using UnityEngine;
using System.Collections;

public class Camper : Agent
{
	public override void Start()
	{
		base.Start();

		StateMachine = new AgentStateMachine(this);
		StateMachine.SetStates(new CamperCamp(this), 
		                       new CamperEvade(),
		                       new CamperDead());
	}
	
	public override void Update()
	{
		base.Update();
	}
}

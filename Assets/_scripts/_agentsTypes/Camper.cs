using UnityEngine;
using System.Collections;

public class Camper : Agent
{
	public override void Start()
	{
		base.Start();

		StateMachine = new AgentStateMachine(this);
		StateMachine.SetStates(new CamperFlock(),
			          			new CamperIdle(),
		                       new CamperDead(), 
		                       new CamperEvade()	 
		);
	}
	
	public override void Update()
	{
		base.Update();
	}
}

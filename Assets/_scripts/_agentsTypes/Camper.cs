using UnityEngine;
using System.Collections;

public class Camper : Agent
{
	public override void Start()
	{
		base.Start();

		StateMachine = new AgentStateMachine(this, new CamperIdle());
	}
	
	public override void Update()
	{
		base.Update();
	}
}

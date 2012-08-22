using UnityEngine;
using System.Collections;

public class Werewolf : Agent
{

	public override void Start()
	{
		base.Start();

		StateMachine = new AgentStateMachine(this);
		StateMachine.SetStates(new WerewolfHunt(this), 
			          		new WerewolfPatrol());
	}
	
	public override void Update()
	{
		base.Update();
	}
}

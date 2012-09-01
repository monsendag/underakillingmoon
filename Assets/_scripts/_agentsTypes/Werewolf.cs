using UnityEngine;
using System.Collections;

public class Werewolf : Agent
{
    public AudioClip SquealSound;

	public override void Start()
	{
		base.Start();

		StateMachine = new AgentStateMachine(this);
		StateMachine.SetStates(new WerewolfHunt(this), 
			          		new WerewolfEvade());
        base.MaxVelocity = 4.0f;
	}
	
	public override void Update()
	{
		base.Update();
	}
}

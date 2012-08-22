using UnityEngine;

public class WerewolfCharge : IAgentState
{
	bool underAttack;
	
	public WerewolfCharge ()
	{
		
	}
	
	public void Update (Agent agent, out IAgentState nextState)
	{
		nextState = this;
		
		/// Has target, target in range -> Attack
		if (true) { 
			nextState = new WerewolfAttack ();
		}
		
		/// Being attacked -> Evade
		if (underAttack) { 
			nextState = new WerewolfEvade ();
		}
		
		/// Has no target -> Idle
		if (true) { 
			nextState = new WerewolfIdle ();
		}
		
	}
	
	public void Trigger (int trigger){
		if(trigger == (int)Werewolf.triggers.underAttack)
			underAttack = true;
	}
}


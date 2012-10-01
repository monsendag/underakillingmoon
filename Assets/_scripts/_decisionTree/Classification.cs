using System;
using System.Linq;

public class Classification
{

	public static Classification Parse(string classification)
	{
		// parse classification as werewolf state
		Type[] werewolfStates = {typeof(WerewolfHunt), typeof(WerewolfPatrol), typeof(WerewolfCharge), 
								typeof(WerewolfAttack), typeof(WerewolfEvade)};

		Type state = werewolfStates.Single(s => s.ToString() == classification);

		// matched state
		if (state != null) {
			return new StateClassification(state);
		}

		throw new Exception("Could not parse classification.");
	} 
}
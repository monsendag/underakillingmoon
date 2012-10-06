using System;
using System.Linq;
using System.Collections.Generic;

public class Classification
{

	static Dictionary<String, Classification> classifications = new Dictionary<string, Classification>();

	public static Classification Parse(string str)
	{
		if (classifications.ContainsKey(str)) {
			return classifications [str];
		}

		Classification classification;

		// parse classification as werewolf state
		Type[] werewolfStates = {typeof(WerewolfHunt), typeof(WerewolfPatrol), typeof(WerewolfCharge), 
								typeof(WerewolfAttack), typeof(WerewolfEvade)};
        UnityEngine.Debug.Log(str);
		Type state = werewolfStates.Single(s => s.ToString() == str);

		// matched state
		if (state != null) {
			classification = new StateClassification(state);
			classifications.Add(str, classification);
			return classification;
		}

		throw new Exception("Could not parse classification.");
	} 
}
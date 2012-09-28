using UnityEngine;
using System.Collections;

public class TargetHealth : IValue
{	
	public int Decide(Agent agent){
		Agent target = AttackPair.GetTargetOrNull(agent);
        if (target == null)
        {
            return 5;
        }
        else return (int) Mathf.Clamp(target.Health, 0, 100) / 20;
	}
}

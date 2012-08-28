//using System;
//using System.Linq;
using UnityEngine;
using System;
using System.Reflection;

public class WerewolfPatrol : AgentState
{
    CampFire Target;
    CampFire[] CampfireList;

    PathSteer _pathSteer = new PathSteer();
    const float SwitchDistance = 5.0f;

	public void InitAction()
	{
        // Generate a list of campfires
        _pathSteer.MaxAcceleration = 16.0f;
        _pathSteer.MaxVelocity = 15.0f;

        CampfireList = (CampFire[]) GameObject.FindSceneObjectsOfType(typeof(CampFire));

        Debug.Log(CampfireList.Length);

        SelectCamp();
		//AttackPair.RemoveByAttacker(agent);

        agent.AddBehaviour("path", _pathSteer, 0);
	}

	public void ExitAction()
	{

	}
	
	public override void Update(out Type nextState)
	{ 
		nextState = GetType();

        float distance = (_pathSteer.Target.Position - agent.KinematicInfo.Position).magnitude;
        if (distance < SwitchDistance)
        {
            // Pick another camp to patrol towards
            SelectCamp();
        }

        // Cycle through

		// search for nearby campers
		/*target = agent.GetAgentsInArea(Config.DefaultWerewolfVIsionRange) 
			.Where(c => c is Camper) // we only like Camper meat
			.OrderBy(a => agent.distanceTo(a)) // order by distance
			.FirstOrDefault(); // select closest

		// Found a target -> Charge towards it
		if (target != null) {
			AttackPair.Add(agent, target);
			nextState = typeof(WerewolfCharge);
		}*/
        Debug.DrawLine(agent.transform.position, Target.transform.position, Color.red);

	}

    private void SelectCamp()
    {
        int num = UnityEngine.Random.Range(0, CampfireList.Length);
        while (CampfireList[num] == Target)
        {
            num = UnityEngine.Random.Range(0, CampfireList.Length);
        }
        Target = CampfireList[num];

        _pathSteer.Target.Position =
            new Vector2(Target.transform.position.x, Target.transform.position.z);
    }


}


using System;
using UnityEngine;

public class CamperDead : AgentState
{
    private float _timer = 0.0f;
    private float _respawnTime = 5.0f;
    private FrictionSteer _frictionSteer = new FrictionSteer();
	public void InitAction()
	{
		Debug.Log("Camper died.");
        agent.ClearBehaviours();
        _frictionSteer.AngularVelocityFrictionPercentage = 1.0f;
        _frictionSteer.VelocityFrictionPercentage = 1.0f;
        agent.AddBehaviour("friction", _frictionSteer, 0);
        AttackPair.RemoveByTarget(agent);
	}

	public void ExitAction()
	{
        agent.RemoveBehaviour("friction");
	}
	
	public override void Update(out Type nextState)
	{
		nextState = GetType();
        _timer += Time.deltaTime;

        if (_timer > _respawnTime)
        {
            Camper camper = agent.GetComponent<Camper>();
            //Debug.Log("Camper Respawned as werewolf.");
            //GameObject werewolfPrefab = (GameObject) Resources.Load("Assets/_prefabs/werewolf.prefab");
            GameObject.Instantiate(camper.WerewolfPrefab, agent.transform.position, agent.transform.rotation);
            GameObject.Destroy(agent.gameObject);
        }
	}
}



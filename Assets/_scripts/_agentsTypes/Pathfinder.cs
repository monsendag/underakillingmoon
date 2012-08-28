using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Agent))]
public class Pathfinder : MonoBehaviour
{
	public Transform target;
	Agent _agent;
	PathSteer _pathSteer;

	// Use this for initialization
	void Start ()
	{
		Time.timeScale = 0.5f;
		_agent = gameObject.GetComponent<Agent>();
		DebugUtil.Assert (_agent != null);
		_agent.MaxVelocity = 15.0f;
		_agent.MaxAcceleration = 27.0f;
		_agent.MaxAngularVelocity = Mathf.PI / 4;
		
		_pathSteer = new PathSteer ();
		_pathSteer.MaxAcceleration = 16.0f;
        _pathSteer.MaxVelocity = 15.0f;
		_pathSteer.Target.Position = new Vector2(target.position.x, target.position.z);
		
		_agent.AddBehaviour("path", _pathSteer, 0);
        _agent.AddBehaviour("avoid", new ObstacleAvoidSteer(), 0);

	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if(Application.isPlaying == true) 
			Gizmos.DrawLine(transform.position, new Vector3(_pathSteer.LocalTarget.x, 0, _pathSteer.LocalTarget.y));
	}
}

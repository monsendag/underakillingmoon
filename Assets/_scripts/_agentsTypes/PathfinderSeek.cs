using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Agent))]
public class PathfinderSeek : MonoBehaviour {
	public Transform target;
	public bool applySmoothing = false;
	
	Agent _agent;
	SeekSteer _seekSteer;
	Vector2[] _waypoints;
	SimpleSmoothModifier _smoothMod;
	uint _currentWaypoint = 1;
	
	// Use this for initialization
	void Start () {
		AStarUtils.GetPath(transform.position, target.position, OnPathCalculated);
		
		_agent = gameObject.GetComponent<Agent>();
		_smoothMod = GetComponent<SimpleSmoothModifier>();
		DebugUtil.Assert (_agent != null);
		_agent.MaxVelocity = 9.0f;
		_agent.MaxAcceleration = 27.0f;
		_agent.MaxAngularVelocity = Mathf.PI / 100;
		
		_seekSteer = new SeekSteer ();
		_seekSteer.MaxAcceleration = 8.0f;
		_seekSteer.Target.Position = new Vector2(transform.position.x, transform.position.z);
		
		_agent.AddBehaviour("seek", _seekSteer, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(_waypoints == null || _currentWaypoint >= _waypoints.Length) return;
		
		_seekSteer.Target.Position = _waypoints[_currentWaypoint];	
	
		float dist = Vector2.Distance(_agent.KinematicInfo.Position, _waypoints[_currentWaypoint]);
		if(dist < 1f)
			++_currentWaypoint;
	}
	
	void OnPathCalculated(Path p){		
		var filtered = AStarUtils.FilterPath(p.vectorPath);
		
		if(applySmoothing) _waypoints = AStarUtils.PathToVectorArray(_smoothMod.SmoothBezier(filtered));
		else _waypoints = AStarUtils.PathToVectorArray(filtered);
			
		_currentWaypoint = 1;
	}
	
	void OnDrawGizmos(){
		if(_waypoints == null || Application.isPlaying == false) return;
		Vector2 cur = _waypoints[_currentWaypoint];
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, new Vector3(cur.x, 0.5f, cur.y));
		
		Gizmos.color = Color.cyan;
		Vector2 prev;
		for(uint i = 1; i < _waypoints.Length; ++i){
			prev = _waypoints[i - 1];
			cur = _waypoints[i];
			Gizmos.DrawLine(new Vector3(prev.x, 0, prev.y), new Vector3(cur.x, 0, cur.y));
		}
	}
}

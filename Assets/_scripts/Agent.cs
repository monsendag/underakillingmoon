/**
 * Holds a series of steering states, and finds their weighted average.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

class SteeringPair
{
	public uint priority;
	public ISteeringBehaviour behaviour;
}


/// The Agent class has two functions. The first is that is tracks steering
/// behaviours, and blends between them. It uses a priority system to determine
/// which behaviours to blend. First, the Agent will get the acceleration outputs
/// from all the steering behaviours with priority 0. If these don't suggest any
/// significant output, it then evaluates priority level 1, then 2 and so on. There
/// are five priority levels. The first priority level it finds with a significant
/// output will have the output of it's behaviours averaged, then applied to the 
/// object.
/// 
/// The second use of the agent class is to work as a finite state machine for the
/// agents behaviour. The Agent class has a property called AgentState, which is of type
/// IAgentState. It's update routine is called every update. See IAgentState for more info.
/// </summary> 
[RequireComponent(typeof(CharacterController))]
public class Agent : MonoBehaviour
{
	const uint NUM_PRIORITY_LEVELS = 5;
	const float EPSILON = 0.01f;
	const float EPSILON_ROTATION = 0.01f;
	private Dictionary<string,SteeringPair> behaviours = 
        new Dictionary<string,SteeringPair>();
	private float _maxAcceleration = 1.0f;
	private float _maxVelocity = 1.0f;
	private float _maxAngularVelocity = 1.0f;
	private uint _health = 100;
	private KinematicInfo _kinematicInfo;
	private CharacterController _controller;
	public AgentStateMachine StateMachine;
	
	#region varaccess
	public float MaxAcceleration {
		get { return _maxAcceleration; } 
		set { _maxAcceleration = value; }
	}

	public float MaxVelocity {
		get { return _maxVelocity; }
		set { _maxVelocity = value; }
	}

	public float MaxAngularVelocity {
		get { return _maxAngularVelocity; }
		set { _maxAngularVelocity = value; }
	}

	public uint Health {
		get { return _health; }
		set { 
			DebugUtil.Assert(value >= 0);
			_health = value;
		}
	}

	public KinematicInfo KinematicInfo { get { return _kinematicInfo; } }
	#endregion

	public virtual void Start()
	{
		// Initialise KinematicInfo.
		_kinematicInfo = new KinematicInfo();
		_kinematicInfo.Orientation = 0.0f;
		_kinematicInfo.Position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
		_kinematicInfo.Velocity = Vector2.zero;
		_kinematicInfo.AngularVelocity = 0.0f;
		
		_controller = gameObject.GetComponent<CharacterController>(); 
	}

	/// <summary>
	/// Adds a steering behaviour to the steering state.
	/// </summary>
	/// <param name="key">
	///    The key of the state to be added. This is so it can be looked up 
	///    later. This key must be unique.
	/// </param>
	/// <param name="behaviour">
	///    The steering behaviour to add.
	/// </param>
	/// <param name="priority">
	///     A value from 0 to NUM_PRIORITY_LEVELS - 1. Behaviours on priority
	///     level 0 are handled first. If  that priority doesn't produce any
	///     steering, drop down to level 2, and so forth.
	/// </param>
	public void AddBehaviour(string key, ISteeringBehaviour behaviour,
       uint priority)
	{
		DebugUtil.Assert(priority < NUM_PRIORITY_LEVELS);
		SteeringPair pair = new SteeringPair();
		pair.behaviour = behaviour;
		pair.priority = priority;
		behaviours [key] = pair;
	}

	/// <summary>
	/// Remove a steering behaviour with a given key. 
	/// </summary>
	/// <param name="key"></param>
	public void RemoveBehaviour(string key)
	{
		SteeringPair pair = new SteeringPair();
		if (behaviours.TryGetValue(key, out pair)) {
			behaviours.Remove(key);
		}
	}

	/// <summary>
	/// Clears all the steering behaviours.
	/// </summary>
	public void ClearBehaviours()
	{
		behaviours.Clear();
	}

	/// <summary>
	/// Set the influence factor of a behaviour.
	/// </summary>
	/// <param name="key"> The key value used to lookup the behaviour. </param>
	/// <param name="priority">
	///     A value from 0 to NUM_PRIORITY_LEVELS - 1. Behaviours on priority
	///     level 0 are handled first. If  that priority doesn't produce any
	///     steering, drop down to level 2, and so forth.
	/// </param>
	public void SetPriority(string key, uint priority)
	{
		DebugUtil.Assert(priority < NUM_PRIORITY_LEVELS);
      
		SteeringPair pair = new SteeringPair();
		if (behaviours.TryGetValue(key, out pair)) {
			behaviours [key].priority = priority;
		}
	}

	public ISteeringBehaviour GetBehaviour(string key)
	{
		SteeringPair pair = new SteeringPair();
		DebugUtil.Assert(behaviours.TryGetValue(key, out pair));
		return pair.behaviour;
	}

	// Update is called once per frame
	public virtual void Update()
	{
		// Allow the agent state to update.
		if (StateMachine != null) {
			StateMachine.Update();
		}
			
		// We keep two sets of priority counts, so we can find an average of
		// the blended behaviours.
		uint[] linearPriorityCounts = new uint[NUM_PRIORITY_LEVELS];
		uint[] angularPriorityCounts = new uint[NUM_PRIORITY_LEVELS];
		Vector2[] linearAccelerations = new Vector2[NUM_PRIORITY_LEVELS];
		float[] angularAccelerations = new float[NUM_PRIORITY_LEVELS];

		// Total up all the influences.
		foreach (var pair in behaviours.Values) {
			SteeringOutput output =
                pair.behaviour.CalculateAcceleration(this);
			// Only take a steering behaviour into account if their is  significant
			// movement.
			if (output.Linear.magnitude > EPSILON) {
				linearPriorityCounts [pair.priority] += 1;
				linearAccelerations [pair.priority] += output.Linear;
			}
            
			if (Mathf.Abs(output.Angular) > EPSILON_ROTATION) {
				angularPriorityCounts [pair.priority] += 1;
				angularAccelerations [pair.priority] += output.Angular;
			}
		}

		// We step through each behaviour priority value, until an
		// acceleration greater than epsilon is encountered.
		SteeringOutput acceleration = new SteeringOutput();
		for (uint i = 0; i < NUM_PRIORITY_LEVELS; ++i) {
			if (linearPriorityCounts [i] > 0) {
				// Blend the accelerations at the current priority level.
				Vector2 linearAveraged = Vector2.zero;
				if (linearPriorityCounts [i] > 0) {
					linearAveraged = linearAccelerations [i] /
						linearPriorityCounts [i];
				}
				float angularAveraged = 0.0f;
				if (angularPriorityCounts [i] > 0) { 
					angularAveraged = angularAccelerations [i] / angularPriorityCounts [i];
				}
				if (linearAveraged.magnitude > EPSILON) {
					acceleration.Linear = linearAveraged;
					acceleration.Angular = angularAveraged;
					break;
				} 
			}
		}

		/* If we didn't angular acceleration, see if any level will produce an angular acceleration. */
		if (Mathf.Abs(acceleration.Angular) < EPSILON_ROTATION) {
			for (uint i = 0; i < NUM_PRIORITY_LEVELS; ++i) {
				if (angularPriorityCounts [i] > 0) {
					float angularAveraged = 0.0f;
					if (angularPriorityCounts [i] > 0) {
						angularAveraged = angularAccelerations [i] / angularPriorityCounts [i];
					}
					if (Mathf.Abs(angularAveraged) > EPSILON_ROTATION) {
						acceleration.Angular = angularAveraged;
						break;
					}
				}
			}
		}

		// Scale back the acceleration to the maximum.
		if (acceleration.Linear.magnitude > _maxAcceleration) {
			acceleration.Linear /= _maxAcceleration;
		}
        
		// And limit the maximum velocity.
		if (_kinematicInfo.Velocity.magnitude > _maxVelocity) {
			_kinematicInfo.Velocity.Normalize();
			_kinematicInfo.Velocity *= _maxVelocity;
		}

        
		// Take the starting position u, and the end position as x, then
		// x = u + 1/2at^2 + vt where a is acceleration, v is velocity, and t is time.
		Vector3 motion = 
            new Vector3(_kinematicInfo.Velocity.x, 0.0f, _kinematicInfo.Velocity.y) * Time.deltaTime + 
			new Vector3(acceleration.Linear.x, 0.0f, acceleration.Linear.y) * Time.deltaTime * Time.deltaTime * 0.5f;

        // Update the velocity vectors.
        _kinematicInfo.Velocity += acceleration.Linear * Time.deltaTime;

        // Note that CharacterController.Move can call OnControllerColliderHit, which will change the
        // velocity.
		_controller.Move(motion);

		Vector2 facingVector = MotionUtils.GetOrientationAsVector(_kinematicInfo.Orientation);
		transform.LookAt(transform.position + new Vector3(facingVector.x, 0.0f, facingVector.y));


		_kinematicInfo.AngularVelocity += acceleration.Angular * Time.deltaTime;

		if (Mathf.Abs(_kinematicInfo.AngularVelocity) > MaxAngularVelocity) {
			_kinematicInfo.AngularVelocity /= Mathf.Abs(_kinematicInfo.AngularVelocity);
			_kinematicInfo.AngularVelocity *= MaxAngularVelocity;
		}

        Vector2 newPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
		
        // Update the Orientation and Position values.
        _kinematicInfo.Position = newPosition;

		_kinematicInfo.Orientation += _kinematicInfo.AngularVelocity * Time.deltaTime;
		_kinematicInfo.Orientation = MotionUtils.MapToRangeRadians(_kinematicInfo.Orientation);
	}
	
	public List<Agent> GetAgentsInArea(float radius)
	{
		return MotionUtils.GetAgentsInArea(transform.position, radius);
	}
	
	public float distanceTo(Agent other)
	{
		return Vector2.Distance(KinematicInfo.Position, other.KinematicInfo.Position);
	}

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // If the agent is moved, but hits a surface like a wall,
        // we remove the component of their velocity which moved them towards the wall.
        Vector2 normal = new Vector2(hit.normal.x, hit.normal.z);
        normal.Normalize();
        _kinematicInfo.Velocity -= Vector2.Dot(normal, _kinematicInfo.Velocity) * normal;
 
    }

}

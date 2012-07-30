/**
 * Holds a series of steering states, and finds their weighted average.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


class SteeringPair
{
    public uint priority;
    public ISteeringBehaviour behaviour;
}

public class Agent : MonoBehaviour 
{
    const uint NUM_PRIORITY_LEVELS = 5;
    const float EPSILON = 0.1f;
    
    private Dictionary<string,SteeringPair> behaviours = 
        new Dictionary<string,SteeringPair>();

    private float _maxAcceleration = 1.0f;
    private float _maxVelocity = 1.0f;
    private float _maxAngularVelocity = 1.0f;

    private KinematicInfo _kinematicInfo;
    private IAgentState _agentState;

    public float MaxAcceleration 
    {
        get { return _maxAcceleration; }
        set { _maxAcceleration = value; }
    }

    public float MaxVelocity
    {
        get { return _maxVelocity; }
        set { _maxVelocity = value; }
    }

    public float MaxAngularVelocity
    {
        get { return _maxAngularVelocity; }
        set { _maxAngularVelocity = value; }
    }

    public IAgentState AgentState
    {
        get { return _agentState; }
        set
        {
            DebugUtil.Assert(value != null);
            _agentState = value;
        }
    }

    public KinematicInfo KinematicInfo { get { return _kinematicInfo; } }

    public void Start()
    {
        // Initialise KinematicInfo.
        _kinematicInfo = new KinematicInfo();
        _kinematicInfo.Orientation = 0.0f;
        _kinematicInfo.Position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
        _kinematicInfo.Velocity = Vector3.zero;
        _kinematicInfo.AngularVelocity = 0.0f;
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
        behaviours[key] = pair;
    }

    /// <summary>
    /// Remove a steering behaviour with a given key. 
    /// </summary>
    /// <param name="key"></param>
    public void RemoveBehaviour(string key)
    {
        SteeringPair pair = new SteeringPair();
        if (behaviours.TryGetValue(key,out pair))
        {
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
        if (behaviours.TryGetValue(key, out pair))
        {
            behaviours[key].priority = priority;
        }
    }

    public ISteeringBehaviour GetBehaviour(string key)
    {
        SteeringPair pair = new SteeringPair();
        DebugUtil.Assert(behaviours.TryGetValue(key, out pair));
        return pair.behaviour;
    }

	// Update is called once per frame
    void Update()
    {
        // Allow the agent state to update.
        if (AgentState != null)
        {
            IAgentState nextState;
            AgentState.Update(this, out nextState);
            AgentState = nextState;
        }

        uint[] priorityCounts = new uint[NUM_PRIORITY_LEVELS];
        Vector2[] linearAccelerations = new Vector2[NUM_PRIORITY_LEVELS];
        float[]  angularAccelerations = new float[NUM_PRIORITY_LEVELS];
     
        // Total up all the influences.
        foreach (var pair in behaviours.Values)
        {
            SteeringOutput output =
                pair.behaviour.CalculateAcceleration(gameObject, KinematicInfo);
            priorityCounts[pair.priority] += 1;
            linearAccelerations[pair.priority] += output.Linear;
            angularAccelerations[pair.priority] += output.Angular;
        }

        // We step through each behaviour priority value, until an
        // acceleration greater than epsilon are encountered.
        SteeringOutput acceleration = new SteeringOutput();

        for (uint i = 0; i < NUM_PRIORITY_LEVELS; ++i)
        {
            if (priorityCounts[i] > 0)
            {
                // Blend the accelerations at the current priority level.
                Vector2 linearAveraged = linearAccelerations[i] / 
                    priorityCounts[i];
                float angularAveraged = angularAccelerations[i] / 
                    priorityCounts[i];
                if (linearAveraged.magnitude > EPSILON)
                {
                    acceleration.Linear = linearAveraged;
                    acceleration.Angular = angularAveraged;
                    break;
                } 
            }
        }

        // Scale back the acceleration to the maximum.
        if (acceleration.Linear.magnitude > _maxAcceleration)
        {
            acceleration.Linear /= _maxAcceleration;
        }
        
        // And limit the maximum velocity.
        if (_kinematicInfo.Velocity.magnitude > _maxVelocity)
        {
            _kinematicInfo.Velocity.Normalize();
            _kinematicInfo.Velocity *= _maxVelocity;
        }

        
        // A bit of hight school maths here. Take the starting position u, and the end position as x, then
        // x = u + 1/2at^2 + vt where a is acceleration, v is velocity, and t is time.
        Vector3 motion = 
            new Vector3(_kinematicInfo.Velocity.x, 0.0f, _kinematicInfo.Velocity.y) * Time.deltaTime + 
            new Vector3(acceleration.Linear.x, 0.0f, acceleration.Linear.y) * Time.deltaTime * Time.deltaTime * 0.5f;

        CharacterController controller = gameObject.GetComponent<CharacterController>();
        controller.Move(motion);

        Vector2 facingVector = MotionUtils.GetOrientationAsVector(_kinematicInfo.Orientation);
        transform.LookAt(transform.position + new Vector3(facingVector.x,0.0f,facingVector.y));

        // Update the velocity vectors.
        _kinematicInfo.Velocity += acceleration.Linear * Time.deltaTime;
        _kinematicInfo.AngularVelocity += acceleration.Angular * Time.deltaTime;

        // Update the Orientation and Position values.
        _kinematicInfo.Position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);

        _kinematicInfo.Orientation += acceleration.Angular * Time.deltaTime;
    }
}
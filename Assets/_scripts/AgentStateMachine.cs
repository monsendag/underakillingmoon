using UnityEngine;
using System.Collections;
using System.Reflection;

public class AgentStateMachine
{
	private Agent _agent;
	private AgentStateMachine _currentState;

	protected Agent agent {
		get { return _agent;}
	}

	public AgentStateMachine()
	{

	}

	public AgentStateMachine(Agent agent, AgentStateMachine initialState)
	{
		_agent = agent;
		CurrentState = initialState;
	}

	// Use this for initialization
	void Start()
	{
		if (_currentState != null) { 
			_currentState.Start();
		}

		_currentState.Start();

	}

	public virtual void Update(out AgentStateMachine state)
	{
		state = null;

	}

	// Update is called once per frame
	public void Update()
	{
		if (CurrentState != null) {
			AgentStateMachine nextState;
			CurrentState.Update(out nextState);
			CurrentState = nextState;
		}
	}

	public AgentStateMachine CurrentState {
		get { return _currentState; }
		set {
			if (_currentState != null) {
				SendStateMessage("ExitAction", null);
			}

			DebugUtil.Assert(value != null);

			_currentState = value;

			SendStateMessage("InitAction", null);
		}
	}

	public void SendStateMessage(string functionName, object[] args)
	{
		MethodInfo callback = _currentState.GetType().GetMethod(functionName);
		if (callback != null) {
			callback.Invoke(_currentState, args);
		}

		// recurse the message
		if (_currentState.CurrentState != null) {
			_currentState.CurrentState.SendStateMessage(functionName, args);
		}
	}
}
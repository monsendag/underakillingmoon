using System;
using UnityEngine;

public class WerewolfEvade : AgentState
{
	Agent attacker = null;
    private const float PlayerSearchDistance = 10.0f;
    private const float CoolDownTime = 5.0f;

    private EvadeSteer _evadeSteer = new EvadeSteer();

    private bool _shouldHunt = false;
    private float _countdownTime = 0.0f;

	public void InitAction()
	{
        _evadeSteer.LocalTarget = new KinematicInfo();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _shouldHunt = false;
        _countdownTime = 0.0f;
        attacker = null;

        var werewolf = agent.GetComponent<Werewolf>();
        if (werewolf != null)
        {
            agent.audio.PlayOneShot(werewolf.SquealSound);
        }

        if (player == null)
        {
            _shouldHunt = true;
            return;
        }

        attacker = player.GetComponent<Agent>();
        if (attacker == null)
        {    
            _shouldHunt = true;
            return;
        }

        agent.AddBehaviour("evade", _evadeSteer, 0);
        _evadeSteer.LocalTarget = attacker.KinematicInfo;

	}

	public void ExitAction()
	{
		agent.RemoveBehaviour("evade");
	}

	public override void Update(out Type nextState)
	{
		
		nextState = GetType();

        if (_shouldHunt)
        {
            nextState = typeof(WerewolfHunt);
            return;
        }

        _countdownTime += Time.deltaTime;
        if (agent.distanceTo(attacker) < PlayerSearchDistance)
        {
            _countdownTime = 0.0f;
        }

        if (_countdownTime > CoolDownTime)
        {
            nextState = typeof(WerewolfHunt);
        }
	}
}



using System;
using UnityEngine;
class WerewolfDead : AgentState
{
    FrictionSteer _friction = new FrictionSteer();
    public void InitAction()
    {
        _friction.VelocityFrictionPercentage = 0.9f;
        _friction.AngularVelocityFrictionPercentage = 0.9f;
        agent.AddBehaviour("friction",  _friction, 0);
    }

    public void ExitAction()
    {
        agent.RemoveBehaviour("friction");
    }

    public override void Update(out Type nextState)
    {
        nextState = this.GetType();
    }
}

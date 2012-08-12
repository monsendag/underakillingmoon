using UnityEngine;
using System.Collections;

public class Camper : Agent {

	public override void Start () {
		base.Start ();

        AgentState = new CamperFlock();
	}
	
	public override void Update () {
		base.Update();
	}
}

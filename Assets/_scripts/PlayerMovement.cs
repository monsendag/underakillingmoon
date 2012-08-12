using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{	
	public float speed = 1.0f;
	
	Agent agent;
	FaceSteer face = new FaceSteer();
    FrictionSteer frictionSteer = new FrictionSteer();
    ArriveSteer arriveSteer = new ArriveSteer();
	
	//CharacterController controller;
	
	void Start() 
	{
		agent = gameObject.AddComponent<Agent>();
        DebugUtil.Assert(agent != null);
		
		agent.AddBehaviour("face", face, 0);
        agent.AddBehaviour("friction",frictionSteer,0);
        agent.AddBehaviour("arrive", arriveSteer, 0);
		agent.MaxVelocity = speed;
		
        face.SlowRadius = 0.5f;
        agent.MaxAngularVelocity = 2 * Mathf.PI;

        frictionSteer.VelocityFrictionPercentage = 0.0f;
        frictionSteer.AngularVelocityFrictionPercentage = 0.99f;

        arriveSteer.Target = new KinematicInfo();
        arriveSteer.Target.Position = new Vector2(0.0f, 0.0f);// agent.KinematicInfo.Position;
        arriveSteer.SlowRadius = 1.5f;
        arriveSteer.MaxAcceleration = 4.0f;
        arriveSteer.MaxVelocity = 4.0f;
			//controller = gameObject.GetComponent<CharacterController>();

	}
	
	// Update is called once per frame
	void Update () {
		var mousePosition = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		var vectorised = new Vector2(mousePosition.x, mousePosition.z);
        face.LocalTarget.Position = vectorised;
        Vector2 mousePosDif = vectorised - agent.KinematicInfo.Position;

		//transform.LookAt(mousePosition);
		
        float vert = Input.GetAxis("Vertical");
        if (mousePosDif.magnitude < 1.0f)
        {
            vert = Mathf.Min(0.0f, vert);
        }
		var movementDirection = transform.TransformDirection
            (new Vector3(Input.GetAxis("Horizontal"), 0.0f, vert));
        var movement2D = new Vector2(movementDirection.x, movementDirection.z);

        arriveSteer.Target.Position = agent.KinematicInfo.Position + movement2D;
        

        Vector3 posone = new Vector3(agent.KinematicInfo.Position.x, transform.position.y, agent.KinematicInfo.Position.y);
        Debug.DrawLine(posone, posone + movementDirection, Color.green);
	}
}

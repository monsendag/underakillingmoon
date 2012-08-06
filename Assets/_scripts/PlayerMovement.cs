using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{	
	public float speed = 1.0f;
	
	Agent agent;
	AlignSteer align = new AlignSteer();
	
	//CharacterController controller;
	
	void Start() 
	{
		agent = gameObject.AddComponent<Agent>();
        DebugUtil.Assert(agent != null);
		
		agent.AddBehaviour("align", align, 0);
		agent.MaxAngularVelocity = 1.0f;
		agent.MaxVelocity = speed;
		
		align.MaxAngularAcceleration = 0.5f;
		
			//controller = gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		var mousePosition = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		var vectorised = new Vector2(mousePosition.x, mousePosition.z);

		align.Target.Orientation = MotionUtils.SetOrientationFromVector(vectorised);
		//transform.LookAt(mousePosition);
		
		/*var movementDirection = transform.TransformDirection(
			new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))
		;
		controller.Move( movementDirection * speed * Time.deltaTime);*/
	}
}

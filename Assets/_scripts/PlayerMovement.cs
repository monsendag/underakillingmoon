using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {	
	public float speed = 1.0f;
	
	CharacterController controller;
	
	void Start() {
			controller = gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		var mousePosition = Camera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.y = 0;
		transform.LookAt(mousePosition);
		
		var movementDirection = transform.TransformDirection(
			new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))
		);
		controller.Move( movementDirection * speed * Time.deltaTime);
	}
}

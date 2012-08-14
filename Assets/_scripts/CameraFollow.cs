using System;
using UnityEngine;

class CameraFollow : MonoBehaviour
{
	public GameObject Target;
	public Vector2 Rectangle;

	void Update()
	{
		DebugUtil.Assert(Target != null);

		Vector3 max = transform.position +
			new Vector3(Rectangle.x / 2.0f, 0.0f, Rectangle.y / 2.0f);
		Vector3 min = transform.position -
			new Vector3(Rectangle.x / 2.0f, 0.0f, Rectangle.y / 2.0f);

		Debug.DrawLine(max, min, Color.blue);


		Vector3 closest = new Vector3(
            Mathf.Max(Mathf.Min(max.x, Target.transform.position.x), min.x),
            0.0f,
            Mathf.Max(Mathf.Min(max.z, Target.transform.position.z), min.z));
		Debug.DrawLine(max, closest, Color.yellow);
		Vector3 motion = Target.transform.position - closest;
		motion.y = 0.0f;
		Debug.Log(motion);
		transform.position += motion;
	}
}


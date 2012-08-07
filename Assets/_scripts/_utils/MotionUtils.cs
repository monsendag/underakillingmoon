using UnityEngine;

public class MotionUtils
{
	/// <summary>
	///     Converts a scalar orientation value into a 2d vector.
	/// </summary>
	/// <param name="orientation">
	///     An orientation value. The value is in radians, representing the
	///     angle from (1.0,0.0).
	/// </param>
	/// <returns>
	///     A unit vector representing that angle. An orientation of 0 
	///     corresponds to a unit vector of (1.0f,0.0f)
	/// </returns>
	public static Vector2 GetOrientationAsVector (float orientation)
	{
		Vector2 orientation2;

		orientation2.x = Mathf.Cos (orientation);
		orientation2.y = Mathf.Sin (orientation);
		return orientation2;
	}

	/// <summary>
	///    Convert a 2d orientation value into a scalar. 
	/// </summary>
	/// <param name="orientation2d">
	///     The 2d orientation vector. Should be a unit vector.
	/// </param>
	/// <returns>
    ///     The angle between (1.0f,0.0f) and the orientation vector, in 
    ///     radians. This value ranges from (-PI,PI].
	/// </returns>
	public static float SetOrientationFromVector (Vector2 orientation2d)
	{
		orientation2d.Normalize ();
		float orientation = Mathf.Atan2 (orientation2d.y, orientation2d.x);
		return orientation;
	}

	/// <summary>
	/// Produces a random number in the range -1 to 1, with a binomial 
	/// distribution, (eg, values around 0 are more likely).
	/// </summary>
	public static float RandomBinomial ()
	{
		return Random.Range (0.0f, 1.0f) - Random.Range (0.0f, 1.0f);
	}

	/// <summary>
	/// Maps a radian value to within the range (-PI, PI].
	/// </summary>
	/// <param name="radians">
	///     A value in radians, in the range [-infinity, infinity].    
	/// </param>
	/// <returns>
	///     A radian value with the range (-PI,PI].
	/// </returns>
	public static float MapToRangeRadians (float radians)
	{

		Vector2 orient = GetOrientationAsVector (radians);
		float value = SetOrientationFromVector (orient);
		return value;
	}
	
	/// <summary>
	/// Gets the agents in area.
	/// </summary>
	/// <returns>
	/// The agents in area.
	/// </returns>
	/// <param name='position'>
	/// Position.
	/// </param>
	/// <param name='radius'>
	/// Radius.
	/// </param>
	public static List<Agent> getAgentsInArea (Vector3 position, float radius)
	{
		Collider[] colliders = Physics.OverlapSphere (position, radius);
			
		List<Agent> agents = new List<Agent>();
		Agent agent;
		foreach (var collider in colliders) {
			agent = collider.GetComponent<Agent> ();
			if(agent != null) {
				agents.add(agent);
			}
		}
		return agents;
	}
}
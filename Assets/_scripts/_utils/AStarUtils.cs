using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using System.Linq;

public static class AStarUtils
{
	//have AStarPath calculate the path
	//optional delegate function for when its complete
	public static Path GetPath(Vector2 start, Vector2 end, OnPathDelegate pDel = null)
	{
		Path p = new Path(MotionUtils.To3D(start), MotionUtils.To3D(end), pDel);
		AstarPath.StartPath(p);
		return p;
	}
	
	//same as above, but with Vector3 params
	public static Path GetPath(Vector3 start, Vector3 end, OnPathDelegate pDel = null)
	{
		Path p = new Path(start, end, pDel);	
		AstarPath.StartPath(p);
		return p;
	}	
	
	//get rid of multiple points in straight line
	//then return as list of Vector2's
	public static List<Vector2> FilterPathAsList(Vector3[] p)
	{
		List<Vector3> list = new List<Vector3>();
		
		list.Add(p[0]);
		for (int i = 1; i < (p.Length - 1); ++i) {
			Vector3 a = list[list.Count - 1], b = p[i], c = p[i + 1];
			Vector3 first = b - a, second = c - b;
			if(first.normalized != second.normalized){
				list.Add(b);
			}				
		}
		
		list.Add (p[p.Length - 1]);
		
		return list.Select(v => MotionUtils.To2D(v)).ToList();
	}
}

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class tk2dEditorUtility
{
	public static string CreateNewPrefab(string name) // name is the filename of the prefab EXCLUDING .prefab
	{
		Object obj = Selection.activeObject;
		string assetPath = AssetDatabase.GetAssetPath(obj);
		string dirPrefix = "";
		if (assetPath.Length > 0)
		{
			dirPrefix = Application.dataPath + "/" + assetPath.Substring(7);
			dirPrefix = dirPrefix.Replace('\\', '/');
			if ((File.GetAttributes(dirPrefix) & FileAttributes.Directory) != FileAttributes.Directory)
			{
				for (int i = dirPrefix.Length - 1; i > 0; --i)
				{
					if (dirPrefix[i] == '/')
					{
						dirPrefix = dirPrefix.Substring(0, i);
						break;
					}
				}
			}
			dirPrefix += "/";
		}
		else
		{
			dirPrefix = Application.dataPath + "/";
		}
		
		// find a unique filename
		string fname = name + ".prefab";
		if (File.Exists(dirPrefix + fname))
		{
			for (int i = 0; i < 100; ++i)
			{
				fname = name + i.ToString() + ".prefab";
				if (!File.Exists(dirPrefix + fname))
					break;
			}
		}
		if (File.Exists(dirPrefix + fname))
		{
			EditorUtility.DisplayDialog("Fatal error", "Please rename sprite collections", "Ok");
			return null;
		}
		
        string path = dirPrefix + fname;
		path = path.Substring(Application.dataPath.Length - 6);
			
		return path;
	}
	
	public static T[] FindPrefabsInProjectWithComponent<T>() where T : Component
	// returns null if nothing is found
	{
		List<T> allGens = new List<T>();
		
		Stack<string> paths = new Stack<string>();
		paths.Push(Application.dataPath);
		while (paths.Count != 0)
		{
			string path = paths.Pop();
			string[] files = Directory.GetFiles(path, "*.prefab");
			foreach (var file in files)
			{
				GameObject go = AssetDatabase.LoadAssetAtPath( file.Substring(Application.dataPath.Length - 6), typeof(GameObject) ) as GameObject;
				if (!go) continue;
				
				T gen = go.GetComponent<T>();
				if (gen)
				{
					allGens.Add(gen);
				}
			}
			
			foreach (string subdirs in Directory.GetDirectories(path)) 
				paths.Push(subdirs);
		}
		
		if (allGens.Count == 0) return null;
		
		T[] allGensArray = new T[allGens.Count];
		for (int i = 0; i < allGens.Count; ++i)
			allGensArray[i] = allGens[i];
		return allGensArray;
	}
	
	public static GameObject CreateGameObjectInScene(string name)
	{
		string realName = name;
		int counter = 0;
		while (GameObject.Find(realName) != null)
		{
			realName = name + counter++;
		}
		
        GameObject go = new GameObject(realName);
		if (Selection.activeGameObject != null)
		{
			string assetPath = AssetDatabase.GetAssetPath(Selection.activeGameObject);
			if (assetPath.Length == 0) go.transform.parent = Selection.activeGameObject.transform;
		}
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;	
        return go;
	}
	
	public static void DrawMeshBounds(Mesh mesh, Transform transform, Color c)
	{
		var e = mesh.bounds.extents;
		Vector3[] boundPoints = new Vector3[] {
			mesh.bounds.center + new Vector3(-e.x, e.y, 0.0f),
			mesh.bounds.center + new Vector3( e.x, e.y, 0.0f),
			mesh.bounds.center + new Vector3( e.x,-e.y, 0.0f),
			mesh.bounds.center + new Vector3(-e.x,-e.y, 0.0f),
			mesh.bounds.center + new Vector3(-e.x, e.y, 0.0f) };
		
		for (int i = 0; i < boundPoints.Length; ++i)
			boundPoints[i] = transform.TransformPoint(boundPoints[i]);
		
		Handles.color = c;
		Handles.DrawPolyLine(boundPoints);
	}
}

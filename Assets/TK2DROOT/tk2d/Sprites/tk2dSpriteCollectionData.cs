using UnityEngine;
using System.Collections;

[System.Serializable]
public class tk2dSpriteDefinition
{
	public string name;
	public Vector3[] boundsData;
    public Vector3[] positions;
    public Vector2[] uvs;
    public int[] indices = new int[] { 0, 3, 1, 2, 3, 0 };
}

public class tk2dSpriteCollectionData : MonoBehaviour 
{
    [HideInInspector]
    public tk2dSpriteDefinition[] spriteDefinitions;
    [HideInInspector]
	public Material material;
    [HideInInspector]
    public Texture texture;
    [HideInInspector]
    public bool premultipliedAlpha;
	
    public int Count { get { return spriteDefinitions.Length; } }
}

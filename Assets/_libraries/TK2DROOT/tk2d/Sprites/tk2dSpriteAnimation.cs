using UnityEngine;
using System.Collections;

[System.Serializable]
public class tk2dSpriteAnimationFrame
{
	public tk2dSpriteCollectionData spriteCollection;
	public int spriteId;
	
	// event info
	public bool triggerEvent = false;
	public string eventInfo = "";
	public int eventInt = 0;
	public float eventFloat = 0.0f;
}

[System.Serializable]
public class tk2dSpriteAnimationClip
{
	public string name = "Default";
	public tk2dSpriteAnimationFrame[] frames;
	public float fps = 30.0f;
	public int loopStart = 0;
	public enum WrapMode
	{
		Loop,
		LoopSection,
		Once,
		PingPong,
		Single
	};
	public WrapMode wrapMode = WrapMode.Loop;
}

public class tk2dSpriteAnimation : MonoBehaviour 
{
	public tk2dSpriteAnimationClip[] clips;
	
	public int GetClipIdByName(string name)
	{
		for (int i = 0; i < clips.Length; ++i)
			if (clips[i].name == name) return i;
		return -1;
	}
}

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class tk2dSpriteGeneratorCache
{
	public tk2dSpriteCollection[] all = null;	// all generators
	public tk2dSpriteCollection current = null;	// generators bound to this object
}

[CustomEditor(typeof(tk2dSprite))]
class tk2dSpriteEditor : Editor
{
	tk2dSpriteGeneratorCache generatorCache = null;
	
    public override void OnInspectorGUI()
    {
        tk2dSprite sprite = (tk2dSprite)target;
		if (generatorCache == null)
		{
			generatorCache = new tk2dSpriteGeneratorCache();
		}
		
		DrawSpriteEditorGUI(sprite);
    }
	
	protected void DrawSpriteEditorGUI(tk2dSprite sprite)
	{
		// maybe cache this if its too slow later
		if (generatorCache.all == null || generatorCache.current == null || generatorCache.current.spriteCollection != sprite.collection)
		{
			generatorCache.all = tk2dEditorUtility.FindPrefabsInProjectWithComponent<tk2dSpriteCollection>();
			if (generatorCache.all != null)
			{
				for (int i = 0; i < generatorCache.all.Length; ++i)
				{
					if (generatorCache.all[i].spriteCollection == sprite.collection)
					{
						generatorCache.current = generatorCache.all[i];
						break;
					}
				}
			}
		}
		
		if (generatorCache.all == null)
		{
			EditorGUILayout.LabelField("Collection", "Error");
		}
		else
		{
			string[] collNames = new string[generatorCache.all.Length];
			int selIndex = -1;
			for (int i = 0; i < generatorCache.all.Length; ++i)
			{
				collNames[i] = generatorCache.all[i].name;
				if (generatorCache.all[i] == generatorCache.current)
					selIndex = i;
			}
			
			int newIndex = EditorGUILayout.Popup("Collection", (selIndex != -1) ? selIndex : 0, collNames); 
			if (newIndex != selIndex)
			{
				generatorCache.current = generatorCache.all[newIndex];
				int newId = (sprite.spriteId >= generatorCache.current.spriteCollection.Count)?0:sprite.spriteId;
				sprite.SwitchCollectionAndSprite(generatorCache.current.spriteCollection, newId);
			}
		}
		
        if (sprite.collection)
        {
			// sanity check sprite id
			if (sprite.spriteId < 0 || sprite.spriteId >= sprite.collection.Count)
				sprite.spriteId = 0;
			
            int newSpriteId = sprite.spriteId;
			
			if (generatorCache.current)
			{
				string[] spriteNames = new string[generatorCache.current.textureRefs.Length];
				for (int i = 0; i < generatorCache.current.textureRefs.Length; ++i)
				{
					spriteNames[i] = generatorCache.current.textureParams[i].name;
				}
				
				newSpriteId = EditorGUILayout.Popup("Sprite", sprite.spriteId, spriteNames);
				
				var tex = tk2dSpriteCollectionEditor.GetThumbnailTexture(generatorCache.current, sprite.spriteId);
				if (tex) 
				{
					float w = tex.width;
					float h = tex.height;
					float maxSize = 128.0f;
					if (w > maxSize)
					{
						h = h / w * maxSize;
						w = maxSize;
					}
					
					Rect r = GUILayoutUtility.GetRect(w, h);
					GUI.DrawTexture(r, tex, ScaleMode.ScaleToFit);
					//GUILayout.Box(tex, GUILayout.Width(w), GUILayout.Height(h));
				}
			}
			else
			{
				newSpriteId = EditorGUILayout.IntSlider(sprite.spriteId, 0, sprite.collection.Count - 1);
			}

			if (newSpriteId != sprite.spriteId)
			{
				sprite.spriteId = newSpriteId;
				GUI.changed = true;
			}

            sprite.color = EditorGUILayout.ColorField("Color", sprite.color);
			sprite.scale = EditorGUILayout.Vector3Field("Scale", sprite.scale);
			
			EditorGUILayout.BeginHorizontal();
			
			if (GUILayout.Button("HFlip"))
			{
				Vector3 s = sprite.scale;
				s.x *= -1.0f;
				sprite.scale = s;
				GUI.changed = true;
			}
			if (GUILayout.Button("VFlip"))
			{
				Vector3 s = sprite.scale;
				s.y *= -1.0f;
				sprite.scale = s;
				GUI.changed = true;
			}
			if (GUILayout.Button("Reset Scale" ))
			{
				Vector3 s = sprite.scale;
				s.x = Mathf.Sign(s.x);
				s.y = Mathf.Sign(s.y);
				s.z = Mathf.Sign(s.z);
				sprite.scale = s;
				GUI.changed = true;
			}
			
			if ( GUILayout.Button("Make Pixel Perfect", GUILayout.ExpandWidth(true) ))
			{
				if (tk2dPixelPerfectHelper.inst) tk2dPixelPerfectHelper.inst.Setup();
				sprite.MakePixelPerfect();
				GUI.changed = true;
			}
			
			sprite.pixelPerfect = GUILayout.Toggle(sprite.pixelPerfect, "Always", GUILayout.Width(60.0f));
			EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.IntSlider("Need a collection bound", 0, 0, 1);
        }
		
		if (GUI.changed)
			EditorUtility.SetDirty(sprite);
	}

	
    [MenuItem("GameObject/Create Other/tk2d/Sprite", false, 12900)]
    static void DoCreateSpriteObject()
    {
		tk2dSpriteCollectionData sprColl = null;
		if (sprColl == null)
		{
			// try to inherit from other Sprites in scene
			tk2dSprite spr = GameObject.FindObjectOfType(typeof(tk2dSprite)) as tk2dSprite;
			if (spr)
			{
				sprColl = spr.collection;
			}
		}

		if (sprColl == null)
		{
			tk2dSpriteCollectionData[] data = tk2dEditorUtility.FindPrefabsInProjectWithComponent<tk2dSpriteCollectionData>();
			if (data == null)
			{
				EditorUtility.DisplayDialog("Create Sprite", "Unable to create sprite as no SpriteCollections have been found.", "Ok");
				return;
			}
			
			sprColl = data[0];
		}

		GameObject go = tk2dEditorUtility.CreateGameObjectInScene("Sprite");
		tk2dSprite sprite = go.AddComponent<tk2dSprite>();
		sprite.collection = sprColl;
		sprite.renderer.material = sprColl.material;
		sprite.Build();
    }
}


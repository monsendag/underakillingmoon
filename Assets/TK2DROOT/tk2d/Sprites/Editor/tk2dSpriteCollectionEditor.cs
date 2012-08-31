using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace SCGE 
{
	class SpriteLut
	{
		public int source; // index into source texture list, will only have multiple entries with same source, when splitting
		public Texture2D sourceTex;
		public Texture2D tex; // texture to atlas
		
		public bool isSplit; // is this part of a split?
		public int rx, ry, rw, rh; // split rectangle in texture coords
		
		public bool isDuplicate; // is this a duplicate texture?
		public int atlasIndex; // index in the atlas
	}
	
	class TexturePacker
	{
		public int width, height;
		Rect[] rects;
		int pad;
		
		public TexturePacker(Texture packed, Rect[] _rects, int _pad)
		{
			rects = _rects;
			width = packed.width;
			height = packed.height;
			pad = _pad;
		}
		
		public bool getTextureLocation(int index, ref int x, ref int y, ref int wid, ref int hit)
		{
			x = (int)(rects[index].x * width) + pad;
			y = (int)(rects[index].y * height) + pad;
			wid = (int)(rects[index].width * width) - pad * 2;
			hit = (int)(rects[index].height * height) - pad * 2;
			
			return false;
		}
	}
}

[CustomEditor(typeof(tk2dSpriteCollection))]
public class tk2dSpriteCollectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        tk2dSpriteCollection gen = (tk2dSpriteCollection)target;
        EditorGUILayout.BeginVertical();
		
		bool rebuild = false;
		bool edit = false;
		currentBuild = null;
		
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Commit")) rebuild = true;
		GUILayout.Space(16.0f);
		if (GUILayout.Button("Edit...")) edit = true;
		EditorGUILayout.EndHorizontal();
		
		
		DrawDefaultInspector();
		
		
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Commit")) rebuild = true;
		GUILayout.Space(16.0f);
		if (GUILayout.Button("Edit...")) edit = true;
		EditorGUILayout.EndHorizontal();

		
		if (rebuild) Rebuild(gen);
		if (edit) 
		{
			if (gen.textureRefs != null && gen.textureRefs.Length > 0)
			{
				bool dirty = false;
				if (gen.textureRefs.Length != gen.textureParams.Length) 
				{
					dirty = true;
				}
				if (!dirty)
				{
					for (int i = 0; i < gen.textureRefs.Length; ++i)
					{
						if (gen.textureParams[i].fromSpriteSheet == false && gen.textureRefs[i] != gen.textureParams[i].texture)
						{
							dirty = true;
							break;
						}
					}
				}
				
				if (dirty)
				{
					Rebuild(gen);
				}
				
				tk2dSpriteCollectionEditorPopup v = EditorWindow.GetWindow( typeof(tk2dSpriteCollectionEditorPopup) ) as tk2dSpriteCollectionEditorPopup;
				v.SetGenerator(gen);
			}
		}

        EditorGUILayout.EndVertical();
    }

    public static void RebuildOutOfDate(string[] changedPaths)
    { 
		tk2dSpriteCollection[] scg = tk2dEditorUtility.FindPrefabsInProjectWithComponent<tk2dSpriteCollection>();
		if (scg == null)
			return;
		
        foreach (tk2dSpriteCollection thisScg in scg)
        {
			if (!thisScg.autoUpdate) 
				continue;
			
            bool needRebuild = false;
            foreach (Texture2D tex in thisScg.textureRefs)
            {
                string texPath = AssetDatabase.GetAssetPath(tex);
                foreach (string changedPath in changedPaths)
                {
                    if (changedPath == texPath)
                    {
                        needRebuild = true;
                        break;
                    }
                }

                if (needRebuild)
                    break;
            }
			
			if (thisScg.spriteSheets != null)
			{
	            foreach (var ss in thisScg.spriteSheets)
	            {
	                string texPath = AssetDatabase.GetAssetPath(ss.texture);
	                foreach (string changedPath in changedPaths)
	                {
	                    if (changedPath == texPath)
	                    {
	                        needRebuild = true;
	                        break;
	                    }
	                }
	
	                if (needRebuild)
	                    break;
	            }			
			}
			
            if (needRebuild)
            {
                tk2dSpriteCollectionEditor.Rebuild(thisScg);
            }
        }
    }
	
	static int defaultPad = 2;
	
	static int GetPadAmount(tk2dSpriteCollection gen)
	{
		return (gen.pixelPerfectPointSampled)?0:defaultPad;
	}
	
	static void PadTexture(Texture2D tex, int pad, bool stretchPad)
	{
		Color bgColor = new Color(0,0,0,0);
		
		for (int y = 0; y < pad; ++y)
		{
			for (int x = 0; x < tex.width; ++x)
			{
				tex.SetPixel(x, y, stretchPad?tex.GetPixel(x, pad):bgColor);
				tex.SetPixel(x, tex.height - 1 - y, stretchPad?tex.GetPixel(x, tex.height - 1 - pad):bgColor);
			}
		}
		for (int x = 0; x < pad; ++x)
		{
			for (int y = 0; y < tex.height; ++y)
			{
				tex.SetPixel(x, y, stretchPad?tex.GetPixel(pad, y):bgColor);
				tex.SetPixel(tex.width - 1 - x, y, stretchPad?tex.GetPixel(tex.width - 1 - pad, y):bgColor);
			}
		}
	}
	
    static int NameCompare(string na, string nb)
    {
		if (na.Length == 0 && nb.Length != 0) return 1;
		else if (na.Length != 0 && nb.Length == 0) return -1;
		else if (na.Length == 0 && nb.Length == 0) return 0;
		
        int numStartA = na.Length - 1;

        // last char is not a number, compare as regular strings
        if (na[numStartA] < '0' || na[numStartA] > '9')
            return System.String.Compare(na, nb, true);

        while (numStartA > 0 && na[numStartA - 1] >= '0' && na[numStartA - 1] <= '9')
            numStartA--;

        int comp = System.String.Compare(na, 0, nb, 0, numStartA);
        
        if (comp == 0)
        {
            if (nb.Length > numStartA)
            {
                bool numeric = true;
                for (int i = numStartA; i < nb.Length; ++i)
                {
                    if (nb[i] < '0' || nb[i] > '9')
                    {
                        numeric = false;
                        break;
                    }
                }

                if (numeric)
                {
                    int numA = System.Convert.ToInt32(na.Substring(numStartA));
                    int numB = System.Convert.ToInt32(nb.Substring(numStartA));
                    return numA - numB;
                }
            }
        }

        return System.String.Compare(na, nb);
    }
	
	static void FixUpParams(tk2dSpriteCollection gen)
	{
		if (gen.textureRefs != null && gen.textureParams != null && gen.textureRefs.Length != gen.textureParams.Length)
        {
			tk2dSpriteCollectionDefinition[] newDefs = new tk2dSpriteCollectionDefinition[gen.textureRefs.Length];
			int c = Mathf.Min( newDefs.Length, gen.textureParams.Length );
			
			if (gen.textureRefs.Length > gen.textureParams.Length)
			{
				Texture2D[] newTexRefs = new Texture2D[gen.textureRefs.Length - gen.textureParams.Length];
				System.Array.Copy(gen.textureRefs, gen.textureParams.Length, newTexRefs, 0, newTexRefs.Length);
				System.Array.Sort(newTexRefs, (Texture2D a, Texture2D b) => NameCompare(a?a.name:"", b?b.name:""));
				System.Array.Copy(newTexRefs, 0, gen.textureRefs, gen.textureParams.Length, newTexRefs.Length);
			}
			
			for (int i = 0; i < c; ++i)
			{
				newDefs[i] = new tk2dSpriteCollectionDefinition();
				newDefs[i].CopyFrom( gen.textureParams[i] );
			}
			for (int i = c; i < newDefs.Length; ++i)
			{
				newDefs[i] = new tk2dSpriteCollectionDefinition();
			}
			gen.textureParams = newDefs;
        }
		
		// clear thumbnails on build
		foreach (var param in gen.textureParams)
		{
			param.thumbnailTexture = null;
		}
	}
	
	static void SetUpTextureFormats(tk2dSpriteCollection gen)
	{
		// make sure all textures are in the right format
		int numTexturesReimported = 0;
		List<Texture2D> texturesToProcess = new List<Texture2D>();
		
		for (int i = 0; i < gen.textureParams.Length; ++i)
		{
			if (gen.textureRefs[i] != null)
			{
				texturesToProcess.Add(gen.textureRefs[i]);
			}
		}
		if (gen.spriteSheets != null)
		{
			for (int i = 0; i < gen.spriteSheets.Length; ++i)
			{
				if (gen.spriteSheets[i].texture != null)
				{
					texturesToProcess.Add(gen.spriteSheets[i].texture);
				}
			}
		}
		foreach (var tex in texturesToProcess)
		{
			// make sure the source texture is npot and readable, and uncompressed
			string thisTextPath = AssetDatabase.GetAssetPath(tex);
            TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(thisTextPath);
            if (importer.textureType != TextureImporterType.Advanced ||
                importer.textureFormat != TextureImporterFormat.AutomaticTruecolor ||
                importer.npotScale != TextureImporterNPOTScale.None ||
                importer.isReadable != true ||
			    importer.maxTextureSize < 4096)
            {
                importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                importer.textureType = TextureImporterType.Advanced;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.isReadable = true;
				importer.mipmapEnabled = false;
				importer.maxTextureSize = 4096;
				
                AssetDatabase.ImportAsset(thisTextPath);

				numTexturesReimported++;
            }
		}
		if (numTexturesReimported > 0)
		{
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}		
	}

	static Texture2D ProcessTexture(bool premultipliedAlpha, bool additive, bool stretchPad, Texture2D srcTex, int sx, int sy, int tw, int th, ref SCGE.SpriteLut spriteLut, int padAmount)
	{
		// Can't have additive without premultiplied alpha
		if (!premultipliedAlpha) additive = false;
		
		int[] ww = new int[tw];
		int[] hh = new int[th];
		for (int x = 0; x < tw; ++x) ww[x] = 0;
		for (int y = 0; y < th; ++y) hh[y] = 0;
		int numNotTransparent = 0;
		for (int x = 0; x < tw; ++x)
		{
			for (int y = 0; y < th; ++y)
			{
				Color col = srcTex.GetPixel(sx + x, sy + y);
				if (col.a > 0)
				{
					ww[x] = 1;
					hh[y] = 1;
					numNotTransparent++;
				}
			}
		}
		
		if (numNotTransparent > 0)
		{
			int x0 = 0, x1 = 0, y0 = 0, y1 = 0;
			for (int x = 0; x < tw; ++x) if (ww[x] == 1) { x0 = x; break; }
			for (int x = tw - 1; x >= 0; --x) if (ww[x] == 1) { x1 = x; break; }
			for (int y = 0; y < th; ++y) if (hh[y] == 1) { y0 = y; break; }
			for (int y = th - 1; y >= 0; --y) if (hh[y] == 1) { y1 = y; break; }
			
			int w1 = x1 - x0 + 1;
			int h1 = y1 - y0 + 1;
			
			Texture2D dtex = new Texture2D(w1 + padAmount * 2, h1 + padAmount * 2);
			for (int x = 0; x < w1; ++x)
			{
				for (int y = 0; y < h1; ++y)
				{
					Color col = srcTex.GetPixel(sx + x0 + x, sy + y0 + y);
					dtex.SetPixel(x + padAmount, y + padAmount, col);
				}
			}
			PadTexture(dtex, padAmount, stretchPad);
			
			if (premultipliedAlpha)
			{
				for (int x = 0; x < dtex.width; ++x)			
				{
					for (int y = 0; y < dtex.height; ++y)
					{
						Color col = dtex.GetPixel(x, y);
                        col.r *= col.a; col.g *= col.a; col.b *= col.a;
						col.a = additive?0.0f:col.a;
						dtex.SetPixel(x, y, col);
					}
				}
			}
			
			dtex.Apply();
		
			spriteLut.rx = sx + x0;
			spriteLut.ry = sy + y0;
			spriteLut.rw = w1;
			spriteLut.rh = h1;
			spriteLut.tex = dtex;
			
			return dtex;
		}
		else
		{
			return null;
		}
	}
	
	static void TrimTextureList(tk2dSpriteCollection gen)
	{
		// trim textureRefs & textureParams
		int lastNonEmpty = -1;
		for (int i = 0; i < gen.textureRefs.Length; ++i)
		{
			if (gen.textureRefs[i] != null) lastNonEmpty = i;
		}
		System.Array.Resize(ref gen.textureRefs, lastNonEmpty + 1);
		System.Array.Resize(ref gen.textureParams, lastNonEmpty + 1);		
	}
	
	static bool SetUpSpriteSheets(tk2dSpriteCollection gen)
	{
		// delete textures which aren't in sprite sheets any more
		// and delete textures which are out of range of the spritesheet
		for (int i = 0; i < gen.textureRefs.Length; ++i)
		{
			if (gen.textureParams[i].fromSpriteSheet)
			{
				bool found = false;
				foreach (var ss in gen.spriteSheets)
				{
					if (gen.textureRefs[i] == ss.texture)
					{
						found = true;
						int numTiles = (ss.numTiles == 0)?(ss.tilesX * ss.tilesY):Mathf.Min(ss.numTiles, ss.tilesX * ss.tilesY);
						// delete textures which are out of range
						if (gen.textureParams[i].regionId >= numTiles)
						{
							gen.textureRefs[i] = null;
							gen.textureParams[i].fromSpriteSheet = false;
							gen.textureParams[i].extractRegion = false;
						}
					}
				}
				
				if (!found)
				{
					gen.textureRefs[i] = null;
					gen.textureParams[i].fromSpriteSheet = false;
					gen.textureParams[i].extractRegion = false;
				}
			}
		}
		
		foreach (var ss in gen.spriteSheets)
		{
			// Sanity check
			if (ss.texture == null)
			{
				continue; // deleted, safely ignore this
			}
			if (ss.tilesX * ss.tilesY == 0 ||
			    (ss.numTiles != 0 && ss.numTiles > ss.tilesX * ss.tilesY))
			{
				EditorUtility.DisplayDialog("Invalid sprite sheet", 
				                            "Sprite sheet '" + ss.texture.name + "' has an invalid number of tiles",
				                            "Ok");
				return false;
			}
			if ((ss.texture.width % ss.tilesX) != 0 || (ss.texture.height % ss.tilesY) != 0)
			{
				EditorUtility.DisplayDialog("Invalid sprite sheet", 
				                            "Sprite sheet '" + ss.texture.name + "' doesn't match tile count",
				                            "Ok");
				return false;
			}
			
			int numTiles = (ss.numTiles == 0)?(ss.tilesX * ss.tilesY):Mathf.Min(ss.numTiles, ss.tilesX * ss.tilesY);
			
			for (int y = 0; y < ss.tilesY; ++y)
			{
				for (int x = 0; x < ss.tilesX; ++x)
				{
					// limit to number of tiles, if told to
					int tileIdx = y * ss.tilesX + x;
					if (tileIdx >= numTiles)
						break;
					
					// find texture in collection
					int textureIdx = -1;
					for (int i = 0; i < gen.textureParams.Length; ++i)
					{
						if (gen.textureParams[i].fromSpriteSheet 
						    && gen.textureParams[i].regionId == tileIdx
						    && gen.textureRefs[i] == ss.texture)
						{
							textureIdx = i;
							break;
						}
					}
					
					if (textureIdx == -1)
					{
						// find first empty texture slot
						for (int i = 0; i < gen.textureParams.Length; ++i)
						{
							if (gen.textureRefs[i] == null)
							{
								textureIdx = i;
								break;
							}
						}						
					}
					
					if (textureIdx == -1)
					{
						// texture not found, so extend arrays
						System.Array.Resize(ref gen.textureRefs, gen.textureRefs.Length + 1);
						System.Array.Resize(ref gen.textureParams, gen.textureParams.Length + 1);
						textureIdx = gen.textureRefs.Length - 1;
					}
					
					gen.textureRefs[textureIdx] = ss.texture;
					var param = new tk2dSpriteCollectionDefinition();
					param.fromSpriteSheet = true;
					param.name = ss.texture.name + "/" + tileIdx;
					param.regionId = tileIdx;
					param.regionW = ss.texture.width / ss.tilesX;
					param.regionH = ss.texture.height / ss.tilesY;
					param.regionX = (tileIdx % ss.tilesX) * param.regionW;
					param.regionY = (ss.tilesY - 1 - (tileIdx / ss.tilesX)) * param.regionH;
					param.extractRegion = true;
					
					param.pad = ss.pad;
					param.anchor = (tk2dSpriteCollectionDefinition.Anchor)ss.anchor;
					param.scale = (ss.scale.sqrMagnitude == 0.0f)?Vector3.one:ss.scale;
					
					gen.textureParams[textureIdx] = param;
				}
			}
		}
			
		return true;
	}
	
	
	static tk2dSpriteCollection currentBuild = null;
	static Texture2D[] sourceTextures;
	
	public static Texture2D GetThumbnailTexture(tk2dSpriteCollection gen, int spriteId)
	{
		var param = gen.textureParams[spriteId];
		if (gen.textureParams[spriteId].thumbnailTexture == null && gen.textureRefs[spriteId] != null)
		{
			if (param.extractRegion)
			{
				Texture2D localTex = new Texture2D(param.regionW, param.regionH);
				for (int y = 0; y < param.regionH; ++y)
				{
					for (int x = 0; x < param.regionW; ++x)
					{
						localTex.SetPixel(x, y, gen.textureRefs[spriteId].GetPixel(param.regionX + x, param.regionY + y));
					}
				}
				localTex.Apply();
				param.thumbnailTexture = localTex;
			}
			else
			{
				param.thumbnailTexture = gen.textureRefs[spriteId];
			}
		}
		
		return param.thumbnailTexture;
	}
	
    public static void Rebuild(tk2dSpriteCollection gen)
    {
		// avoid "recursive" build being triggered by texture watcher
		if (currentBuild == gen)
			return;
		
		currentBuild = gen;
		
        string path = AssetDatabase.GetAssetPath(gen);
		string subDirName = Path.GetDirectoryName( path.Substring(7) );
		if (subDirName.Length > 0) subDirName += "/";
		
		string dataDirFullPath = Application.dataPath + "/" + subDirName + Path.GetFileNameWithoutExtension(path) + "_Data";
		string dataDirName = "Assets/" + dataDirFullPath.Substring( Application.dataPath.Length + 1 ) + "/";
		
		if (gen.atlasTexture == null || gen.defaultMaterial == null || gen.spriteCollection == null)
		{
			if (!Directory.Exists(dataDirFullPath)) Directory.CreateDirectory(dataDirFullPath);
			AssetDatabase.Refresh();
		}
		
		string texturePath = gen.atlasTexture?AssetDatabase.GetAssetPath(gen.atlasTexture):(dataDirName + "atlas.png");
		string materialPath = gen.defaultMaterial?AssetDatabase.GetAssetPath(gen.defaultMaterial):(dataDirName + "default.mat");
		string prefabObjectPath = gen.spriteCollection?AssetDatabase.GetAssetPath(gen.spriteCollection):(dataDirName + "data.prefab");
		
      	FixUpParams(gen);
		
		SetUpTextureFormats(gen);
		
		SetUpSpriteSheets(gen);
		
		TrimTextureList(gen);
		
		// blank texture used when texture has been deleted
		Texture2D blankTexture = new Texture2D(2, 2);
		blankTexture.SetPixel(0, 0, Color.magenta);
		blankTexture.SetPixel(0, 1, Color.yellow);
		blankTexture.SetPixel(1, 0, Color.cyan);
		blankTexture.SetPixel(1, 1, Color.grey);
		blankTexture.Apply();
		
		// make local texture sources
		sourceTextures = new Texture2D[gen.textureRefs.Length];
		for (int i = 0; i < gen.textureParams.Length; ++i)
		{
			var param = gen.textureParams[i];
			if (param.extractRegion && gen.textureRefs[i] != null)
			{
				Texture2D localTex = new Texture2D(param.regionW, param.regionH);
				for (int y = 0; y < param.regionH; ++y)
				{
					for (int x = 0; x < param.regionW; ++x)
					{
						localTex.SetPixel(x, y, gen.textureRefs[i].GetPixel(param.regionX + x, param.regionY + y));
					}
				}
				localTex.name = gen.textureRefs[i].name + "/" + param.regionId.ToString();
				localTex.Apply();
				sourceTextures[i] = localTex;
			}
			else
			{
				sourceTextures[i] = gen.textureRefs[i];
			}
		}
		
		// catalog all textures to atlas
		int numTexturesToAtlas = 0;
		List<SCGE.SpriteLut> spriteLuts = new List<SCGE.SpriteLut>();
		for (int i = 0; i < gen.textureParams.Length; ++i)
		{
			Texture2D currentTexture = sourceTextures[i];
			
			if (sourceTextures[i] == null)
			{
				gen.textureParams[i].dice = false;
				gen.textureParams[i].anchor = tk2dSpriteCollectionDefinition.Anchor.MiddleCenter;
				gen.textureParams[i].name = "";
				gen.textureParams[i].extractRegion = false;
				gen.textureParams[i].fromSpriteSheet = false;
				
				currentTexture = blankTexture;
			}
			else
			{
				if (gen.textureParams[i].name == null || gen.textureParams[i].name == "" || gen.textureParams[i].texture != currentTexture) 
					gen.textureParams[i].name = currentTexture.name;
			}
			
			gen.textureParams[i].texture = currentTexture;
			
			
			if (gen.textureParams[i].dice)
			{
				// prepare to dice this up
				int diceUnitX = gen.textureParams[i].diceUnitX;
				int diceUnitY = gen.textureParams[i].diceUnitY;
				if (diceUnitX <= 0) diceUnitX = 128; // something sensible, please
				if (diceUnitY <= 0) diceUnitY = diceUnitX; // make square if not set
				
				Texture2D srcTex = currentTexture;
				for (int sx = 0; sx < srcTex.width; sx += diceUnitX)
				{
					for (int sy = 0; sy < srcTex.height; sy += diceUnitY)
					{
						int tw = Mathf.Min(diceUnitX, srcTex.width - sx);
						int th = Mathf.Min(diceUnitY, srcTex.height - sy);
				
						SCGE.SpriteLut diceLut = new SCGE.SpriteLut();
						diceLut.source = i;
						diceLut.isSplit = true;
						diceLut.sourceTex = srcTex;
						diceLut.isDuplicate = false; // duplicate diced textures can be chopped up differently, so don't detect dupes here

						Texture2D dest = ProcessTexture(gen.premultipliedAlpha, gen.textureParams[i].additive, true, srcTex, sx, sy, tw, th, ref diceLut, GetPadAmount(gen));
						if (dest)
						{
							diceLut.atlasIndex = numTexturesToAtlas++;
							spriteLuts.Add(diceLut);
						}
					}
				}
			}
			else			
			{
				SCGE.SpriteLut lut = new SCGE.SpriteLut();
				lut.sourceTex = currentTexture;
				lut.source = i;
				
				lut.isSplit = false;
				lut.isDuplicate = false;
				for (int j = 0; j < spriteLuts.Count; ++j)
				{
					if (spriteLuts[j].sourceTex == lut.sourceTex)
					{
						lut.isDuplicate = true;
						lut.atlasIndex = spriteLuts[j].atlasIndex;
						lut.tex = spriteLuts[j].tex; // get old processed tex
						
						lut.rx = spriteLuts[j].rx; lut.ry = spriteLuts[j].ry; 
						lut.rw = spriteLuts[j].rw; lut.rh = spriteLuts[j].rh; 
						
						break;
					}
				}
				
				if (!lut.isDuplicate) 
				{
					lut.atlasIndex = numTexturesToAtlas++;
					bool stretchPad = false;
					if (gen.textureParams[i].pad == tk2dSpriteCollectionDefinition.Pad.Extend) stretchPad = true;
					
					Texture2D dest = ProcessTexture(gen.premultipliedAlpha, gen.textureParams[i].additive, stretchPad, currentTexture, 0, 0, currentTexture.width, currentTexture.height, ref lut, GetPadAmount(gen));
					if (dest == null)
					{
						// fall back to a tiny blank texture
						lut.tex = new Texture2D(1, 1);
						lut.tex.SetPixel(0, 0, new Color( 0, 0, 0, 0 ));
						PadTexture(lut.tex, GetPadAmount(gen), stretchPad);
						lut.tex.Apply();
						
						lut.rx = currentTexture.width / 2; lut.ry = currentTexture.height / 2;
						lut.rw = 1; lut.rh = 1;
					}
				}
				
				spriteLuts.Add(lut);
			}
		}
				
        // Create texture
        Texture2D tex = new Texture2D(64, 64, TextureFormat.ARGB32, false);
		Texture2D[] textureList = new Texture2D[numTexturesToAtlas];
        int titer = 0;
        for (int i = 0; i < spriteLuts.Count; ++i)
        {
			SCGE.SpriteLut _lut = spriteLuts[i];
			if (!_lut.isDuplicate)
			{
				textureList[titer++] = _lut.tex;
			}
        }
		Rect[] atlasRects = tex.PackTextures(textureList, 0); // padding is handled explicitly earlier on
		SCGE.TexturePacker tp = new SCGE.TexturePacker(tex, atlasRects, GetPadAmount(gen));
	
		byte[] bytes = tex.EncodeToPNG();
		System.IO.FileStream fs = System.IO.File.Create(texturePath);
		fs.Write(bytes, 0, bytes.Length);
		fs.Close();
		
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		tex = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D)) as Texture2D;
		gen.atlasTexture = tex;

		// display warning if texture exceeds MaxSize (i.e. final texture has been downsampled)
		if (tp.width > tex.width || tp.height > tex.height)
		{
			EditorUtility.DisplayDialog("Atlas texture too large", 
			                            "Consider breaking it up into multiple SpriteCollections,\n" +
			                            "or increase the MaxSize parameter on the texture\n" +
			                            "Texture will otherwise be downsampled.",
			                            "Ok");
		}		
		
		
        // Create material
        if (gen.defaultMaterial == null)
        {
			Material mat;
            if (gen.premultipliedAlpha)
                mat = new Material(Shader.Find("tk2d/PremulVertexColor"));
            else
                mat = new Material(Shader.Find("tk2d/BlendVertexColor"));
            mat.mainTexture = tex;
            AssetDatabase.CreateAsset(mat, materialPath);
			AssetDatabase.SaveAssets();

			gen.defaultMaterial = AssetDatabase.LoadAssetAtPath(materialPath, typeof(Material)) as Material;
		}
		

        // Create prefab
		if (gen.spriteCollection == null)
		{
			Object p = PrefabUtility.CreateEmptyPrefab(prefabObjectPath);
			GameObject go = new GameObject();
			go.AddComponent<tk2dSpriteCollectionData>();
			PrefabUtility.ReplacePrefab(go, p);
			DestroyImmediate(go);
			AssetDatabase.SaveAssets();

			gen.spriteCollection = AssetDatabase.LoadAssetAtPath(prefabObjectPath, typeof(tk2dSpriteCollectionData)) as tk2dSpriteCollectionData;
		}
		
		// Make sure texture is set to point filtered when no pad mode is selected
		if (gen.pixelPerfectPointSampled && tex.filterMode != FilterMode.Point)
		{
			TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath( AssetDatabase.GetAssetPath(tex) );
			importer.filterMode = FilterMode.Point;
			EditorUtility.SetDirty(importer);
		}
		
		tk2dSpriteCollectionData coll = gen.spriteCollection;
        coll.texture = tex;
        coll.material = gen.defaultMaterial;
        coll.premultipliedAlpha = gen.premultipliedAlpha;
        coll.spriteDefinitions = new tk2dSpriteDefinition[gen.textureParams.Length];
        UpdateVertexCache(gen, tp, coll, spriteLuts);
		
        // refresh existing
        tk2dSprite[] sprs = Resources.FindObjectsOfTypeAll(typeof(tk2dSprite)) as tk2dSprite[];
        foreach (tk2dSprite spr in sprs)
        {
			if (spr.collection == gen.spriteCollection)
			{
				if (spr.spriteId < 0 || spr.spriteId >= spr.collection.spriteDefinitions.Length)
            		spr.spriteId = 0;
				
				spr.Build();
			}
        }
		tk2dStaticSpriteBatcher[] batchedSprs = Resources.FindObjectsOfTypeAll(typeof(tk2dStaticSpriteBatcher)) as tk2dStaticSpriteBatcher[];
		foreach (var spr in batchedSprs)
		{
			if (spr.spriteCollection == gen.spriteCollection)
			{
				spr.Build();
			}
		}
		
		// save changes
		EditorUtility.SetDirty(gen.spriteCollection);
		EditorUtility.SetDirty(gen);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	
		sourceTextures = null; // need to clear, its static
		currentBuild = null;
    }

    static void UpdateVertexCache(tk2dSpriteCollection gen, SCGE.TexturePacker packer, tk2dSpriteCollectionData coll, List<SCGE.SpriteLut> spriteLuts)
    {
        float fwidth = packer.width;
        float fheight = packer.height;
        float scale = 2.0f * gen.targetOrthoSize / gen.targetHeight;

        for (int i = 0; i < sourceTextures.Length; ++i)
        {
			SCGE.SpriteLut _lut = null;
			for (int j = 0; j < spriteLuts.Count; ++j)
			{
				if (spriteLuts[j].source == i)
				{
					_lut = spriteLuts[j];
					break;
				}
			}
			
            var thisTexParam = gen.textureParams[i];
            int tx = 0, ty = 0, tw = 0, th = 0;
            bool flipped = packer.getTextureLocation(_lut.atlasIndex, ref tx, ref ty, ref tw, ref th);
            int sd_y = packer.height - ty - th;
			
			float uvOffsetX = 0.001f / fwidth;
			float uvOffsetY = 0.001f / fheight;
			
            Vector2 v0 = new Vector2(tx / fwidth + uvOffsetX, 1.0f - (sd_y + th) / fheight + uvOffsetY);
            Vector2 v1 = new Vector2((tx + tw) / fwidth - uvOffsetX, 1.0f - sd_y / fheight - uvOffsetY);

            Mesh mesh = null;
            Transform meshTransform = null;
            GameObject instantiated = null;

            if (thisTexParam.overrideMesh)
            {
				// Disabled
                instantiated = Instantiate(thisTexParam.overrideMesh) as GameObject;
                MeshFilter meshFilter = instantiated.GetComponentInChildren<MeshFilter>();
                if (meshFilter == null)
                {
                    Debug.LogError("Unable to find mesh");
                    DestroyImmediate(instantiated);
                }
                else
                {
                    mesh = meshFilter.sharedMesh;
                    meshTransform = meshFilter.gameObject.transform;
                }
            }

            if (mesh)
            {
                coll.spriteDefinitions[i] = new tk2dSpriteDefinition();
                coll.spriteDefinitions[i].positions = new Vector3[mesh.vertices.Length];
                coll.spriteDefinitions[i].uvs = new Vector2[mesh.vertices.Length];
                for (int j = 0; j < mesh.vertices.Length; ++j)
                {
                    coll.spriteDefinitions[i].positions[j] = meshTransform.TransformPoint(mesh.vertices[j]);
                    coll.spriteDefinitions[i].uvs[j] = new Vector2(v0.x + (v1.x - v0.x) * mesh.uv[j].x, v0.y + (v1.y - v0.y) * mesh.uv[j].y);
                }
                coll.spriteDefinitions[i].indices = new int[mesh.triangles.Length];
                for (int j = 0; j < mesh.triangles.Length; ++j)
                {
                    coll.spriteDefinitions[i].indices[j] = mesh.triangles[j];
                }

                DestroyImmediate(instantiated);
            }
            else
            {
				Texture2D thisTextureRef = sourceTextures[i];
				
				float texHeight = thisTextureRef?thisTextureRef.height:2;
       			float texWidth = thisTextureRef?thisTextureRef.width:2;
				
				float h = thisTextureRef?thisTextureRef.height:64;
				float w = thisTextureRef?thisTextureRef.width:64;
                h *= thisTexParam.scale.x;
                w *= thisTexParam.scale.y;

				float scaleX = w * scale;
                float scaleY = h * scale;
				
                Vector3 pos0 = new Vector3(-0.5f * scaleX, 0, -0.5f * scaleY);
                switch (thisTexParam.anchor)
                {
                    case tk2dSpriteCollectionDefinition.Anchor.LowerLeft: pos0 = new Vector3(0, 0, 0); break;
                    case tk2dSpriteCollectionDefinition.Anchor.LowerCenter: pos0 = new Vector3(-0.5f * scaleX, 0, 0); break;
                    case tk2dSpriteCollectionDefinition.Anchor.LowerRight: pos0 = new Vector3(-scaleX, 0, 0); break;

                    case tk2dSpriteCollectionDefinition.Anchor.MiddleLeft: pos0 = new Vector3(0, 0, -0.5f * scaleY); break;
                    case tk2dSpriteCollectionDefinition.Anchor.MiddleCenter: pos0 = new Vector3(-0.5f * scaleX, 0, -0.5f * scaleY); break;
                    case tk2dSpriteCollectionDefinition.Anchor.MiddleRight: pos0 = new Vector3(-scaleX, 0, -0.5f * scaleY); break;

                    case tk2dSpriteCollectionDefinition.Anchor.UpperLeft: pos0 = new Vector3(0, 0, -scaleY); break;
                    case tk2dSpriteCollectionDefinition.Anchor.UpperCenter: pos0 = new Vector3(-0.5f * scaleX, 0, -scaleY); break;
                    case tk2dSpriteCollectionDefinition.Anchor.UpperRight: pos0 = new Vector3(-scaleX, 0, -scaleY); break;

                    case tk2dSpriteCollectionDefinition.Anchor.Custom:
                        {
                            pos0 = new Vector3(-thisTexParam.anchorX * thisTexParam.scale.x * scale, 0, -(h - thisTexParam.anchorY * thisTexParam.scale.y) * scale);
                        }
                        break;
                }

                Vector3 pos1 = pos0 + new Vector3(scaleX, 0, scaleY);
				
				List<Vector3> positions = new List<Vector3>();
				List<Vector2> uvs = new List<Vector2>();
				
				// build mesh
				if (_lut.isSplit)
				{
					for (int j = 0; j < spriteLuts.Count; ++j)
					{
						if (spriteLuts[j].source == i)
						{
							_lut = spriteLuts[j];

				            flipped = packer.getTextureLocation(_lut.atlasIndex, ref tx, ref ty, ref tw, ref th);
				            sd_y = packer.height - ty - th;
				            v0 = new Vector2(tx / fwidth + uvOffsetX, 1.0f - (sd_y + th) / fheight + uvOffsetY);
				            v1 = new Vector2((tx + tw) / fwidth - uvOffsetX, 1.0f - sd_y / fheight - uvOffsetY);
						
							float x0 = _lut.rx / texWidth;
							float y0 = _lut.ry / texHeight;
							float x1 = (_lut.rx + _lut.rw) / texWidth;
							float y1 = (_lut.ry + _lut.rh) / texHeight;
							
							Vector3 dpos0 = new Vector3(Mathf.Lerp(pos0.x, pos1.x, x0), 0.0f, Mathf.Lerp(pos0.z, pos1.z, y0));
							Vector3 dpos1 = new Vector3(Mathf.Lerp(pos0.x, pos1.x, x1), 0.0f, Mathf.Lerp(pos0.z, pos1.z, y1));
							
							positions.Add(new Vector3(dpos0.x, dpos0.z, 0));
							positions.Add(new Vector3(dpos1.x, dpos0.z, 0));
							positions.Add(new Vector3(dpos0.x, dpos1.z, 0));
							positions.Add(new Vector3(dpos1.x, dpos1.z, 0));
							
			                if (flipped)
			                {
			                    uvs.Add(new Vector2(v0.x,v0.y));
			                    uvs.Add(new Vector2(v0.x,v1.y));
			                    uvs.Add(new Vector2(v1.x,v0.y));
			                    uvs.Add(new Vector2(v1.x,v1.y));
			                }
			                else
			                {
			                    uvs.Add(new Vector2(v0.x,v0.y));
			                    uvs.Add(new Vector2(v1.x,v0.y));
			                    uvs.Add(new Vector2(v0.x,v1.y));
			                    uvs.Add(new Vector2(v1.x,v1.y));
			                }
						}
					}
				}
				else
				{
					float x0 = _lut.rx / texWidth;
					float y0 = _lut.ry / texHeight;
					float x1 = (_lut.rx + _lut.rw) / texWidth;
					float y1 = (_lut.ry + _lut.rh) / texHeight;					
					
					Vector3 dpos0 = new Vector3(Mathf.Lerp(pos0.x, pos1.x, x0), 0.0f, Mathf.Lerp(pos0.z, pos1.z, y0));
					Vector3 dpos1 = new Vector3(Mathf.Lerp(pos0.x, pos1.x, x1), 0.0f, Mathf.Lerp(pos0.z, pos1.z, y1));
					
					positions.Add(new Vector3(dpos0.x, dpos0.z, 0));
					positions.Add(new Vector3(dpos1.x, dpos0.z, 0));
					positions.Add(new Vector3(dpos0.x, dpos1.z, 0));
					positions.Add(new Vector3(dpos1.x, dpos1.z, 0));
					
	                if (flipped)
	                {
	                    uvs.Add(new Vector2(v0.x,v0.y));
	                    uvs.Add(new Vector2(v0.x,v1.y));
	                    uvs.Add(new Vector2(v1.x,v0.y));
	                    uvs.Add(new Vector2(v1.x,v1.y));
	                }
	                else
	                {
	                    uvs.Add(new Vector2(v0.x,v0.y));
	                    uvs.Add(new Vector2(v1.x,v0.y));
	                    uvs.Add(new Vector2(v0.x,v1.y));
	                    uvs.Add(new Vector2(v1.x,v1.y));
	                }
				}	

				// build sprite definition
				if (coll.spriteDefinitions[i] == null)
                	coll.spriteDefinitions[i] = new tk2dSpriteDefinition();
				coll.spriteDefinitions[i].indices = new int[ 6 * (positions.Count / 4) ];
				for (int j = 0; j < positions.Count / 4; ++j)
				{
	                coll.spriteDefinitions[i].indices[j * 6 + 0] = j * 4 + 0;
					coll.spriteDefinitions[i].indices[j * 6 + 1] = j * 4 + 3; 
					coll.spriteDefinitions[i].indices[j * 6 + 2] = j * 4 + 1; 
					coll.spriteDefinitions[i].indices[j * 6 + 3] = j * 4 + 2; 
					coll.spriteDefinitions[i].indices[j * 6 + 4] = j * 4 + 3; 
					coll.spriteDefinitions[i].indices[j * 6 + 5] = j * 4 + 0;
				}
				
				coll.spriteDefinitions[i].positions = new Vector3[positions.Count];
				coll.spriteDefinitions[i].uvs = new Vector2[uvs.Count];
				for (int j = 0; j < positions.Count; ++j)
				{
					coll.spriteDefinitions[i].positions[j] = positions[j];
					coll.spriteDefinitions[i].uvs[j] = uvs[j];
				}
			}
			
            Vector3 boundsMin = new Vector3(1.0e32f, 1.0e32f, 1.0e32f);
            Vector3 boundsMax = new Vector3(-1.0e32f, -1.0e32f, -1.0e32f);
            foreach (Vector3 v in coll.spriteDefinitions[i].positions)
            {
                boundsMin = Vector3.Min(boundsMin, v);
                boundsMax = Vector3.Max(boundsMax, v);
            }
			
			coll.spriteDefinitions[i].boundsData = new Vector3[2];
			coll.spriteDefinitions[i].boundsData[0] = (boundsMax + boundsMin) / 2.0f;
			coll.spriteDefinitions[i].boundsData[1] = (boundsMax - boundsMin);
			coll.spriteDefinitions[i].name = gen.textureParams[i].name;
        }
    }
	
	// Menu entries
	
	[MenuItem("Assets/Create/tk2d/Sprite Collection", false, 10000)]
    static void DoCollectionCreate()
    {
		string path = tk2dEditorUtility.CreateNewPrefab("SpriteCollection");
        if (path != null)
        {
            GameObject go = new GameObject();
            go.AddComponent<tk2dSpriteCollection>();
            go.active = false;

            Object p = PrefabUtility.CreateEmptyPrefab(path);
            PrefabUtility.ReplacePrefab(go, p, ReplacePrefabOptions.ConnectToPrefab);

            GameObject.DestroyImmediate(go);
        }
    }	
}

using UnityEngine;
using UnityEditor;
using System.Collections;

public class SpritePostprocessor : AssetPostprocessor
{

    void OnPreprocessTexture()
    {
        if (assetImporter.assetPath.Contains("Sprites"))
        {
            TextureImporter importer = assetImporter as TextureImporter;
            importer.anisoLevel = 0;
			importer.spritePixelsPerUnit = 6;
			if (assetImporter.assetPath.Contains("Characters") || assetImporter.assetPath.Contains("Items"))
			{
				importer.spriteImportMode = SpriteImportMode.Multiple;
			}
			if (assetImporter.assetPath.Contains("Tiles"))
			{
				importer.spritePixelsPerUnit = 8;
				importer.wrapMode = TextureWrapMode.Repeat;
				importer.spriteImportMode = SpriteImportMode.Single;
			}
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
			
        }
		
    }

}

using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class NormalMapMarker : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        // Get texture importer for current texture
        TextureImporter textureImporter = (TextureImporter)assetImporter;

        //check if it's not null or already a normal map
        if (textureImporter != null && textureImporter.textureType != TextureImporterType.NormalMap)
        {
            // check if the name or path suggests its a normal map
            if (assetPath.ToLower().Contains("normal") || assetPath.ToLower().Contains("_n"))
            {

                textureImporter.textureType = TextureImporterType.NormalMap;
                textureImporter.SaveAndReimport();
            }
        }
    }
}

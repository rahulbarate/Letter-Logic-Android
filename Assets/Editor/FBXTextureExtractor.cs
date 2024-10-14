using UnityEditor;
using UnityEngine;

public class FBXTextureExtractor : AssetPostprocessor
{
    // Reference to texture folder
    static readonly string textureFolder = "Assets/Textures/";
    // This method get called everytime a model/asset is imported.
    void OnPostprocessModel(GameObject importedModel)
    {
        // Checking if imported asset is fbx or not
        if (assetPath.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase))
        {
            // Reference to the modelImporter of imported asset.
            ModelImporter modelImporter = (ModelImporter)assetImporter;
            // Checking if folder exists, else creating new
            if (!AssetDatabase.IsValidFolder(textureFolder))
            {
                AssetDatabase.CreateFolder("Assets", "Textures");

            }
            // Checking if textures are successfully extracted or not.
            bool result = modelImporter.ExtractTextures(textureFolder);
            if (result)
            {
                Debug.Log("Textures extracted successfully for " + importedModel.name + " in " + textureFolder);
            }
            else
            {
                Debug.Log("Error while extracting textures for " + importedModel.name);
            }
        }
    }
}

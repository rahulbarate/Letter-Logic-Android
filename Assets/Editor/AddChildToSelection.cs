using UnityEngine;
using UnityEditor;

public class AddChildToSelection
{
    [MenuItem("Tools/Add Child To Selected Objects")]
    static void AddChild()
    {
        if (Selection.gameObjects.Length < 2)
        {
            Debug.LogWarning("Select parents first, then select the child prefab LAST.");
            return;
        }

        GameObject sceneReference = Selection.activeGameObject;
        GameObject prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(sceneReference);

        if (prefabAsset == null)
        {
            Debug.LogError("Last selected object must be a prefab instance.");
            return;
        }

        foreach (var parent in Selection.gameObjects)
        {
            if (parent == sceneReference) continue;

            GameObject child = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset, parent.transform);

            child.transform.localPosition = sceneReference.transform.localPosition;
            child.transform.localRotation = sceneReference.transform.localRotation;
            child.transform.localScale = sceneReference.transform.localScale;

            Undo.RegisterCreatedObjectUndo(child, "Add Child");
        }
    }
}

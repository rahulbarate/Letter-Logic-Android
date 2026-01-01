using UnityEngine;

public class ScreenSpaceVFXAnchors : MonoBehaviour
{
    public Camera cam;
    public float depth = 3f; // distance in front of camera

    public Transform topLeft, topRight, bottomLeft, bottomRight, center;

    void LateUpdate()
    {
        topLeft.position = GetWorldPoint(0, Screen.height, depth);
        topRight.position = GetWorldPoint(Screen.width, Screen.height, depth);
        bottomLeft.position = GetWorldPoint(0, 0, depth);
        bottomRight.position = GetWorldPoint(Screen.width, 0, depth);
        center.position = GetWorldPoint(Screen.width / 2f, Screen.height / 2f, depth);
    }

    Vector3 GetWorldPoint(float x, float y, float z)
    {
        return cam.ScreenToWorldPoint(new Vector3(x, y, z));
    }
}


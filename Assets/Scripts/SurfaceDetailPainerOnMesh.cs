using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SurfaceDetailPainerOnMesh : MonoBehaviour
{
    [SerializeField] GameObject[] detailPrefabs;// holds prefabs/details to be painted
    [SerializeField] int numberOfDetails = 100;
    [SerializeField] bool alignToSurfaceNormal;
    [SerializeField] float prefabYPositionOffset = 0.0f;

    MeshFilter meshFilter;





    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("Mesh Filter component not found for this object!");
            return;
        }

        // Getting mesh vertices;
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        //Generating details on the surface using vertices
        for (int i = 0; i < numberOfDetails; i++)
        {
            // Choosing random vertex
            int vertexIndex = Random.Range(0, vertices.Length);

            // Getting position for that vertex
            Vector3 vertexPos = transform.TransformPoint(vertices[vertexIndex]);

            // Adjusting Y offset for prefab
            vertexPos.y += prefabYPositionOffset;

            // Choosing random rotation angle for Y
            float randomYRotation = Random.Range(0f, 360f);

            // spawning random prefab at the random vertex choosen
            GameObject detailPrefab = detailPrefabs[Random.Range(0, detailPrefabs.Length)];
            GameObject detailInstance = Instantiate(detailPrefab, vertexPos, Quaternion.Euler(0f, randomYRotation, 0f));

            // Aligning prefab to normal
            if (alignToSurfaceNormal)
            {
                Vector3 normal = transform.TransformDirection(normals[vertexIndex]);
                detailInstance.transform.up = normal;
            }


        }



    }
}

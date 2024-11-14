using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombDropper : MonoBehaviour
{
    [SerializeField] GameObject bombCopy;
    [SerializeField] GameObject requestPlatform;
    // AlphabetLCInstantiator alphabetLCInstantiator;
    [SerializeField] float delayInBombing;


    MeshFilter meshFilter;
    Vector3[] vertices;


    // Start is called before the first frame update
    void Start()
    {
        // Get mesh filter
        meshFilter = GetComponent<MeshFilter>();
        vertices = meshFilter.mesh.vertices;
        // alphabetLCInstantiator = requestPlatform.GetComponent<AlphabetLCInstantiator>();

        StartCoroutine(DropBombs());

    }

    IEnumerator DropBombs()
    {
        while (!GameDataSave.IsLevelCompleted)
        {
            int randomVertexIndex = UnityEngine.Random.Range(0, vertices.Length);
            Vector3 vertexPos = transform.TransformPoint(vertices[randomVertexIndex]);
            Instantiate(bombCopy, vertexPos, Quaternion.Euler(0f, 0f, 180f));
            yield return new WaitForSeconds(delayInBombing);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class BombDropper : MonoBehaviour
{
    [SerializeField] GameObject bombCopy;
    [SerializeField] GameObject requestPlatform;
    // AlphabetLCInstantiator alphabetLCInstantiator;
    [SerializeField] float delayInBombing;
    [SerializeField] GameDataSave gameDataSave;
    bool isLevelCompleted = false;

    private ObjectPool<GameObject> bombPool;

    MeshFilter meshFilter;
    Vector3[] vertices;


    // Start is called before the first frame update
    void Start()
    {
        // Get mesh filter
        meshFilter = GetComponent<MeshFilter>();
        vertices = meshFilter.mesh.vertices;
        // gameDataSave.E_LevelCompleted += SetIsLevelCompleted;
        // alphabetLCInstantiator = requestPlatform.GetComponent<AlphabetLCInstantiator>();

        bombPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(bombCopy),
            actionOnGet: (obj) =>
            {
                obj.SetActive(true);
                var handler = obj.GetComponent<BombHandler>();
                handler.ResetForPool();
                handler.pool = bombPool;
            },
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );

        StartCoroutine(DropBombs());

    }

    private void SetIsLevelCompleted()
    {
        isLevelCompleted = true;
    }
    // private void OnDisable()
    // {
    //     gameDataSave.E_LevelCompleted -= SetIsLevelCompleted;
    // }

    IEnumerator DropBombs()
    {
        while (!isLevelCompleted)
        {
            int randomVertexIndex = UnityEngine.Random.Range(0, vertices.Length);
            Vector3 vertexPos = transform.TransformPoint(vertices[randomVertexIndex]);
            GameObject bomb = bombPool.Get();
            bomb.transform.position = vertexPos;
            bomb.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            yield return new WaitForSeconds(delayInBombing);

        }
    }
}

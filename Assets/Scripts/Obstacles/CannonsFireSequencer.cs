using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonsFireSequencer : MonoBehaviour
{
    // Start is called before the first frame update
    int childCount;
    [SerializeField] float delayInSeconds = 3f;
    [SerializeField] GameObject requestPlatform;
    [SerializeField] GameDataSave gameDataSave;
    bool isLevelCompleted;
    // AlphabetLCInstantiator alphabetLCInstantiator;
    void Start()
    {
        childCount = transform.childCount;
        // gameDataSave.E_LevelCompleted += SetIsLevelCompleted;
        // alphabetLCInstantiator = requestPlatform.GetComponent<AlphabetLCInstantiator>();
        StartCoroutine(FireCannon());
    }

    private void SetIsLevelCompleted()
    {
        isLevelCompleted = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator FireCannon()
    {
        while (!isLevelCompleted)
        {
            foreach (Transform child in transform)
            {
                if (!isLevelCompleted)
                {
                    // break;
                    CannonHandler cannonHandler = child.GetComponent<CannonHandler>();
                    if (cannonHandler != null)
                    {
                        child.GetComponent<CannonHandler>().FireCannon();
                        yield return new WaitForSeconds(delayInSeconds);

                    }
                }
            }

        }
    }

    // private void OnDisable()
    // {
    //     gameDataSave.E_LevelCompleted -= SetIsLevelCompleted;
    // }
}

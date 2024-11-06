using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonsFireSequencer : MonoBehaviour
{
    // Start is called before the first frame update
    int childCount;
    [SerializeField] float delayInSeconds = 3f;
    void Start()
    {
        childCount = transform.childCount;
        StartCoroutine(FireCannon());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator FireCannon()
    {
        while (true)
        {
            foreach (Transform child in transform)
            {
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

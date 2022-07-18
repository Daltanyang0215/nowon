using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Box : MonoBehaviour
{
    public GameObject spwanBox;

    public float spwanRange;
    public int spwanCount;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spwanCount; i++)
        {
            Instantiate(spwanBox, new Vector3(Random.Range(-spwanRange, spwanRange),3 ,Random.Range(-spwanRange, spwanRange)), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

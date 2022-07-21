using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Box : MonoBehaviour
{
    public GameObject spwanBox;
    public Player player;

    public float spwanRange;
    public float spwangheiht;

    public int spwanCount;

    // Start is called before the first frame update
    void Start()
    {
        SpwanBox();
        InvokeRepeating("ResetBox", 5f,5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpwanBox()
    {
        for (int i = 0; i < spwanCount; i++)
        {
            GameObject addblock = Instantiate(spwanBox, new Vector3(Random.Range(-spwanRange, spwanRange), Random.Range(3, spwangheiht), Random.Range(-spwanRange, spwanRange)), Quaternion.identity);
            addblock.transform.parent = transform;
            addblock.GetComponent<Box_Script>().player = player;
        }
    }

    void ResetBox()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                transform.GetChild(i).gameObject.transform.position = new Vector3(Random.Range(-spwanRange, spwanRange), Random.Range(3, spwangheiht), Random.Range(-spwanRange, spwanRange));
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

}

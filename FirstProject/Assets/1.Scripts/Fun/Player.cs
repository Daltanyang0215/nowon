using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    
    private float forwad,right, rotate;
    private bool clampCTR= false;

    public GameObject bullet;
    public List<GameObject> blocks = new List<GameObject>();

    void Update()
    {
        GetInput();
        //BoxDestroy();
        //BulletShot();
        StartCoroutine(BulletShot());
    }

    void FixedUpdate()
    {
        Move();
    }

    public void GetInput()
    {
        rotate = Input.GetAxis("Mouse X");
        forwad = Input.GetAxisRaw("Vertical");
        right = Input.GetAxisRaw("Horizontal");
        clampCTR = Input.GetKey(KeyCode.LeftControl);
    }

    public void Move()
    {
        
        transform.Translate(new Vector3(right, 0, forwad).normalized * speed * Time.fixedDeltaTime);
        if(!clampCTR) transform.Rotate(new Vector3(0, rotate * rotateSpeed ,0));
    }

    public void BoxDestroy()
    {
        if (Input.GetMouseButtonUp(0))
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                Destroy(blocks[i]);
            }
            blocks.Clear();
        }
    }

    //public void BulletShot()
    //{
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        for (int i = 0; i < blocks.Count; i++)
    //        {
    //            GameObject shotbullet = Instantiate(bullet,transform.position,Quaternion.identity);
    //            shotbullet.GetComponent<Bullet>().target = blocks[i].transform;
    //        }
    //        blocks.Clear();
    //    }

    //}

    public IEnumerator BulletShot()
    {
        while (true) {
            if (Input.GetMouseButtonUp(0))
            {
                for (int i = 0; i < blocks.Count; i++)
                {
                    GameObject shotbullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    shotbullet.GetComponent<Bullet>().target = blocks[i].transform;
                    yield return new WaitForSeconds(0.05f);
                }
                blocks.Clear();
            }
            yield return null ;
        }
    }

}

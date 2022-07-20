using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public Vector3 moveVec;

    public bool isDash;
    public Vector3 dashVec;

    private float forwad,right, rotate;
    private bool clampCTR= false;

    public GameObject bullet;
    public List<GameObject> blocks = new List<GameObject>();
    public Queue<GameObject> blocksQueue = new Queue<GameObject>();
    public int maxTargetBlock;

    private void Start()
    {
        StartCoroutine(BulletShot());
        
    }

    void Update()
    {
        GetInput();
        //BoxDestroy();
        //BulletShot();
        
        Dash();
        TargetQueue();
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

    void TargetQueue()
    {

    }

    public void Move()
    {
        if (!isDash)
            moveVec = new Vector3(right, 0, forwad).normalized;
        else
            moveVec = dashVec;

        transform.Translate(moveVec * speed * Time.fixedDeltaTime);
        if(!clampCTR) transform.Rotate(new Vector3(0, rotate * rotateSpeed ,0));
    }

    public void Dash()
    {
        if(moveVec != Vector3.zero && !isDash && Input.GetKeyDown(KeyCode.Space))
        {
            isDash = true;
            speed *= 4;
            dashVec = moveVec;

            Invoke("Undash", 0.2f);
        }
    }

    public void Undash()
    {
        isDash = false;
        speed /= 4;
    }


    //public void BoxDestroy()
    //{
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        for (int i = 0; i < blocks.Count; i++)
    //        {
    //            Destroy(blocks[i]);
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

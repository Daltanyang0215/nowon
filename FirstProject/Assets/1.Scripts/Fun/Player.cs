using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    
    private float forwad, right;
    public float moveSpeed;
    private Vector3 moveVec;

    private bool isDash;
    private Vector3 dashVec;

    [Space]
    [Header("Camera")]

    private float rotateX, rotateY;
    [SerializeField] private float rotateX_Min, rotateX_Max;
    public float rotateYSpeed;
    public float rotateXSpeed;
    [SerializeField] private Transform cameraAnchor;
    private bool clampCTR= false;

    [Space]
    [Header("Block Target")]
    
    [SerializeField] private GameObject bullet;
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
        CameraRotate();
        TargetQueue();
    }

    void FixedUpdate()
    {
        Move();
    }

    public void GetInput()
    {
        rotateY = Input.GetAxis("Mouse X");
        rotateX = Input.GetAxis("Mouse Y");
        forwad = Input.GetAxisRaw("Vertical");
        right = Input.GetAxisRaw("Horizontal");
        clampCTR = Input.GetKey(KeyCode.LeftControl);
    }

    void TargetQueue()
    {

    }

    void CameraRotate()
    {
        if (!clampCTR)
        {

            float tmp_x = cameraAnchor.eulerAngles.x -rotateX * rotateXSpeed;
            float tmp_y = transform.eulerAngles.y + rotateY * rotateYSpeed;

            tmp_x = tmp_x > 180 ? tmp_x-360 : tmp_x; // 오일러 0~360 -> -180~180 으로 전환
            tmp_y = tmp_y > 180 ? tmp_y-360 : tmp_y; // 오일러 0~360 -> -180~180 으로 전환

            if (rotateX_Max < tmp_x)
            {
                tmp_x = rotateX_Max;
            }
            else if (tmp_x < rotateX_Min)
            {
                tmp_x = rotateX_Min;
            }

            cameraAnchor.rotation = Quaternion.Euler(tmp_x, transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Euler(0, tmp_y,0);

            //cameraAnchor.Rotate(Vector3.right, tmp_x);
            //cameraAnchor.Rotate(Vector3.left * rotateX * rotateXSpeed);
        }
    }

    public void Move()
    {
        if (!isDash)
            moveVec = new Vector3(right, 0, forwad).normalized;
        else
            moveVec = dashVec;

        transform.Translate(moveVec * moveSpeed * Time.fixedDeltaTime);
    }

    public void Dash()
    {
        if(moveVec != Vector3.zero && !isDash && Input.GetKeyDown(KeyCode.Space))
        {
            isDash = true;
            moveSpeed *= 4;
            dashVec = moveVec;

            Invoke("Undash", 0.2f);
        }
    }

    public void Undash()
    {
        isDash = false;
        moveSpeed /= 4;
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

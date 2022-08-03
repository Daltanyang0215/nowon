using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveSpeed_correction;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float rotateSpeed_correction;
    public bool curve = true;
    private bool shot = false;

    private void Start()
    {
        //transform.LookAt(target);

    }

    public void FixedUpdate()
    {
        BulletMove();
    }

    public void BulletMove()
    {
        if (curve)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);
            Vector3 dir = target.transform.position - this.transform.position;

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
        }
        else
        {
            if (shot)
            {
                transform.LookAt(target);
                transform.position = Vector3.Lerp(transform.position, target.position, 0.2f);
            }
        }
    }

    public void Update()
    {
        if (!target.gameObject.activeSelf)
            Destroy(gameObject);

        moveSpeed += moveSpeed_correction * Time.deltaTime;
        rotateSpeed += rotateSpeed_correction * Time.deltaTime;
        if (Vector3.Distance(transform.position, target.position) < rotateSpeed * 2) rotateSpeed *= 1.5f;
    }
    public void DelayShot()
    {
        Invoke("DoShot", 1);
    }
    private void DoShot()
    {
        shot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject == target.gameObject)
            {
                other.gameObject.SetActive(false);
                if (!shot) PlayerBulletShot.Instance.stack++;
                Destroy(gameObject);
            }
        }
    }

}

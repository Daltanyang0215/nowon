using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float rotateSpeed_correction;

    private void Start()
    {
        //transform.LookAt(target);
        
    }

    public void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position,target.position , moveSpeed);
        
        // 직선 이동
        // -------------------------------------------------------------------------------
        // 곡선 이동
        
        transform.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);
        Vector3 dir = target.transform.position - this.transform.position;

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
    }

    public void Update()
    {
        if(!target.gameObject.activeSelf)
            Destroy(gameObject);

        rotateSpeed += rotateSpeed_correction* Time.deltaTime;
        if (Vector3.Distance(transform.position, target.position) < rotateSpeed * 2) rotateSpeed *= 1.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.gameObject == target.gameObject)
            {
                other.gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }

}

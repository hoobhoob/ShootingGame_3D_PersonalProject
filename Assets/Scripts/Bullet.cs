using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bulletObj;
    private float bulletSpeed = 60000f;
    private Coroutine _shootCO;
    private Rigidbody _rigidbody;
    private Vector3 _moveDir;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 dir)
    {
        ResetVelocity();
        _moveDir = dir;
        _shootCO = StartCoroutine(ShootCO());
    }

    IEnumerator ShootCO()
    {
        while (true)
        {
            _rigidbody.AddForce(_moveDir * bulletSpeed * Time.deltaTime);
            //_rigidbody.velocity = _moveDir * Time.deltaTime;
            //_bulletObj.transform.position += _moveDir * bulletSpeed * Time.deltaTime;
            //Debug.Log($"Plus : {_moveDir * bulletSpeed * Time.deltaTime}");
            //Debug.Log($"bullet position : {_bulletObj.transform.position}");
            yield return null;
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Ground")
        {
            Debug.Log("Bullet Hit");
            StopCoroutine(_shootCO);
            gameObject.SetActive(false);
        }
    }

    private void ResetVelocity()
    {
        _rigidbody.velocity = Vector3.zero;
        _moveDir = Vector3.zero;
    }
}

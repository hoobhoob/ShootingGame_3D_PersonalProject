using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform _bulletSpawnTransform;

    public int ReloadBulletCount { get; private set; }
    public int CurBulletCount { get; private set; }
    private ObjectPool _bulletPool;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _clip;
        _bulletPool = GetComponent<ObjectPool>();
    }

    public void Fire()
    {
        _particleSystem.Stop();
        _particleSystem.Play();

        _audioSource.Stop();
        _audioSource.Play();

        CurBulletCount += -1;
        Debug.Log("Weapon Fire!");
        GameObject bulletObj = _bulletPool.SpawnFromPool("Bullet");
        bulletObj.SetActive(true);
        Vector3 bulletposition = _bulletSpawnTransform.transform.position;
        Vector3 forward = _bulletSpawnTransform.transform.forward;
        bulletposition += 3.5f * forward;
        bulletObj.transform.position = bulletposition;
    }

    public bool IsEmpty()
    {
        return CurBulletCount == 0;
    }

    public void ReloadBullet(int bulletCount)
    {
        CurBulletCount = bulletCount < ReloadBulletCount ? bulletCount : ReloadBulletCount;
    }
}

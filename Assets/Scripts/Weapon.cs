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
    [SerializeField] private BulletCountUI _bulletCountUI;

    public int ReloadBulletCount { get; private set; }
    public int CurBulletCount { get; private set; }
    private ObjectPool _bulletPool;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _clip;
        _bulletPool = GetComponent<ObjectPool>();
        ReloadBulletCount = 30;
        CurBulletCount = ReloadBulletCount;
    }

    private void Start()
    {
        _bulletCountUI.SetReloadBulletCountTxt(ReloadBulletCount);
        _bulletCountUI.SetCurBulletCountTxt(CurBulletCount);
    }

    public void Fire()
    {
        if (!IsEmpty())
        {
            _particleSystem.Stop();
            _particleSystem.Play();

            _audioSource.Stop();
            _audioSource.Play();

            CurBulletCount += -1;

            GameObject bulletObj = _bulletPool.SpawnFromPool("Bullet");
            bulletObj.SetActive(true);

            Vector3 bulletposition = _bulletSpawnTransform.transform.position;
            Vector3 forward = _bulletSpawnTransform.transform.forward;

            bulletposition += 3.5f * forward;
            bulletObj.transform.position = bulletposition;
            bulletObj.transform.rotation = Quaternion.LookRotation(forward);

            bulletObj.GetComponent<Bullet>().Shoot(forward);

            _bulletCountUI.SetCurBulletCountTxt(CurBulletCount);
        }
    }

    public bool IsEmpty()
    {
        if (CurBulletCount <= 0)
            CurBulletCount = 0;

        return CurBulletCount == 0;
    }

    public void ReloadBullet(int bulletCount)
    {
        CurBulletCount = bulletCount < ReloadBulletCount ? bulletCount : ReloadBulletCount;

        _bulletCountUI.SetCurBulletCountTxt(CurBulletCount);
    }

    public void SetReloadBulletCount(int reloadBulletCount)
    {
        ReloadBulletCount = reloadBulletCount;

        _bulletCountUI.SetReloadBulletCountTxt(ReloadBulletCount);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private ParticleSystem _particleSystem;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _clip;
    }

    public void Fire()
    {
        _particleSystem.Stop();
        _particleSystem.Play();
        _audioSource.Stop();
        _audioSource.Play();
        Debug.Log("Weapon Fire!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBoard : MonoBehaviour
{
    [SerializeField] private Collider2D[] _colliders;
    private int[] _scores;
    private bool _isHit = false;
    private int _cLength;
    private int _index = -1;

    private void Awake()
    {
        _cLength = _colliders.Length;
        int score = 100;
        _scores = new int[_cLength];
        for (int i = 0; i < _cLength; i++)
        {
            _scores[_cLength - i - 1] = score;
            score += -20;
        }
    }

    public void CheckTrigger()
    {
        for (int i = _cLength - 1; i >= 0; i--)
        {
            if (_colliders[i].isTrigger)
            {
                _index = i;
                _isHit = true;
                break;
            }
        }
    }

    public bool IsHit()
    {
        return _isHit;
    }

    public int GetScore()
    {
        if (_isHit && _index >= 0)
            return _scores[_index];
        else
            return 0;
    }

    public void ResetTargetBoard()
    {
        _isHit = false;
        _index = -1;
    }
}

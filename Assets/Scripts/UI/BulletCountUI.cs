using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletCountUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _curBulletCountTxt;
    [SerializeField] private TMP_Text _reloadBulletCountTxt;
    private Color _originColor;
    private int _reloadBulletCount;

    private void Awake()
    {
        _originColor = _curBulletCountTxt.color;
    }

    public void SetReloadBulletCountTxt(int amount)
    {
        _reloadBulletCount = amount;
        _reloadBulletCountTxt.text = amount.ToString();
    }

    public void SetCurBulletCountTxt(int amount)
    {
        if (amount == _reloadBulletCount)
        {
            _curBulletCountTxt.color = _reloadBulletCountTxt.color;
        }else if(0 < amount)
        {
            _curBulletCountTxt.color = _originColor;
        }
        else
        {
            _curBulletCountTxt.color = Color.red;
        }

        _curBulletCountTxt.text = amount.ToString();
    }
}

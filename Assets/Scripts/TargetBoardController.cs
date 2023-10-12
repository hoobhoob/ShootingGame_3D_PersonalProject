using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBoardController : MonoBehaviour
{
    [SerializeField] private GameObject _targetBoardObj;
    [SerializeField] private ScoreUI _scoreUI;
    private TargetBoard _targetBoard;
    private Vector3 _originTargetPosition;

    private void Awake()
    {
        _targetBoardObj.SetActive(true);
        _originTargetPosition = _targetBoardObj.transform.position;
        _targetBoard = _targetBoardObj.GetComponent<TargetBoard>();
    }

    private void Start()
    {
        StartCoroutine(OpenTargetBoardCO());
    }

    IEnumerator OpenTargetBoardCO()
    {
        while(true)
        {
            if (_targetBoardObj.activeSelf == false)
            {
                Debug.Log("activeSelf false");
                yield return new WaitForSecondsRealtime(2f);

                _targetBoardObj.SetActive(true);
            }
            else
            {
                _targetBoard.CheckTrigger();

                if (_targetBoard.IsHit())
                {
                    _scoreUI.AddScore(_targetBoard.GetScore());
                    _targetBoard.ResetTargetBoard();
                    _targetBoardObj.SetActive(false);
                }
            }

            yield return null;
        }
    }
}

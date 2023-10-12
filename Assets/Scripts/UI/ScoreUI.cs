using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _totalScoreTxt;
    [SerializeField] private GameObject _plusScoreBG;
    [SerializeField] private TMP_Text _plusScoreTxt;
    private int _totalScore = 0;
    private Coroutine _scoreCoroutine;

    private void Awake()
    {
        _totalScoreTxt.text = "0";
    }

    public void AddScore(int score)
    {
        _totalScore += score;
        _plusScoreTxt.text = $"+{score}";
        _totalScoreTxt.text = $"{_totalScore}";

        if (_scoreCoroutine != null)
            StopCoroutine(_scoreCoroutine);
        _scoreCoroutine = StartCoroutine(PlusScoreCO());

    }

    IEnumerator PlusScoreCO()
    {
        _plusScoreBG.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        _plusScoreBG.SetActive(false);
    }
}

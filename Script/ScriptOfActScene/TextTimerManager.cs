using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTimerManager : MonoBehaviour
{
    TMP_Text timerText;

    int time = 0;

    void Awake()
    {
        timerText = GetComponent<TMP_Text>();
    }

    public void StartTimer()
    {
        time = 0;
        StartCoroutine(DelayCountTime());
    }
    IEnumerator DelayCountTime()
    {
        while (true)
        {
            timerText.SetText("経過時間：{0}", time);

            yield return new WaitForSeconds(1);

            time++;
        }
    }

    public void EndTimer()
    {
        StopCoroutine(DelayCountTime());
    }
}

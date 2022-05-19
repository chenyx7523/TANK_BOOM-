using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//  控制暂停倒计时的显示         TODO  用协程方法重构一次
public class SpendTime : ScenesManager
{
    [HideInInspector] public Text NumberText;

    [HideInInspector] public int Number;
    [HideInInspector] private int m_FirstNumber = ValueManager.FirstNumber;           //暂停倒计时从几开始
    [HideInInspector] private WaitForSeconds WaitTime;



    //使用协程重构
    void OnEnable()
    {
        Number = m_FirstNumber + 1;
        NumberText.text = string.Empty;
        WaitTime = new WaitForSeconds(1f);
        StartCoroutine(CountDown());
        //Debug.Log("脚本启动");
    }
    IEnumerator CountDown()
    {
        yield return StartCoroutine(CountDown1());
        if (!(Number == 0))
        {
            StartCoroutine(CountDown());
        }
        else
        {
            NumberText.text = string.Empty;
            Number = m_FirstNumber;
            Hide();
        }
    }

    IEnumerator CountDown1()
    {
        Number--;
        if (Number == 0)
        {
            NumberText.text = "开始！";
        }
        else
        {
            NumberText.text = Number.ToString();
        }
        
        yield return WaitTime;
    }




    //[HideInInspector] private float time;
    //[HideInInspector] public int Number;
    //[HideInInspector] private int FirstNumber = 4;

    //void Awake()
    //{
    //    time = 1;
    //    Number = FirstNumber;
    //    NumberText.text = string.Empty;
    //    Debug.Log("唤醒啦");
    //}
    //void Update()
    //{
    //    if (Number > 0)
    //    {
    //        if (time < 1)
    //        {
    //            time += Time.deltaTime;
    //        }
    //        else if (time >= 1)
    //        {
    //            Number--;
    //            NumberText.text = Number.ToString();
    //            time = 0;
    //        }
    //    }
    //    if (Number == 0)
    //    {

    //        time = 1;
    //        NumberText.text = string.Empty;
    //        Number = FirstNumber;
    //        Hide();

    //    }
    //}





















}

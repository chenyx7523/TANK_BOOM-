using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//  控制暂停倒计时的显示
public class SpendTime : ScenesManager
{
    [HideInInspector] public Text NumberText;
    [HideInInspector] private float time;
    [HideInInspector] public  int Number;
    [HideInInspector] private int FirstNumber = 4;
    [HideInInspector] public bool timeEnd;

    

    void Awake()
    {
        time = 1;
        Number = FirstNumber;
        NumberText.text = string.Empty;
        timeEnd = false;
    }
    
    void Update()
    {
        if (Number > 0)
        {
            if (time < 1)
            {
                time += Time.deltaTime;
            }
            else if (time >= 1)
            {
                Number--;
                NumberText.text = Number.ToString();
                time = 0;
                timeEnd = false;
            }
        }
        if (Number == 0)
        {
            timeEnd = true;
            time = 1;
            NumberText.text = string.Empty;
            Number = FirstNumber;
            timeEnd = false;
            Hide();
            
        }







    }
















}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    //public GameObject lampLight;        //路灯组件
    //public GameObject TankLight;     //坦克车灯

    public GameObject Sun;         //太阳
    public GameObject SceneNumber;

    private bool SunLight;    //是否由太阳光
    public float SunMoveSpeed=0.05f; //太阳移动速度
    [HideInInspector]private float SunValue;






    void Start()
    {
        //实例化太阳灯


        SunLight = false;
        //LampLight();
        //TimeValue = Time.deltaTime;
    }

    private void Update()
    {
        SunUp();
        //Debug.Log(SunLight);
        //Debug.Log(Time.deltaTime);
        //Debug.Log("Time.deltaTime的值为 ：" + Time.deltaTime);
    }


    //void LampLight()
    //{
    //    int ScenenNumber = SceneNumber.GetComponent<SceneNumber>().m_SceneNumber;
    //    if (ScenenNumber == 1)   //白天
    //    {
    //        lampLight.SetActive(false);   //路灯开关
    //    }
    //    else if (ScenenNumber == 2)   //黑夜
    //    {
    //        lampLight.SetActive(true);
    //    }
    //}

    void SunUp()
    {
        Sun.GetComponent<Light>().intensity = SunValue;

        if (SunValue < 1 && ! SunLight)
        {
            
            SunValue += Time.deltaTime * SunMoveSpeed;  //可以正常相加
            if (SunValue >=1)
            {
                SunLight = true;
            }
        }    
        if(SunLight)
        {
            
            
            SunValue -= Time.deltaTime * SunMoveSpeed;  //可以正常减去

            if (SunValue <= 0)
            {
                SunLight = false;
            }  
            
        } 
     
    }





















}

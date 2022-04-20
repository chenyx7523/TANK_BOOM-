using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public GameObject lampLight;        //路灯组件
    //public GameObject TankLight;     //坦克车灯
    public GameObject Sun;         //太阳


    public GameObject SceneNumber;








    void Start()
    {

        LampLight();
    }



    void LampLight()
    {
        int ScenenNumber = SceneNumber.GetComponent<SceneNumber>().m_SceneNumber;
        if (ScenenNumber == 1)
        {
            lampLight.SetActive(false);
        }
        else if (ScenenNumber == 2)
        {
            lampLight.SetActive(true);
        }
    }
    

    


















}

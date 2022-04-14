using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{

    public float Damp;
    public float star;
    public float targer;
    public float speed;


    public void GameStar()
    {
        SceneManager.LoadScene(1);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public  void GameRestar()
    {
        float i = DampTime(10);
        if (i == 1)
        {
            SceneManager.LoadScene(1);
        }
        
    }

    //延时方法
    public float DampTime(float deltaTime)
    {
        
        star = 0;
        targer = 1;
        speed = 1;
         
        Damp = Mathf.SmoothDamp(star,targer, ref speed,deltaTime);   



        return Damp;
    }








}

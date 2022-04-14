using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
     public void GameStar()
    {
        SceneManager.LoadScene(1);
    }

    public void GameExit()
    {
        Application.Quit();
    }


    //重新开始游戏
    public  void GameRestar()
    {
        SceneManager.LoadScene(1);
    }
    

  

    








}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public GameObject MapChange;

    //[System.Obsolete]
    public void GameStar()
    {        
        MapChange.SetActive(true);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void DayMap()
    {
        SceneManager.LoadScene(1);
    }

    public void NightMap()
    {
        SceneManager.LoadScene(2);
    }

    public void Close()
    {
        MapChange.SetActive(false);
    }





    //重新开始游戏
    public  void GameRestar()
    {
        SceneManager.LoadScene(1);
    }
    

  

    








}

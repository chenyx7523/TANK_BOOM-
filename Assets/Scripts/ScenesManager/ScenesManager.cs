using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [HideInInspector] public GameObject MapChange;              //地图选择
    [HideInInspector] public GameObject Spendpage;              //暂停界面
    


    

    //[System.Obsolete]

    private void Start()
    {
        //test();
    }

    public int SceneNumber()
    {
        //判断一下当前所在场景（day or night）
        Scene a = SceneManager.GetActiveScene();
        ////Debug.Log("Active Scene name is: " + a.name + "\nActive Scene index: " + a.buildIndex); //https://docs.unity.cn/cn/2019.4/ScriptReference/SceneManagement.Scene-buildIndex.html
        int m_SceneNumber = a.buildIndex;
        return m_SceneNumber;
    }


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
        SceneManager.LoadScene(SceneNumber());
    }
    
    //public void SuspendIsFalse()
    //{
    //    Spendpage.SetActive(false);
    //    Debug.Log("页面已关闭");
    //}
    //显示
    public virtual void Show()  //virtual声明成一个虚方法
    {
        gameObject.SetActive(true);
    }
    //隐藏
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    //private void test()
    //{
         
    //    Debug.Log (SceneNumber());  
    //}
    public void ReMenu()
    {
        SceneManager.LoadScene(0);
    }

    






}

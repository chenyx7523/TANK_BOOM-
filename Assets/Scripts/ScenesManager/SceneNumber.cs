using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNumber : MonoBehaviour
{
    [HideInInspector] public int m_SceneNumber;                              //记录一下场景序号

    void Start()
    {
        GetSceneNumber();
    }


    public void GetSceneNumber()
    {
        //判断一下当前所在场景（day or night）
        Scene a = SceneManager.GetActiveScene();
        ////Debug.Log("Active Scene name is: " + a.name + "\nActive Scene index: " + a.buildIndex); //https://docs.unity.cn/cn/2019.4/ScriptReference/SceneManagement.Scene-buildIndex.html
        m_SceneNumber = a.buildIndex;
    }

}

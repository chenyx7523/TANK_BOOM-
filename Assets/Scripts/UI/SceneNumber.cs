using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNumber : ScenesManager
{
    [HideInInspector] public int m_SceneNumber;
    void Start()
    {
        m_SceneNumber = SceneNumber();
    }

    
}

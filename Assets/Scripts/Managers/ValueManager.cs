using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour
{
    public static ValueManager instance;

    public static float testNumber = 1314;

    public void Awake()
    {
        instance = this;    
    }
    public static ValueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ValueManager>();
            }
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.AddComponent<ValueManager>();
                instance = go.AddComponent<ValueManager>(); 
            }
            return instance;
        }
    
    }

}

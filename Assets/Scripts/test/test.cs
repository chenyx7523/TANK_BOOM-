using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //transform.RotateAround(Vector3.zero,Vector3.right,10f * Time.deltaTime);
    //}


    //private void HideClick()
    //{
    //    gameObject.SetActive(false);    
    //}
    [HideInInspector]public float a = 5f;
    [HideInInspector] public float b = 30f;
    public void OnEnable()
    {
        Debug.Log(b);
    }

}

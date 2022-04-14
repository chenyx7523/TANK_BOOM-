using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NoRotate : MonoBehaviour
{
    // 确保世界空间UI生命值条等元素朝向正确的方向

    public bool m_UseRelativeRotation = true;            // 这个游戏对象是否使用相对旋转

    //m_localRotation 相对于父级变换旋转的变换旋转
    private Quaternion m_localRotation;               //定义一个四元数，用来相对旋转   现场开始时的局部旋转。

    private void Start()
    {
        //令四元数等于最初的旋转
        m_localRotation = transform.localRotation;   //localRotation 用于相对旋转，限于四元数  https://docs.unity.cn/cn/2019.4/ScriptReference/Transform-localRotation.html
    }

    private void Update()
    {
        //不断使得相对旋转角度相同，即角度不变
        if (m_UseRelativeRotation)
            transform.rotation = m_localRotation;  //rotation 旋转属性
    }
}

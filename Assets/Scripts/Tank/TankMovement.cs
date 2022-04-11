using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    //移动
    public int m_Playernum;           // 用来识别哪个坦克属于哪个玩家  
    public float m_Speed;              // 坦克的速度配置 
    public float m_TurnSpeed;        // 坦克的旋转速度 

    //声音
    public AudioSource m_Move;          // 引擎音源 
    public AudioClip m_EngineQuiet;     //引擎静止
    public AudioClip m_EngineDriving;   //驾驶中
    //public float m_PitchRange;

}

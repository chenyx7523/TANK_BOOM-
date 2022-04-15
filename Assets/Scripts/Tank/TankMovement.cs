﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    //移动
    public int m_Playernum;           // 用来识别哪个坦克属于哪个玩家  
    public float m_Speed;              // 坦克的速度配置 
    public float m_TurnSpeed;        // 坦克的旋转速度 

    //声音
    public AudioSource m_MoveSound;          // 引擎音源 
    public AudioClip m_EngineQuiet;     //引擎静止
    public AudioClip m_EngineDriving;   //驾驶中
    public float m_PitchRange;       //引擎声音变化

    //其他
    private string m_MoveAxis;      //移动键名称
    private string m_TurnAxis;      //转向键名称
    private Rigidbody m_Rigidbody;  //坦克刚体
    private float m_MoveValue;      //移动值
    private float m_TureValue;      //转向值
    private float m_SonudStar;      //声音起始值
    //private ParticleSystem[] m_ParticleSystem;   //实例化粒子系统


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    //激活后执行
    private void OnEnable()
    {
        //坦克运动状态为false
        m_Rigidbody.isKinematic = false;
        //输入值为0
        m_MoveValue = 0;
        m_TureValue = 0;
        //获取粒子系统，初始化粒子系统 TODO

    }

    private void OnDisable()
    {
        //坦克停下时运动状态为true
        m_Rigidbody.isKinematic = true;

        //停止所有粒子效果TODO
    }

    private void Start()
    {
        //记录按键操作，并且区分玩家，按键名称管理在 Edit/Project Setting/Input Manager
        m_MoveAxis = "Vertical" + m_Playernum;
        m_TurnAxis = "Horizontal" + m_Playernum;

        //存储原始音频大小，方便后期赋值
        m_SonudStar = m_MoveSound.pitch;

    }

    private void Update()
    {
        //存取玩家输入键
        m_MoveValue = Input.GetAxis(m_MoveAxis);
        m_TureValue = Input.GetAxis(m_TurnAxis);

        //每次输入都将改变坦克运行状态，所以都要进行状态判定来确定播放的音频
        //播放引擎声音 TODO
        //EnginePlay();
        
    }


    /*方法备注
     * 由于坦克音频运动只有两种可能，即运行或待机
     * 每当输入数值发生改变的时候，说明坦克的运动状态也发生改变
     * 可以不做判断直接将音频切换到另一个音频上
     * 并且在待机时由于输入都小于0.1，所以一直播放待机，但是输入值一旦增大，状态立马切换
     * 
     * 二号方案
     * 使当前位置和前几帧位置进行判断，有移动则是运行，无则是待机
     * 但是有延迟
     * 
     * 三号方案
     * 按下前进后退键则是运行
     * 松开则是待机
     */

    //根据坦克是否移动以及当前播放的音频播放正确的音频剪辑。（移动时播放引擎声音，停止时播放其他声音）
    private void EnginePlay()
    {
        //转向和移动接近停止时才需要切换待机和运行
        if (Mathf.Abs(m_MoveValue) < 0.1f && Mathf.Abs(m_TureValue) < 0.1f)
        {
            //待机状态
            if (m_MoveSound.clip == m_EngineDriving)
            {
                //状态改为空并将声音改为引擎待机声音，给个随机值，使得声音有变化
                m_MoveSound = null;
                m_MoveSound.clip = m_EngineQuiet;
                m_MoveSound.pitch = Random.Range(m_SonudStar - m_PitchRange, m_SonudStar + m_PitchRange);
                m_MoveSound.Play();
                

            }
        }
        else
        {
            if (m_MoveSound.clip == m_EngineQuiet)
            {
                m_MoveSound = null;
                m_MoveSound.clip = m_EngineDriving;
                m_MoveSound.pitch = Random.Range(m_SonudStar - m_PitchRange, m_SonudStar + m_PitchRange);
                m_MoveSound.Play();
            }
        }
    }

    //移动并旋转坦克   FixedUpdate用于物理计算的更新
    private void FixedUpdate()
    {
        Move();
        Turn();
    }


    //移动
    private void Move()
    {
        //移动的坐标值为   运动方向（前后方向or左右）*正负（前or后）*（速度*时间）即每秒运动speed距离
        Vector3 movemet = transform.forward * m_MoveValue * m_Speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movemet);//MovePosition   https://docs.unity.cn/cn/2019.4/ScriptReference/Rigidbody.MovePosition.html
    }



    //旋转
    private void Turn()
    {
        float turn = m_TureValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRoation = Quaternion.Euler(0f, turn, 0f);//强制将角度转换为四元数，（角度不支持浮点类型）
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRoation);
    }


























}


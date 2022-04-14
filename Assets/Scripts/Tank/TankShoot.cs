﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//控制坦克的子弹和爆炸
public class TankShoot : MonoBehaviour
{
    public int m_Playernum = 1;              // 用来识别不同的玩家。
    public Rigidbody m_Shell;                // 实例化预制体。
    public Transform m_FireTransform;        // 发射炮弹的坐标
    public Slider m_Aim;                     // 实例化滑块
    public AudioSource m_ShootAudio;         // 参考用于播放射击音频的音频源。注意:不同于运动音源。
    public AudioClip m_ChargingClip;         // 每次射击充能时播放的音频。
    public AudioClip m_FireClip;             // 每次射击时播放的音频。

    public float m_MinFire = 10f;             // 初始给予炮弹的力。
    public float m_MaxFire = 40;              // 在最大充能时间内按动射击按钮给予炮弹的力。
    private float m_MaxFireTime = 1f;         // 炮弹最大充能所需时间。

    private string m_FireButtonName;        // 长按发射的输入键（即空格）。
    private float m_UpFireButton;           // 当发射按钮被释放时，将给予炮弹的力量。
    private float m_ChargeSpeed;            // 根据最大充电时间，发射力增加的速度。
    private bool m_Fire;                   // 是否已经发射。
    private float m_ReFireTime = 2f;          //重新发射的间隔时间


    private void OnEnable()
    {
        // 当坦克启动时，重置发射力和UI
        m_UpFireButton = m_MinFire;
        m_Aim.value = m_MinFire;
        m_Fire = false;
    }


    private void Start()
    {

        m_FireButtonName = "Fire" + m_Playernum;

        // 发射力充能的速率是在最大充电时间内可能产生的力的范围。
        m_ChargeSpeed = (m_MaxFire - m_MinFire) / m_MaxFireTime;//（最大值-最小值）/最小到最大的时间 =充能速率

    }

    private void Update()
    {
        // 滑块应该有最小发射力的默认值。
        m_Aim.value = m_MinFire;

        // 如果超过了最大力，而炮弹还没有发射
        if (m_UpFireButton >= m_MaxFire && !m_Fire)
        {
            // 用Max力量发射炮弹。
            m_UpFireButton = m_MaxFire;
            Fire();
        }
        // 开火按钮刚刚开始被按下
        else if (Input.GetButtonDown(m_FireButtonName))
        {
            //重置发射状态和发射力量。
            m_Fire = false;
            m_UpFireButton = m_MinFire;

            // 播放充能声音
            m_ShootAudio.clip = m_ChargingClip;
            m_ShootAudio.Play();
        }
        //按住了射击键，而炮弹还没有发射
        else if (Input.GetButton(m_FireButtonName) && !m_Fire)
        {
            // 增加发射力并更新滑块。
            m_UpFireButton += m_ChargeSpeed * Time.deltaTime;
            m_Aim.value = m_UpFireButton;
        }
        // 发射按钮被释放，炮弹还没有发射,使其发射
        else if (Input.GetButtonUp(m_FireButtonName) && !m_Fire)
        {

            Fire();
        }

    }

    private void Fire()
    {
        // 使得状态改为发射了。
        m_Fire = true;
        // 创建一个子弹的实例，并将原来子弹的位置和旋转赋值给新创建的实例，并引用它的刚体。
        Rigidbody ShellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation);

        // 设置炮弹的速度方向为坦克的前进方向。
        //velocity https://docs.unity.cn/cn/2019.4/ScriptReference/Rigidbody-velocity.html
        //  forward   https://docs.unity.cn/cn/2019.4/ScriptReference/Vector3-forward.html   
        ShellInstance.velocity = m_UpFireButton * m_FireTransform.forward;

        // 改变剪辑射击剪辑并播放它。
        m_ShootAudio.clip = m_FireClip;
        m_ShootAudio.Play();

        // 重置发射部队。这是一种预防措施，以防丢失按钮事件。
        m_UpFireButton = m_MinFire;
    }









}

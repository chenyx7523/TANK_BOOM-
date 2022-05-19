using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour
{
    
    public static ValueManager instance;

    #region    游戏管理数据
    public static int NumberToWin = 2;                           // 获胜回合数。
    public static float StarTime = 0.5f;                        // 延迟0.5s后开始。
    public static float SuspendWait = 1f;                       //暂停结束后继续执行的时间
    public static float EndTime = 1f;                           // 延迟1s后进入下一个对局。
    public static int FirstNumber = 3;                          //暂停倒计时从几开始
    #endregion

    #region       坦克数值类

    //坦克生命值类
    public static float TankStarHealth = 100f;      //  每个坦克开始时的生命值。

    //坦克移动类
    public static float TankSpeed = 10f;         // 坦克的速度配置 
    public static float TurnSpeed = 200f;        // 坦克的旋转速度 

    //炮弹伤害类
    public static float MinFire = 20f;          // 初始给予炮弹的力。
    public static float MaxFire = 40f;          // 在最大充能时间内按动射击按钮给予炮弹的力。
    public static float MaxFireTime = 1f;       // 炮弹最大充能所需时间。
    public static float ReFireTime = 2f;        //重新发射的冷却时间

    #endregion


    #region       黑夜模式太阳移动速度
    public static float SunMoveSpeed = 0.05f;                   //太阳移动速度
    #endregion

    


    //方法引用
    public static ValueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ValueManager>();
            }
            //若还为空则直接生成一个对象并附加脚本
            if (instance == null)
            {
                GameObject ValueManger = new GameObject();
                ValueManger.AddComponent<ValueManager>();
                instance = ValueManger.AddComponent<ValueManager>();
            }
            return instance;
        }

    }

    //游戏运行前执行
    private void Reset()
    {
        
    }

}

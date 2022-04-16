using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;                             // 用来区分普通碰撞物体和坦克，当前为坦克  LayerMask图层，类似与ps中用于分组
    public ParticleSystem m_ShellExplosionParticles;        // 子弹爆炸效果的粒子动画。     
    public AudioSource m_ShellExplosionSound;               // 子弹爆炸时播放的音频。


    public float m_MaxDamage=100f;                          // 子弹最大伤害。
    public float m_Shock=1000f;                             // 爆炸范围的中心的坦克所承受的冲击波推力。
    public float m_ShellDisappearTime = 2f;                 // 子弹碰撞后消失的时间，2s。
    public float m_ShellExplosionRadius = 10f;             //  子弹爆炸的球体半径



    private void Start()
    {
        // 如果到那时还没有被摧毁，那就在它的寿命结束后摧毁。
        Destroy(gameObject, m_ShellDisappearTime);    //https://docs.unity.cn/cn/2019.4/ScriptReference/Object.Destroy.html
    }

    //碰撞一旦发生，则调用OnTriggerEnter方法，比FixedUpdate执行次数小 
    private void OnTriggerEnter(Collider other)       // Collider  所有碰撞体的基类
    {
        // 收集子弹碰撞范围内的所有碰撞体，生成数组     OverlapSphere   https://docs.unity.cn/cn/2019.4/ScriptReference/Physics.OverlapSphere.html
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ShellExplosionRadius, m_TankMask);
                                                   // 爆炸中心              爆炸半径                在那些层查询

        for(int i = 0; i < colliders.Length; i++)
        {
            // 获取他们的刚体。
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            // 如果没有刚体，继续下一个物体。
            if (!targetRigidbody)

                continue;

            // 添加爆炸的推力。 API addExplosionForce  向模拟爆炸效果的刚体施加力
            // https://docs.unity.cn/cn/2019.4/ScriptReference/Rigidbody.AddExplosionForce.html
            targetRigidbody.AddExplosionForce(m_MaxDamage*5, transform.position, m_ShellExplosionRadius);
                                                //中心伤害            位置                 范围

            // 找到与刚体相关的TankHealth脚本。
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            // 根据目标与炮弹的距离计算出目标应该承受的伤害。
            float amount = MakeDamage(targetRigidbody.position);

            //将伤害给坦克
            targetHealth.TankDamage(amount);   

        }
        // 使爆炸粒子动画没有父物体（直接删除会使得下次无法引用）
        m_ShellExplosionParticles.transform.parent = null;
        // 播放粒子系统。
        m_ShellExplosionParticles.Play();
        // 播放爆炸音效。
        m_ShellExplosionSound.Play();

        // 一旦粒子动画播放完成，摧毁它们所在的游戏对象
        ParticleSystem.MainModule mainModule = m_ShellExplosionParticles.main;
        //粒子系统 MainModule 的脚本接口。 https://docs.unity.cn/cn/2019.4/ScriptReference/ParticleSystem.MainModule.html
        Destroy(m_ShellExplosionParticles.gameObject, mainModule.duration);

        Destroy(gameObject);    

    }

    private float MakeDamage (Vector3 targetPosition)
    {
        //定义一个坐标为 目标坐标-子弹爆炸点坐标           即目标和子弹爆炸点的差
        Vector3 ToTarget = targetPosition  - transform.position;

        // 计算炮弹到目标的距离。（即向量长度）爆炸距离
        float ExplosionDistance = ToTarget.magnitude;     // magnitude 向量长度

        // 计算目标和子弹爆炸中心相距(爆炸半径)的比例。
        float i = (m_ShellExplosionRadius - ExplosionDistance) / m_ShellExplosionRadius;

        // 根据最大可能伤害的比例计算伤害。
        float damage = i * m_MaxDamage;

        // 确保最小伤害总是0。
        damage = Mathf.Max(0f, damage);

        return damage;  



    }



































}

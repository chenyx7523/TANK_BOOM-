using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{

    public float m_TankStarHealth = 100;            //  每个坦克开始时的生命值。
    public Slider m_TankHealthSlider;              // 表示坦克当前生命值的滑块。
    public Image m_TankHealthFillImage;                // 滑动块的图像组件。  
    public Color m_FullHealthColor = Color.green;                // 当生命值满时，生命条的颜色。
    public Color m_ZeroHealthColor=Color.red;                // 当没有生命值时，生命条的颜色
    public GameObject m_TankDeathPrefab;           // 一个在Awake中实例化的预制件，当坦克b死亡时使用。


    private AudioSource m_TankDeathAudio;            // 当坦克爆炸时播放的音频源。
    private ParticleSystem m_TankDeathParticle;     // 坦克爆炸的粒子特效。 
    private float m_TankCurrentHealth;              // 坦克当前生命值。
    private bool m_TankIsDead;                      // 判断坦克是否死亡 


    private void Awake()
    {
        //实例化爆炸预制件，并在其上获得粒子系统的参考。 
        m_TankDeathParticle = Instantiate (m_TankDeathPrefab).GetComponent<ParticleSystem>();   //TODO

        //获取实例化预制件(爆炸的粒子特效)上的音频源的引用。 
        m_TankDeathAudio = m_TankDeathParticle.GetComponent<AudioSource>();

        //禁用预制件，这样它就可以在需要的时候被激活。 
        m_TankDeathParticle.gameObject.SetActive(false);    

    }



    private void OnEnable()
    {   
        // 当被启用时，重置坦克的健康状况以及死亡状态。
        m_TankCurrentHealth = m_TankStarHealth;
        m_TankIsDead = false;

        // 更新运行状况滑块的值和颜色。
        UpdateHealthUI();
    }

  


    // 更新运行状况滑块的值和颜色。
    private void UpdateHealthUI()
    {
        //m_TankCurrentHealth = 30f;
        // 将当前声明的值赋值给UI滑块。
        m_TankHealthSlider.value = m_TankCurrentHealth;   
        //m_TankHealthSlider.maxValue = m_TankStarHealth;
        //float health = m_TankCurrentHealth / m_TankStarHealth;      /* m_TankCurrentHealth / m_TankStarHealth;*/        
        //m_TankHealthFillImage.fillAmount = health;

        //[BUG]

        //根据当前血量和满血的百分比，在选定的颜色之间插入条的颜色。
        //Lerp线性插值  https://docs.unity.cn/cn/2019.4/ScriptReference/Color.Lerp.html

        m_TankHealthFillImage.color =
            Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_TankCurrentHealth / m_TankStarHealth);

        //if (m_TankCurrentHealth <= 33)
        //{
        //    m_TankHealthFillImage.color = Color.red;
        //}
    }


    //坦克受伤
    public void TankDamage(float amount)        //amount 即为收到的伤害值
    {

        // 根据造成的伤害减少当前生命值。
        m_TankCurrentHealth -= amount;

        // 更新运行状况滑块的值和颜色。
        UpdateHealthUI();
        
        // 如果当前运行状况为零或低于零且坦克还没死亡，则调用TankDeath方法。 
        if (m_TankCurrentHealth <= 0f && !m_TankIsDead)
        {
            TankDeath();
            
        }

    }



    private void TankDeath()
    {
        // 设置该标志，以便此函数只被调用一次。否则死亡方法一直播放
        m_TankIsDead = true;
        // 移动爆炸预制件到坦克的位置并生效。
        m_TankDeathParticle.transform.position = transform.position;
        m_TankDeathParticle.gameObject.SetActive(true);

        // 播放坦克爆炸的粒子特效和音频。
        m_TankDeathParticle.Play();
        m_TankDeathAudio.Play();    

        gameObject.SetActive(false);    

    }












}

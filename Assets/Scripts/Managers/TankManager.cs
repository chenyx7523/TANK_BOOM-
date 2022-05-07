using System;
using UnityEngine;



/*
 * 用来管理坦克的移动和行为
 * 与ganmemanager互动
 * 决定玩家在何时可以控制自己的坦克
 */
[Serializable]  //强制可序列化（使其在ui界面中显示）
public class TankManager
{


    public Color m_PlayerColor;                             // 这是这个坦克将要生成的颜色。
    public Transform m_Bron;                                // 坦克出生的位置和方向。

    [HideInInspector] public int m_PlayerNumber;            // 用来指定玩家。
    [HideInInspector] public string m_ColoredPlayerText;    // 一串代表玩家的数字，颜色与他们的坦克相匹配。
    [HideInInspector] public GameObject m_Instance;         // 创建对象时对其实例的引用。
    [HideInInspector] public int m_WinTime;                 // 这个玩家到目前为止的胜利次数。


    private TankMovement m_Movement;                        // 参考坦克的移动脚本，用于禁用和启用控制。
    private TankFire m_Fire;                                // 参考坦克的射击脚本，用于禁用和启用控制。
    /*private GameObject m_CanvasGameObject;  */                // 用于在每个回合的开始和结束阶段禁用世界空间UI。

    //初始化（位置，颜色等）
    public void Setup()
    {
        // 获取对组件的引用。
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Fire = m_Instance.GetComponent<TankFire>();
        // 设置玩家号码，使其在脚本中保持一致。
        m_Movement.m_Playernum = m_PlayerNumber;
        m_Fire.m_Playernum = m_PlayerNumber;
        // 使用html富文本创建一个字符串
        //使得玩家代号颜色为玩家颜色
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">玩家 " + m_PlayerNumber + "</color>";
        // 找到坦克的网格渲染组件。       renderers 为实例化后的tank的子对象的所有渲染网格
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();
        // 浏览所有的子模型网格Mesh并附上颜色
        for (int i = 0; i < renderers.Length; i++)
        {
            //将他们的材质颜色设置为该玩家特有的颜色。  
            renderers[i].material.color = m_PlayerColor;

            //TODO
            /*改变材质包
             * public Material m_material
             * Material m_material = new Material (shader .Find ("legcay Shaders /Transparent/diffuse"));
             * GetComponent <Renderer>().m_material = material;
             * 
             * 或
             * gameObject.GetComponent<Render>().material=新的材质,
             * 
             * 老款
             * Public Material myMaterial ; //定义材质类型变量,Public型，从外面拖拽上去//gameObject.renderer.material = myMaterial；
             */


        }
    }


    // 在游戏中玩家不应该有操作的阶段使用。
    public void DisableControl()
    {
        //禁用坦克移动 ， 坦克开火脚本
        m_Movement.enabled = false;          //enabled  启用
        m_Fire.enabled = false;

        //关闭屏幕显示（无意义）
        //m_CanvasGameObject.SetActive(false);
    }


    // 在游戏中玩家应该有操作的阶段使用。
    public void EnableControl()
    {
        m_Movement.enabled = true;        // enabled（启用）
        m_Fire.enabled = true;

        //m_CanvasGameObject.SetActive(true);
    }


    // 在每个回合的开始使用，使坦克进入默认状态。
    public void Reset()
    {
        //默认出生地点
        m_Instance.transform.position = m_Bron.position;
        m_Instance.transform.rotation = m_Bron.rotation;

        //不关闭无法重新激活初始化
        m_Instance.SetActive(false);         //https://docs.unity.cn/cn/2019.4/ScriptReference/GameObject.SetActive.html
        m_Instance.SetActive(true);
    }
}

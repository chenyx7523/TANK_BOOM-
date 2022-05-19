using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSkin : MonoBehaviour
{

    public Color m_PlayerColor;                             // 这是这个坦克将要生成的颜色。
    [HideInInspector] public GameObject m_Instance;         // 创建对象时对其实例的引用。

    //游戏对象
    public GameObject Tank1;
    //材质
    public Material green;
    public Material blue;
    public Material red;
   

    public float num;
    void test1()
    {

        Tank1.GetComponent<MeshRenderer>().sharedMaterial = red;
    }
    void Update()
    {
        num += Time.deltaTime;
        if (num > 2)
        {
            Setup();
            num = 0;
        }

    }
    public void Setup()
    {
        // 找到坦克的网格渲染组件。       renderers 为实例化后的tank的子对象的所有渲染网格
        MeshRenderer[] renderers = Tank1.GetComponentsInChildren<MeshRenderer>();
        // 浏览所有的子模型网格Mesh并附上颜色
        for (int i = 0; i < renderers.Length; i++)
        {
            //将他们的材质颜色设置为该玩家特有的颜色。  
            renderers[i].GetComponent<MeshRenderer>().sharedMaterial = blue;

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










}

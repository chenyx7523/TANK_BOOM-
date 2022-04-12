using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float m_DampTime = 0.2f;     //相机移动并不是立刻的，延迟0.2s后进行移动  
    public float m_Blank = 4f;          // 顶部/底部最目标和屏幕边缘之间的空间。  （屏幕的留白，保证坦克不超出屏幕边缘）
    public float m_MinSize = 7f;        // 防止放的过大，坦克贴近时最小显示范围仍有6.5f       

    private Camera m_Camera;                   // 标记相机 
    private float m_ZoomSpeed;                 // 用来平滑前后（正交）移动 
    private Vector3 m_CameraMove;              // 用来平滑左右移动  
    private Vector3 m_CameraTargerPosition;    // 相机目标到达的地点 （预期的位置）

    public Transform[] m_Targers;             // 摄像机要瞄准的所有目标。（即两个坦克）


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        Move();//将相机移动到需要的位置。
        Zoom();//将相机进行合适的缩放。
    }

    private void Move()
    {
        //获取两个玩家的中心点
        FindAveragePosition();
        //移动到目标位置
        //当前位置  目标到达点  参考变量（ref表示将要回到那个变量） 所用时间 Vector3.SmoothDamp  https://docs.unity.cn/cn/2019.4/ScriptReference/Vector3.SmoothDamp.html
        transform.position = Vector3.SmoothDamp(transform.position, m_CameraTargerPosition, ref m_CameraMove, m_DampTime);
        //当前位置               目标位置             平滑移动      多少时间完成
    }

    //获取两个玩家的中心点
    private void FindAveragePosition()
    {
        //新增一个坐标
        Vector3 averagePos = new Vector3();
        //当前玩家数量为0
        int PlayNumber = 0;
        //遍历每个玩家 
        for (int i = 0; i < m_Targers.Length; i++)
        {
            //将每个玩家的坐标值相加
            averagePos += m_Targers[i].position;
            //每遍历一个，玩家数量加1
            PlayNumber++;

        }
        //如果厂上玩家数大于0
        if (PlayNumber > 0)
        {
            //坐标和÷玩家数量
            averagePos /= PlayNumber;
            //y轴保持不变
            averagePos.y = transform.position.y;
            //赋值给期望移动的坐标
            m_CameraTargerPosition = averagePos;
        }
    }

    //将相机进行合适的缩放。
    private void Zoom()
    {

        // 根据所需的位置找到所需的大小，并平稳地过渡到该大小。 
        float targerSize = FindRequiredSize();
        //orthographicSize(正切角度大小)API https://docs.unity.cn/cn/2019.4/ScriptReference/Camera-orthographicSize.html
        //SmoothDamp随时间推移将一个值逐渐改变为所需目标。  https://docs.unity.cn/cn/2019.4/ScriptReference/Mathf.SmoothDamp.html
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, targerSize, ref m_ZoomSpeed, m_DampTime);
    }                                                    //当前大小               目标大小          平滑缩放       多少时间完成

    //获得一个相机的期望大小并返回（size）  {未理解}
    private float FindRequiredSize()
    {
        //找到相机在空间中的移动位置
        Vector3 m_TargerPosition = transform.InverseTransformPoint(m_CameraTargerPosition);

        //从0开始相机大小的计算
        float size = 0;

        //遍历所有玩家
        for (int i = 0; i < m_Targers.Length; i++)
        {
            // 在相机的局部空间中查找目标的位置
            Vector3 playerPos = transform.InverseTransformPoint(m_Targers[i].position);
            // 从相机的局部空间的期望位置到目标的位置的差。
            Vector3 desiredPosToTarget = playerPos - m_TargerPosition;
            // 从当前的尺寸中选择最大的和坦克“向上”或“向下”距离相机。
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            // 从当前的尺寸和计算的尺寸中选择最大的，基于坦克是在相机的左边还是右边。
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        //给出边缘的空白区域
        size += m_Blank;

        //相机最近不会小于的尺寸
        size = Mathf.Max(size, m_MinSize);

        return size;
    }

    //游戏重新开始后重置摄像机位置
    public void ResectCamera()
    {
        //获取两辆车的平均中心点
        FindAveragePosition();

        // 在没有阻尼的情况下，将相机的位置设置为所需的位置。
        transform.position = m_CameraTargerPosition;

        // 找到并设置所需的相机的正切大小。
        m_Camera.orthographicSize = FindRequiredSize(); 
    }





















}
   



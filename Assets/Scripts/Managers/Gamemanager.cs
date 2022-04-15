﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    public int m_NumberToWin = 5;                           // 获胜回合数。
    public float m_StarDelay = 0.5f;                        // 延迟0.5s后开始。
    public float m_EndDelay = 1f;                           // 延迟1s后进入下一个对局。

    public CameraManager m_CameraManager;                  // 在不同阶段的控制，请参考CameraControl脚本。
    public Text m_Text;                                    // 参考叠加文本显示获胜文本等。
    public GameObject m_TankPrefab;                        // 参考玩家将控制的预制物。
    [SerializeField]  public TankManager[] m_Tank;         // 一组管理器，用于启用和禁用坦克的不同方面。
    


    private int m_RoundNUm;                               // 记录回合数。
    private WaitForSeconds m_StartWait;                   // 开始时的延迟延迟。     WaitForSeconds     https://docs.unity.cn/cn/2019.4/ScriptReference/WaitForSeconds.html
    private WaitForSeconds m_EndWait;                     // 结束后的延迟。
    private TankManager m_RoundWinner;                    // 回合胜利者。
    private TankManager m_GameWinner;                     // 比赛胜利者。


    private void Start()
    {
        // 制造延迟
        m_StartWait = new WaitForSeconds (m_StarDelay);
        m_EndWait = new WaitForSeconds (m_EndDelay);

        AllTank();
        SetCameraTargets();

        // 一旦坦克被创造出来，摄像机将它们作为目标，游戏就开始了。


    }



    private void AllTank()
    {
        // 遍历所有坦克
        for (int i = 0; i < m_Tank.Length; i++)
        {
            // 创建它们，设置它们的玩家编号和控制所需的引用。
            m_Tank[i].m_Instance =                  //在生成点生成
                Instantiate(m_TankPrefab, m_Tank[i].m_Bron.position, m_Tank[i].m_Bron.rotation) as GameObject;

            m_Tank[i].m_Playernum = i + 1;
            m_Tank[i].Setup();

        }
    }

    private void SetCameraTargets()
    {
        // 创建与坦克数量相同大小的转换集合。
        Transform[] targets = new Transform[m_Tank.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            //将其设置为适当的坦克变换。
            targets[i] = m_Tank[i].m_Instance.transform;   //将坦克实例赋值给数组

        }
        // 这些是摄像机应该跟踪的目标。
        m_CameraManager.m_Targets = targets;     //相机跟踪数组元素

    }


    private IEnumerator GameLoop()
    {
        // 首先运行“RoundStarting”协程，但在它完成之前不要返回。
        yield return StartCoroutine(RoundStarting());

        // 一旦“RoundStarting”协程完成，运行“RoundPlaying”协程，但在它完成之前不要返回。
        yield return StartCoroutine(RoundPlaying());

        // 一旦执行返回到这里，运行“RoundEnding”协程，同样，在它完成之前不要返回。
        yield return StartCoroutine(RoundEnding());

        // 这段代码直到“RoundEnding”完成才会运行。 在这一点上，检查是否找到了游戏赢家。
        if (m_GameWinner != null)
        {

            SceneManager.LoadScene(0);

        }
        else
        {
            // 如果还没有赢家，重新启动这个协程，循环继续。
            //请注意，这个协程不会屈服。这意味着当前版本的GameLoop将会结束。 Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
            StartCoroutine(GameLoop());
        }        

    }


    private IEnumerator RoundStarting()
    {
        // 一旦回合开始重置坦克，确保它们不能移动。
        ResetAllTanks();
        DisableTankControl();

        // 将相机的变焦和位置调整到适合重置坦克的位置。
        m_CameraManager.ResectCamera();

        // 增加整数，并显示文本显示玩家它是多少。
        m_RoundNUm++;
        m_Text.text = "ROUND " + m_RoundNUm;

        // 等待指定的时间长度，直到将控制权交还给游戏循环。
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        // 当这一轮游戏开始时，让玩家控制坦克。
        EnableTankControl();

        // 清除屏幕上的文本。
        m_Text.text = string.Empty;

        // 现在已经没有坦克了…
        while (!OneTankLeft())
        {
            // 下一帧返回。
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        // 阻止坦克移动。
        DisableTankControl();

        //清除前一回合的赢家
        m_RoundWinner = null;

        // 现在回合结束了，看看是否有赢家。
        m_RoundWinner = GetRoundWinner();

        // 如果有赢家，增加他们的分数。
        if (m_RoundWinner != null)
            m_RoundWinner.m_WinTime++;

        //现在胜者的分数增加了，看看是否有人拥有这款游戏。
        m_GameWinner = GetGameWinner();

        // 获得基于分数和是否有游戏赢家的消息，并显示它。
        string message = EndMessage();
        m_Text.text = message;

        // 等待指定的时间长度，直到将控制权交还给游戏循环。
        yield return m_EndWait;
    }


    // 这是用来检查是否有一个或更少的坦克剩余，从而一轮应该结束。
    private bool OneTankLeft()
    {
        // 从零开始计算剩下的坦克数量。
        int numTanksLeft = 0;

        // 
        for (int i = 0; i < m_Tank.Length; i++)
        {
            // 如果它们是活动的，则增加计数器。
            if (m_Tank[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        // 如果只剩一个或更少的坦克，则返回true，否则返回false。
        return numTanksLeft <= 1;
    }


    // 这个函数是为了找出回合中是否有赢家。
    // 这个函数是在假设当前只有1个或更少的坦克处于活动状态时调用的。
    private TankManager GetRoundWinner()
    {
        // 
        for (int i = 0; i < m_Tank.Length; i++)
        {
            //如果其中一个是有效的，它是赢家，所以返回它。
            if (m_Tank[i].m_Instance.activeSelf)
                return m_Tank[i];
        }

        // 如果没有坦克是活动的，这是一个绘图，所以返回null。
        return null;
    }


    // 这个函数是为了找出游戏中是否有赢家。
    private TankManager GetGameWinner()
    {

        for (int i = 0; i < m_Tank.Length; i++)
        {
            // 如果其中一个有足够积分，返回。
            if (m_Tank[i].m_WinTime == m_NumberToWin)
                return m_Tank[i];
        }

        // 大家都没有积分则返回空
        return null;
    }


    // 返回字符串消息，在每一轮结束时显示。
    private string EndMessage()
    {
        // 默认情况下，当一轮结束时没有赢家，所以默认结束消息是平局。
        string message = "DRAW!";

        // 如果有赢家，那么就改变信息来反映这一点。
        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColorPlayerName + " WINS THE ROUND!";

        // 在初始消息后添加一些换行符。
        message += "\n\n\n\n";


        for (int i = 0; i < m_Tank.Length; i++)
        {
            //通过所有的坦克，并将他们的分数添加到信息中。
            message += m_Tank[i].m_ColorPlayerName + ": " + m_Tank[i].m_WinTime + " WINS\n";   //（总分信息）
        }

        // 如果游戏中有赢家，那就改变整个信息来反映这一点。
        if (m_GameWinner != null)
            message = m_GameWinner.m_ColorPlayerName + " WINS THE GAME!";  //（当前回合信息）

        return message;
    }


    // 这个功能是用来打开所有的坦克和重置他们的位置和属性。
    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tank.Length; i++)
        {
            m_Tank[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tank.Length; i++)
        {
            m_Tank[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tank.Length; i++)
        {
            m_Tank[i].DisAbleControl();
        }
    }
}




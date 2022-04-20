using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        public int m_NumberToWin = 5;                           // 获胜回合数。
        public float m_StarTime = 0.5f;                        // 延迟0.5s后开始。
        public float m_EndTime = 1f;                           // 延迟1s后进入下一个对局。

        public CameraManager m_CameraManager;                    // 在不同阶段的控制，请参考CameraControl脚本。
        public Text m_Text;                                    // 参考叠加文本显示获胜文本等。
        public GameObject m_TankPrefab;                        // 参考玩家将控制的预制物。
        public TankManager[] m_Tank;                           // 一组管理器，用于启用和禁用坦克的不同方面。


        private int m_RoundNUm = 0;                               // 记录回合数。
        private WaitForSeconds m_StartWait;                   // 开始时的延迟延迟。     WaitForSeconds     https://docs.unity.cn/cn/2019.4/ScriptReference/WaitForSeconds.html
        private WaitForSeconds m_EndWait;                     // 结束后的延迟。
        private TankManager m_RoundWinner;                    // 回合胜利者。
        private TankManager m_GameWinner;                     // 比赛胜利者。


        private void Start()
        {
            // 制造延迟
            m_StartWait = new WaitForSeconds(m_StarTime);
            m_EndWait = new WaitForSeconds(m_EndTime);

            AllTank();
            SetCameraTargets();

            // 一旦坦克被创造出来，摄像机将它们作为目标，游戏就开始了。(执行协程)
            StartCoroutine(GameLoop());

        }



        private void AllTank()
        {
            // 遍历所有坦克
            for (int i = 0; i < m_Tank.Length; i++)
            {
                // 创建它们，设置它们的玩家编号和控制所需的引用。
                m_Tank[i].m_Instance =                  //在生成点生成
                    Instantiate(m_TankPrefab, m_Tank[i].m_Bron.position, m_Tank[i].m_Bron.rotation);

                m_Tank[i].m_PlayerNumber = i + 1;
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
            // 这些是摄像机应该跟踪的目标。将坐标传入Camera中后续调用
            m_CameraManager.m_Targets = targets;     //相机跟踪数组元素

        }

        // BUG 完全未执行
        // 这将在游戏开始时调用，并将一个接一个地运行游戏的每个阶段。IEnumerator(协程)
        private IEnumerator GameLoop()
        {
            // 首先运行“RoundStarting”协程，但在它完成之前不要返回。
            //Debug.Log("yield return StartCoroutine(RoundStarting());");
            yield return StartCoroutine(RoundStarting());            // API   https://docs.unity.cn/cn/2019.4/ScriptReference/MonoBehaviour.StartCoroutine.html

            // 一旦“RoundStarting”协程完成，运行“RoundPlaying”协程，但在它完成之前不要返回。
            yield return StartCoroutine(RoundPlaying());

            // 一旦执行返回到这里，运行“RoundEnding”协程，同样，在它完成之前不要返回。
            yield return StartCoroutine(RoundEnding());

            // 这段代码直到“RoundEnding”完成才会运行。 在这一点上，检查是否找到了游戏赢家。
            if (m_GameWinner != null)
            {

                SceneManager.LoadScene(1);

            }
            else
            {
                // 如果还没有赢家，重新启动这个协程，循环继续。
                //请注意，这个协程不会结束。这意味着当前版本的GameLoop将会结束。 Note that this coroutine doesn't yield.  This means that the current version of the GameLoop will end.
                StartCoroutine(GameLoop());
            }

        }


        private IEnumerator RoundStarting()
        {
            // 这个功能是用来打开所有的坦克和重置他们的位置和属性。
            ResetAllTanks();
            //禁止玩家操作和启用tank功能
            DisableTankControl();

            //重置摄像机位置，初始化
            m_CameraManager.ResectCamera();

            // 增加整数，并显示文本显示玩家它是多少。
            m_RoundNUm++;
            m_Text.text = "回合 " + m_RoundNUm;
            //Debug.Log("star");

            // 等待指定的时间长度，直到将控制权交还给游戏循环。
            yield return m_StartWait;
        }


        private IEnumerator RoundPlaying()
        {
            // 当这一轮游戏开始时，让玩家控制坦克。
            EnableTankControl();

            // 清除屏幕上的文本。
            m_Text.text = string.Empty;
            //Debug.Log("playing");
            // 直到没有坦克
            while (!RemainTank())
            {
                // 下一帧返回。
                yield return null;
            }
        }


        private IEnumerator RoundEnding()
        {
            // 阻止玩家对坦克的操作。
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
        private bool RemainTank()
        {
            // 从零开始计算剩下的坦克数量。
            int remainTank = 0;

            // 
            for (int i = 0; i < m_Tank.Length; i++)
            {
                // 如果它们是活动的，则增加计数器。
                if (m_Tank[i].m_Instance.activeSelf)
                    remainTank++;
            }

            // 如果只剩一个或更少的坦克，则返回true，否则返回false。
            return remainTank <= 1;
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
            string message = "平局啦!";

            // 如果有赢家，那么就改变信息来反映这一点。
            if (m_RoundWinner != null)
                message = m_RoundWinner.m_ColoredPlayerText + " 是本回合赢家!\n太厉害啦!";

            // 在初始消息后添加一些换行符。
            message += "\n\n\n\n";


            for (int i = 0; i < m_Tank.Length; i++)
            {
                //通过所有的坦克，并将他们的分数添加到信息中。
                message += m_Tank[i].m_ColoredPlayerText + "  :  " + m_Tank[i].m_WinTime + " 回合\n";   //（总分信息）
            }

            // 如果游戏中有赢家，那就改变整个信息来反映这一点。
            if (m_GameWinner != null)
                message = m_GameWinner.m_ColoredPlayerText + " 获得了本局胜利!";  //（当前回合信息）

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

        //允许玩家操作和启用tank功能
        private void EnableTankControl()
        {
            for (int i = 0; i < m_Tank.Length; i++)
            {
                m_Tank[i].EnableControl();
            }
        }

        //禁止玩家操作和启用tank功能
        private void DisableTankControl()
        {
            for (int i = 0; i < m_Tank.Length; i++)
            {
                m_Tank[i].DisableControl();
            }
        }
    }

}






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
        [ReadOnly] public int m_NumberToWin;                           // 获胜回合数。
        [ReadOnly] public float m_StarTime;                        // 延迟0.5s后开始。
        [ReadOnly] public float m_SuspendWait;                       //暂停结束后继续执行的时间
        [ReadOnly] public float m_EndTime;                           // 延迟1s后进入下一个对局。



        public CameraManager m_CameraManager;                    // 在不同阶段的控制，请参考CameraControl脚本。
        public ScenesManager m_ScenesManager;
        public Text m_Text;                                    // 参考叠加文本显示获胜文本等。
        public GameObject m_TankPrefab;                        // 参考玩家将控制的预制物。
        public TankManager[] m_Tank;                           // 一组管理器，用于启用和禁用坦克的不同方面。

        public GameObject m_SpendPage;                        //暂停页面
        public GameObject m_SpendTime;                        //倒计时界面
        [HideInInspector] public bool Suspending;                              //暂停状态
        [HideInInspector] public bool IsEnding;                                //是否在结算界面



        private int m_RoundNUm = 0;                           // 记录回合数。
        private WaitForSeconds m_StartWait;                   // 开始时的延迟延迟。     WaitForSeconds     https://docs.unity.cn/cn/2019.4/ScriptReference/WaitForSeconds.html
        private WaitForSeconds m_EndWait;                     // 结束后的延迟。
        private TankManager m_RoundWinner;                    // 回合胜利者。
        private TankManager m_GameWinner;                     // 比赛胜利者。


        public GameObject m_EndPage;                          //对局结束界面
        public Text m_EndText;                               //对局最终信息


        [HideInInspector] public int m_SceneNumber;          //记录一下场景序号

        private void Start()
        {
            m_NumberToWin = ValueManager.NumberToWin;
            m_StarTime = ValueManager.StarTime;
            m_SuspendWait = ValueManager.SuspendWait;
            m_EndTime = ValueManager.EndTime;

            // 初始化延迟
            //API waitForSeconds   延迟执行协程
            //https://docs.unity.cn/cn/2019.4/ScriptReference/WaitForSeconds.html
            m_StartWait = new WaitForSeconds(m_StarTime);             
            m_EndWait = new WaitForSeconds(m_EndTime);


            //获取当前加载的场景序号
            
            m_SceneNumber = m_ScenesManager.SceneNumber();

            //实例化生成两个坦克
            AllTank();
            //相机的位置部署
            SetCameraTargets();
            //初始化暂停状态
            Suspending = false;
            //float a = ValueManager.Instance.testNumber;
            //Debug.Log(ValueManager.Instance.testNumber);
            //Debug.Log(ValueManager.testNumber);


            // 游戏就开始了。(执行协程)
            StartCoroutine(GameLoop());
            


        }
        //当前仅用来做暂停判断
        private void Update()
        {
            //是否按下暂停键
            IsRoundSuspend();
            //SuspendEndbool();
        }

        //实例化生成两个坦克
        private void AllTank()
        {
            // 遍历所有坦克
            for (int i = 0; i < m_Tank.Length; i++)
            {
                // 创建它们，设置它们的玩家编号和控制所需的引用。
                m_Tank[i].m_Instance =                  //在生成点生成              API Instantiate  克隆     https://docs.unity.cn/cn/2019.4/ScriptReference/Object.Instantiate.html
                    Instantiate(m_TankPrefab, m_Tank[i].m_Bron.position, m_Tank[i].m_Bron.rotation);

                m_Tank[i].m_PlayerNumber = i + 1;
                m_Tank[i].Setup();  //坦克初始化
            }
        }

        //讲坦克的参数传递给跟踪的相机
        private void SetCameraTargets()
        {
            // 创建与坦克数量相同大小的转换集合。
            Transform[] targets = new Transform[m_Tank.Length];

            for (int i = 0; i < targets.Length; i++)
            {
                //将其设置为适当的坦克变换。
                targets[i] = m_Tank[i].m_Instance.transform;   //将坦克实例的位置赋值给数组

            }
            // 这些是摄像机应该跟踪的目标。将坐标传入Camera中后续调用
            m_CameraManager.m_Targets = targets;     //相机跟踪数组元素

        }

        // 
        // 这将在游戏开始时调用，并将一个接一个地运行游戏的每个阶段。IEnumerator(协程)
        private IEnumerator GameLoop()
        {
            //游戏的初始化
            yield return StartCoroutine(RoundStarting());            // API StartCoroutine   https://docs.unity.cn/cn/2019.4/ScriptReference/MonoBehaviour.StartCoroutine.html

            //游戏运行中
            yield return StartCoroutine(RoundPlaying());

            // 回合结束的判定
            yield return StartCoroutine(RoundEnding());

            // 后续代码直到“RoundEnding”完成才会运行。 在这一点上，检查是否找到了游戏赢家。
            //有玩家则进入游戏结束阶段，
            if (m_GameWinner != null)
            {
                //TODO
                //弹出退出或重新开始窗口
                m_Text.text = string.Empty;
                m_EndText.text = EndMessage();
                m_EndPage.SetActive(true);
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

            // 设定回合数，每次回合开始回合数++
            m_RoundNUm++;
            m_Text.text = "回合 " + m_RoundNUm;
            //Debug.Log("star");

            // 等待指定的时间长度，执行下一个协程
            yield return m_StartWait;
        }

        //游戏进行中
        private IEnumerator RoundPlaying()
        {

            // 游戏正式开始，让玩家控制坦克。
            EnableTankControl();
            //是否在回合结束计分状态 （false）   主要用于防止在计分状态暂停游戏出现bug
            IsEnding = false;   

            // 清除屏幕上的文本。
            m_Text.text = string.Empty;
            //Debug.Log("playing");
            // 直到没有坦克
            while (!IsOnlyOneTank())  //只剩一个的时候执行接下来的协程
            {
                // 下一帧返回。
                yield return null;
            }
        }

        //private IEnumerator RoundSuspend()
        //{
        //    yield return m_SuspendWait;
        //}

        private IEnumerator RoundEnding()
        {
            //防止结算中暂停   结算界面正在播放
            IsEnding = true;    
            // 阻止玩家对坦克的操作。
            DisableTankControl();

            //清除前一回合的赢家
            m_RoundWinner = null;

            // 现在回合结束了，看看是否有赢家。
            m_RoundWinner = GetRoundWinner();

            // 如果有赢家，增加他们的分数。
            if (m_RoundWinner != null)
                m_RoundWinner.m_WinTime++;

            //现在胜者的分数增加了，看看是否有人攒够积分
            m_GameWinner = GetGameWinner();

            // 显示回合信息
            string message = GameMessage();
            m_Text.text = message;
            // 等待指定的时间长度，直到将控制权交还给游戏循环。
            yield return m_EndWait;
        }


        // 判断是否只剩下一个坦克，是则返回true
        private bool IsOnlyOneTank()
        {
            // 从零开始计算剩下的坦克数量。
            int remainTank = 0;              //remain   剩余  

            // 
            for (int i = 0; i < m_Tank.Length; i++)
            {
                // 如果它们是活动的，则增加计数器。
                if (m_Tank[i].m_Instance.activeSelf)
                    remainTank++;
            }

            // 如果只剩一个或更少的坦克，则返回true，都活着则false。
            return remainTank <= 1;
        }


        // 获取赢家坦克
        // 
        private TankManager GetRoundWinner()
        {
            // 
            for (int i = 0; i < m_Tank.Length; i++)
            {
                //存在活着的坦克，则它是赢家。
                if (m_Tank[i].m_Instance.activeSelf)
                    return m_Tank[i];
            }

            // 如果没有赢家，返回null。
            return null;
        }


        // 是否有玩家赢得五次回合
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


        // 返回字符串消息，用于胜利结果显示
        private string GameMessage()
        {
            // 默认情况下，当一轮结束时没有赢家，所以默认结束消息是平局。  若有则被覆盖
                string message = "平局啦!";
                

            // 显示赢家信息    玩家编号 + 是本回合赢家
            
                message = m_RoundWinner.m_ColoredPlayerText + " 是本回合赢家!\n太厉害啦!";

            // 在初始消息后添加一些换行符。
            message += "\n\n\n\n";


            for (int i = 0; i < m_Tank.Length; i++)
            {
                //通过所有的坦克，并将他们的分数添加到信息中。
                message += m_Tank[i].m_ColoredPlayerText + "  :  " + m_Tank[i].m_WinTime + " 回合\n";   //（总分信息）
            }

            // 存在整局赢家，输出赢家编号 + 获得胜利


            return message;



        }
        private string EndMessage()
        {

            string endmessage = m_GameWinner.m_ColoredPlayerText + " 获得了本局胜利!";  
            return endmessage;

        }


        // 这个功能是用来打开所有的坦克和重置他们的位置和属性。（每回合后初始化）
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

        //    private void test()
        //    {
        //        TankHealth testTankHealth = new TankHealth();
        //        testTankHealth.TankDamage(3f);
        //        testTankHealth.m_FullHealthColor = Color.green;

        //    }


        //控制游戏暂停
        private void IsRoundSuspend()
        {
            //esc被按下，游戏暂停
            if (Input.GetKeyUp(KeyCode.Escape)&& !IsEnding)
            {
                if (!Suspending)
                {
                    //暂停坦克控制
                    DisableTankControl();
                    //SuspendText.text = string.Empty;
                    m_SpendPage.SetActive(true);
                    Suspending = true;
                }
                else if (Suspending)  //暂停中,准备结束暂停
                {
                    m_SpendPage.SetActive(false);
                    m_SpendTime.SetActive(true);
                    DisableTankControl();

                    //延时三秒实现--恢复坦克运动
                    Invoke("SuspendEnd", 4f);
                }
            }
        }
        //结束暂停状态   （在 IsRoundSuspend 中延时三秒调用）
        private void SuspendEnd()
        {
            //恢复坦克运动
            EnableTankControl();
            //暂停状态改为false
            Suspending = false;
        }

        //是否在暂停中 （弃用，有BUG）
        public bool IsSuspend()
        {
            return Suspending;
        }

































    }

}






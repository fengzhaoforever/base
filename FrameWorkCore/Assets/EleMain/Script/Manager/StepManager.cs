using System.Text;
using System;

public class StepManager  {

    public enum StepState
    {
        /// <summary>
        /// 初始状态
        /// </summary>
        None,
        /// <summary>
        /// 仪器展示阶段
        /// </summary>
        Introdece,
        /// <summary>
        /// 操作阶段
        /// </summary>
        Gameing,
        /// <summary>
        /// 实验结束
        /// </summary>
        GameOver,
    }
    #region 单例模式
    private static StepManager _instance;
    public static StepManager Instance()
    {
        if (_instance == null)
            _instance = new StepManager();
        return _instance;
    }
    private StepManager()
    {
        
        GameState = StepState.None;
        _configStepDesc = (ConfigStepDesc)ConfigManager.Instance().GetConfig(ConfigType.ConfigStepDesc);
    }
    #endregion
    public StepState GameState { get; private set; }
    
    private int _curStepIndex = 0;
    /// <summary>
    /// 从1开始
    /// </summary>
    public int CurSetpIndex
    {
        get { return _curStepIndex<1?1:_curStepIndex; }
        private set
        {
            _curStepIndex = value>5?5:value;
            if (value > 5)
            {
                ChangeGameState(StepState.GameOver);
                if (EventBase.Instance().DoGameOver != null)
                    EventBase.Instance().DoGameOver();
                UIManager.Instance().ShowPanel(UIManager.PanelType.PanelGameOver);
            }
            else
            {
                EventBase.Instance().StepToNext();
            }
        }
    }
    public  DateTime OverTime;
    private ConfigStepDesc _configStepDesc;
    
    /// <summary>
    /// 获取当前步骤描述
    /// </summary>
    /// <returns></returns>
    public string GetCurStepDesc()
    {
        return _configStepDesc.Get(CurSetpIndex).Desc;
    }
    /// <summary>
    /// 获取指定步骤描述
    /// </summary>
    /// <param name="index">从0开始</param>
    /// <returns></returns>
    public string GetCurStepDesc(int index)
    {
        return _configStepDesc.Get(index+1).Desc;
    }
    /// <summary>
    /// 获取所有步骤描述
    /// </summary>
    /// <returns>格式化显示</returns>
    public string GetAllDesc()
    {
        
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 5; i++)
        {
            if (CurSetpIndex - 1 == i)
            {
                sb.AppendFormat("<color=red>{0}</color>\n", _configStepDesc.Get(i+1).Desc);
            }
            else
            {
                sb.AppendFormat("{0}\n", _configStepDesc.Get(i+1).Desc);
            }
        }
        return sb.ToString();
    }

    public void StepToNext()
    {
        CurSetpIndex++;
        
    }
    public string GetScore()
    {
        return GameState== StepState .GameOver? "100":((CurSetpIndex - 1) * 20).ToString();
    }

    public string GetLastTime()
    {
        TimeSpan ts = OverTime- DateTime.Now;
        string str = ts.Minutes + ":" + ts.Seconds;
        return str;
    }

    public void ChangeGameState(StepState state)
    {
        GameState = state;
        if (state == StepState.Gameing)
        {
            EventBase.Instance().StepToNext();
            OverTime = DateTime.Now.AddSeconds(1800);
        }
    }

}

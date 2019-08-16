using UnityEngine;
public class EventBase  {
    private EventBase() { }
    private static EventBase _instance;
    public static EventBase Instance()
    {
        if (_instance == null)
            _instance = new EventBase();
        return _instance;
    }
    public delegate void CloseOtherTweenBar(int id);
    /// <summary>
    /// 关闭其他下拉菜单栏
    /// </summary>
    public CloseOtherTweenBar EveCloseOtherTweenBar;

    public delegate void StepNext();
    /// <summary>
    /// 流程控制：下一步
    /// </summary>
    public StepNext StepToNext;

    public delegate void GameOver();
    /// <summary>
    /// 实验结束
    /// </summary>
    public GameOver DoGameOver;

    public delegate void EleIntroduceMove();
    /// <summary>
    /// 仪器展示
    /// </summary>
    public EleIntroduceMove DoEleIntroduceMove;

    public delegate float DelTest();
    public DelTest DoDelTest;

    public delegate void EventTest(GameObject go,string str);
    public event EventTest eventTest;
    public void DoEvent(GameObject go,string str)
    {
        eventTest(go, str);
    }
    public delegate float EventTestTime(string str);
    public event EventTestTime eventTestTime;
    public void DoEventTestTime(string str)
    {
        Debug.LogWarning(eventTestTime(str));
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 排列顺序：系统 教学 信息 视角
/// </summary>
public class PanelTopBar : MonoBehaviour {
    public class StepInfo
    {
        public GameObject Go;
        public Text Name;
        public Text CurDesc;
        public Text AllDesc;
        public StepInfo(Transform tran)
        {
            Go = tran.gameObject;
            Name = tran.Find("Name").GetComponent<Text>();
            CurDesc = tran.Find("CurDesc").GetComponent<Text>();
            AllDesc = tran.Find("AllDesc").GetComponent<Text>();
            EventTrigger trigger = CurDesc.GetComponent<EventTrigger>();
            EventTrigger.Entry entity = new EventTrigger.Entry();
            entity.eventID = EventTriggerType.PointerClick;
            entity.callback.AddListener(ShowOrHideAllDesc);
            trigger.triggers.Add(entity);
            SetName("电极抛光");
            Go.SetActive(false);
        }

        private void ShowOrHideAllDesc(BaseEventData arg0)
        {
            if (AllDesc.gameObject.activeInHierarchy)
            {
                AllDesc.gameObject.SetActive(false);
            }
            else
            {
                AllDesc.gameObject.SetActive(true);
                string str = StepManager.Instance().GetAllDesc();
                SetAllDesc(str);
            }
        }
        public void ShowOrHide(bool close = false)
        {
            if (Go.activeInHierarchy||close)
            {
                if (AllDesc.gameObject.activeInHierarchy)
                {
                    AllDesc.gameObject.SetActive(false);
                }
                Go.SetActive(false);
            }
            else
            {
                EventBase.Instance().EveCloseOtherTweenBar(-1);
                Go.SetActive(true);
                string str = StepManager.Instance().GetCurStepDesc();
                SetCurDesc(str);//(Regex.Unescape(str));
            }
                
        }
        public void SetName(string str)
        {
            Name.text = str;
        }
        public void SetCurDesc(string str)
        {
            CurDesc.text = str;
        }
        public void SetAllDesc(string str)
        {
            AllDesc.text = str;
        }

        internal void UpdateDesc()
        {
            string str = StepManager.Instance().GetCurStepDesc();
            SetCurDesc(str);
            string strAll = StepManager.Instance().GetAllDesc();
            SetAllDesc(strAll);
        }
    }
    private int _tweenBarId;
    public  int TweenBarId { get { return _tweenBarId++; } }
    public Button[] Btns = new Button[5];
    public Transform[] Trans = new Transform[4];
    public Transform ChangeColor;
    public Text Score;
    public Text LeftTime;
    private float _allTimer = 1800f;
    private float _countDowner;
    private TweenBar[] _tweenBars = new TweenBar[4];
    private TweenBar _changeColor;
    private Image TweenBarBg;
    private StepInfo _stepInfo;
    void Start () {
        Score.text = "0";
        LeftTime.text = "30:00";
        for (int i = 0; i < 4; i++)
        {
            if (i == 1)//教学按钮跳过
                continue;
            if (!Trans[i])
                continue;
            TweenBar tempBar = new TweenBar(Trans[i], TweenBarId);
            _tweenBars[i] = tempBar;
            Button temp = Btns[i];
            temp.onClick.AddListener(() => tempBar.Tween());
        }
        Btns[1].onClick.AddListener(() => 
        {
            EventBase.Instance().EveCloseOtherTweenBar(-1);
            UIManager.Instance().ShowPanel(UIManager.PanelType.PanelShowDes,false);
        });
        _changeColor = new TweenBar(ChangeColor, TweenBarId);
        TweenBarBg = transform.Find("Bg").GetComponent<Image>();
        _changeColor.AddCallToBtn(0, () => TweenBarBg.color = Color.red);
        _changeColor.AddCallToBtn(1, () => TweenBarBg.color = Color.green);
        _changeColor.AddCallToBtn(2, () => TweenBarBg.color = Color.white);
        _changeColor.AddCallToBtn(3, () => TweenBarBg.color = Color.Lerp(Color.red, Color.white, 0.5f));
        _tweenBars[0].AddCallToBtn(1, _changeColor);
        _tweenBars[0].AddCallToBtn(2, () => UIManager.Instance().ShowPanel(UIManager.PanelType.PanelQuit));

        _stepInfo = new StepInfo(transform.Find("StepInfo"));
        Btns[4].onClick.AddListener(() => _stepInfo.ShowOrHide());
        EventBase.Instance().EveCloseOtherTweenBar += CloseOtherTweenBar;
        EventBase.Instance().StepToNext += _stepInfo.UpdateDesc;
        EventBase.Instance().StepToNext += UpdateScore;
        EventBase.Instance().DoGameOver += UpdateScore;
    }

    private void Update()
    {
        if (StepManager.Instance().GameState == StepManager.StepState.Gameing)
        {
            if (_countDowner > 0)
            {
                _countDowner -= Time.deltaTime;
            }
            else
            {
                LeftTime.text = StepManager.Instance().GetLastTime();
                _countDowner = 1;
            }
        }
    }

    private void UpdateScore()
    {
        Score.text = StepManager.Instance().GetScore();
    }
    /// <summary>
    /// 关闭其他的下拉菜单
    /// </summary>
    /// <param name="id"></param>
    private void CloseOtherTweenBar(int id)
    {
        for (int i = 0; i < _tweenBars.Length; i++)
        {
            TweenBar tempBar = _tweenBars[i];
            if (tempBar != null)
                if (tempBar.ID != id)
                    tempBar.Tween(true);
        }
        if(id>=0)
        {
            _stepInfo.ShowOrHide(true);
        }
    }

    private void OnDestroy()
    {
        EventBase.Instance().EveCloseOtherTweenBar -= CloseOtherTweenBar;
        EventBase.Instance().StepToNext -= _stepInfo.UpdateDesc;
        EventBase.Instance().StepToNext -= UpdateScore;
        EventBase.Instance().DoGameOver -= UpdateScore;
    }

}

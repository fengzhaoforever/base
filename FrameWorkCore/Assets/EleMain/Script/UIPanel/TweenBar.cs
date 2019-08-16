using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// 菜单栏
/// </summary>
public class TweenBar  {
    public int ID;
    public GameObject  Go;
    public Button[] Btns;
    private bool _isOut { get; set; }
    private bool _isShow { get; set; }
    private float _tweenDistance;
    private float _cellHigh;
    private readonly float _topBarHigh;
    private float _defaultY;
    private List<TweenBar> _childTweens;
    public TweenBar(Transform tran,int id)
    {
        ID = id;
        _topBarHigh = 32;
        Go = tran.gameObject;
        Btns = new Button[tran.childCount];
        for (int i = 0; i < tran.childCount; i++)
        {
            Btns[i] = tran.GetChild(i).GetComponent<Button>();
        }
        GridLayoutGroup grid = tran.GetComponent<GridLayoutGroup>();
        _cellHigh = grid.cellSize.y + grid.spacing.y;
        _tweenDistance = _cellHigh* tran.childCount + _topBarHigh;
        _defaultY = tran.localPosition.y;
        _childTweens = new List<TweenBar>();
    }
    /// <summary>
    /// 弹出 收回
    /// </summary>
    public void Tween(bool close = false)
    {
        if(close)
        {
            TweenUp();
            _isOut = false;
        }
        else
        {
            if (_isOut)
                TweenUp();
            else
                TweenDown();
            _isOut = !_isOut;
        }
        
        if(!_isOut)
        {
            if (_childTweens.Count > 0)
            {
                for (int i = 0; i < _childTweens.Count; i++)
                {
                    _childTweens[i].ShowOrHide(true);
                }
            }
        }
    }
    /// <summary>
    /// 弹出
    /// </summary>
    private void TweenDown()
    {
        Go.transform.DOLocalMoveY(_defaultY - _tweenDistance, 0.3f);
        EventBase.Instance().EveCloseOtherTweenBar(ID);
    }
    /// <summary>
    /// 收回
    /// </summary>
    private void TweenUp()
    {
       
        Go.transform.DOLocalMoveY(_defaultY, 0.3f);
    }
    /// <summary>
    /// 显示 隐藏
    /// </summary>
    public void ShowOrHide(bool close = false)
    {
        if (!close)
        {
            if (_isShow)
                Go.SetActive(false);
            else
                Go.SetActive(true);
            _isShow = !_isShow;
        }
        else
        {
            Go.SetActive(false);
            _isShow = false;
        }

    }
    /// <summary>
    /// 给指定的按钮添加监听方法
    /// </summary>
    /// <param name="index"></param>
    /// <param name="call"></param>
    public void AddCallToBtn(int index,TweenBar tween)
    {
        Btns[index].onClick.AddListener(() => tween.ShowOrHide());
        if (!_childTweens.Contains(tween))
            _childTweens.Add(tween);
    }
    public void AddCallToBtn(int index, UnityAction call)
    {
        Btns[index].onClick.AddListener(call);
    }
}


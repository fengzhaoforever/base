using UnityEngine;
using System.Collections;
using DG.Tweening;
public class MoveControl : MonoBehaviour
{
    private RaycastHit[] _hit;
    //private GameObject _go;
    private Vector3 _goScreenPos;
    private Vector3 _offset;
    private Vector3 _offsetCamera;
    private EleItem _curDragEleItem;
    private EleItem _curCastEleItem;
    private bool _mouseDown;
    private int _eleIntroduceMoveIndex = 0;
    public Transform DefaultTran;
    void Start()
    {
        EventBase.Instance().StepToNext += StepNext;
        EventBase.Instance().DoEleIntroduceMove += DoEleIntroduceMove;
    }

    private void DoEleIntroduceMove()
    {
        StepManager.Instance().ChangeGameState(StepManager.StepState.Introdece);
        StartCoroutine(EleIntroduceMove());
    }

    private void Update()
    {
        if (StepManager.Instance().GameState == StepManager.StepState.Gameing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseDown = true;
                Debug.Log("OnMouseDown0();");
                OnMouseDown0();
            }
            if (Input.GetMouseButtonUp(0))
            {
                _mouseDown = false;
                Debug.Log("OnMouseUp0();");
                OnMouseUp0();
            }
            if (_mouseDown)
            {
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {

                    Debug.Log("OnMouseDrag0();");
                    OnMouseDrag0();
                }
                else
                {

                }
            }
        }

    }

    private void StepNext()
    {
        EleDataBase.Instance().GetCurStepItem().CurDragEleItem.SetHighLighting(true);
    }

    private void OnMouseDown0()
    {
        _curDragEleItem = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, (1 << 8)))
        {
            _curDragEleItem = EleDataBase.Instance().GetOneEleItem(hit.collider.name);
            if (EleDataBase.Instance().IsCurDragTarget(_curDragEleItem))
            {
                _goScreenPos = Camera.main.WorldToScreenPoint(_curDragEleItem.Go.transform.position);
                _offset = _curDragEleItem.Go.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _goScreenPos.z));
                _curCastEleItem = EleDataBase.Instance().GetCurStepItem().CurCastEleItem;
                Debug.Log("_curDragEleItem" + _curDragEleItem.Name);
                Debug.Log("_curCastEleItem" + _curCastEleItem.Name);
            }
            else
            {
                _curDragEleItem = null;
            }
        }

    }

    private void OnMouseDrag0()
    {
        if (_curDragEleItem != null)
        {
            Vector3 currentMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _goScreenPos.z);
            Vector3 temp = Camera.main.ScreenToWorldPoint(currentMousePos) + _offset;
            _offsetCamera = temp - Camera.main.transform.position;
            _curDragEleItem.TweenCloseToCamera(temp - _offsetCamera / 2);
            //
            if (IsCastTarget())
            {
                //要触碰的物体描边高亮
                UIManager.Instance().ShowPanel(UIManager.PanelType.PanelTipCast);
                _curCastEleItem.SetHighLighting(false);
                _curCastEleItem.IsBeRayCast = true;
            }
            else
            {
                //要触碰的物体描边关闭高亮
                UIManager.Instance().HidePanel(UIManager.PanelType.PanelTipCast);
                _curCastEleItem.OffHighLighting();
                _curCastEleItem.IsBeRayCast = false;
            }
        }
    }

    private void OnMouseUp0()
    {
        //transform.position = transform.position + _offsetCamera / 2;
        if (_curDragEleItem != null && _curCastEleItem != null)
        {
            if (_curCastEleItem.IsBeRayCast)//触碰到了目标
            {
                _curDragEleItem.OffHighLighting();
                _curCastEleItem.OffHighLighting();
                _curDragEleItem.TweenToPos(
                    new Vector3(_curCastEleItem.Go.transform.localPosition.x,
                    _curCastEleItem.Go.transform.localPosition.y + 0.2f,
                    _curCastEleItem.Go.transform.localPosition.z)
                    , _curDragEleItem.PlayAni);
                //_curDragEleItem.PlayAni();
            }
            else
            {
                _curDragEleItem.TweenToPos(_curDragEleItem.DefaultPos);
            }
        }
    }
    private bool IsCastTarget()
    {
        if (_curCastEleItem == null)
            return false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        _hit = Physics.RaycastAll(ray, 100f, (1 << 8));
        if (_hit.Length > 1)
        {
            for (int i = 0; i < _hit.Length; i++)
            {
                RaycastHit hit = _hit[i];
                GameObject go = hit.collider.gameObject;
                if (hit.collider.name.Equals(_curCastEleItem.Name))//if (hit.collider.tag)
                {
                    Debug.Log("触碰到了目标物体");
                    return true;
                    //触碰到了目标物体
                    //播放动画   
                    //动画播放完毕后复位物体
                    //切换到下一步
                }

            }
        }
        return false;
    }
    /// <summary>
    /// 仪器展示
    /// </summary>
    public IEnumerator EleIntroduceMove()
    {
        EleItem item = EleDataBase.Instance().EleItems[_eleIntroduceMoveIndex];
        var tweener = Camera.main.transform.DOMove(item.LookPos, 0.3f);
        tweener.OnStart(() =>
        {
            Camera.main.transform.DORotateQuaternion(item.LookQua, 0.3f);
            if (_eleIntroduceMoveIndex > 0 && _eleIntroduceMoveIndex < EleDataBase.Instance().EleItems.Count)
            {
                EleDataBase.Instance().EleItems[_eleIntroduceMoveIndex - 1].OffHighLighting();
            }
        });
        tweener.OnComplete(() =>
        {
            item.SetHighLighting(true);
            _eleIntroduceMoveIndex++;
        });
        yield return new WaitForSeconds(2f);
        if (_eleIntroduceMoveIndex < EleDataBase.Instance().EleItems.Count)
        {
            StartCoroutine(EleIntroduceMove());
        }
        else
        {
            Camera.main.transform.DOMove(DefaultTran.position, 0.3f).OnStart(
                () => {
                    Camera.main.transform.DORotateQuaternion(DefaultTran.rotation, 0.3f);
                    EleDataBase.Instance().EleItems[_eleIntroduceMoveIndex - 1].OffHighLighting();
                }).OnComplete(
                ()=>StepManager.Instance().ChangeGameState(StepManager.StepState.Gameing));
        }
    }

    private void OnDestroy()
    {
        EventBase.Instance().StepToNext -= StepNext;
        EventBase.Instance().DoEleIntroduceMove -= DoEleIntroduceMove;
    }


}
